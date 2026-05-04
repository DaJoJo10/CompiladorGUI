
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Windows.Forms;
using System;
using System.Drawing;
using System.Windows.Forms;
using Compilador.UI.FORMS;

namespace Compilador.UI.Forms
{
    public partial class FrmCompilador : Form
    {
        private RichTextBox txtEditor;
        private Panel pnlLineNumbers;
        private bool isDarkTheme = false;

        public FrmCompilador()
        {
            InitializeComponent();
            InicializarEditor();
            AplicarTemaClaro();

            btnTema.Click += BtnTema_Click;
        }


        private void InicializarEditor()
        {
            pnlLineNumbers = new Panel
            {
                Dock = DockStyle.Left,
                Width = 50
            };
            pnlLineNumbers.Paint += PnlLineNumbers_Paint;

            txtEditor = new RichTextBox
            {
                Dock = DockStyle.Fill,
                Font = new Font("Consolas", 11F),
                WordWrap = false,
                BorderStyle = BorderStyle.None,
                ScrollBars = RichTextBoxScrollBars.Vertical
            };

            txtEditor.VScroll += (s, e) => pnlLineNumbers.Invalidate();
            txtEditor.TextChanged += (s, e) => pnlLineNumbers.Invalidate();
            txtEditor.Resize += (s, e) => pnlLineNumbers.Invalidate();

            splitEditor.Panel1.Controls.Add(txtEditor);
            splitEditor.Panel1.Controls.Add(pnlLineNumbers);
        }


        private void PnlLineNumbers_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(pnlLineNumbers.BackColor);

            int firstLine = txtEditor.GetLineFromCharIndex(
                txtEditor.GetCharIndexFromPosition(new Point(0, 0)));

            int lastLine = txtEditor.GetLineFromCharIndex(
                txtEditor.GetCharIndexFromPosition(
                    new Point(0, txtEditor.Height)));

            int lineHeight = txtEditor.Font.Height;
            int y = 2;

            for (int i = firstLine; i <= lastLine + 1; i++)
            {
                string line = (i + 1).ToString();
                SizeF size = e.Graphics.MeasureString(line, txtEditor.Font);

                e.Graphics.DrawString(
                    line,
                    txtEditor.Font,
                    Brushes.Gray,
                    pnlLineNumbers.Width - size.Width - 5,
                    y
                );

                y += lineHeight;
            }
        }


        private void BtnTema_Click(object sender, EventArgs e)
        {
            isDarkTheme = !isDarkTheme;

            if (isDarkTheme)
                AplicarTemaOscuro();
            else
                AplicarTemaClaro();
        }

        private void AplicarTemaClaro()
        {
            txtEditor.BackColor = Color.White;
            txtEditor.ForeColor = Color.Black;
            txtTokens.BackColor = Color.White;
            txtTokens.ForeColor = Color.Black;
            txtEstatus.BackColor = Color.White;
            txtEstatus.ForeColor = Color.Black;
            gridSimbolos.BackgroundColor = Color.White;
            gridSimbolos.DefaultCellStyle.BackColor = Color.White;
            gridSimbolos.DefaultCellStyle.ForeColor = Color.Black;
            pnlLineNumbers.BackColor = Color.FromArgb(245, 245, 245);
        }

        private void AplicarTemaOscuro()
        {
            txtEditor.BackColor = Color.FromArgb(30, 30, 30);
            txtEditor.ForeColor = Color.Gainsboro;
            txtTokens.BackColor = Color.FromArgb(30, 30, 30);
            txtTokens.ForeColor = Color.Gainsboro;
            txtEstatus.BackColor = Color.FromArgb(30, 30, 30);
            txtEstatus.ForeColor = Color.Gainsboro;
            gridSimbolos.BackgroundColor = Color.FromArgb(30, 30, 30);
            gridSimbolos.DefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            gridSimbolos.DefaultCellStyle.ForeColor = Color.Gainsboro;
            pnlLineNumbers.BackColor = Color.FromArgb(45, 45, 48);
        }

        private void btnNuevo_Click(object sender, EventArgs e)
        {
            limpiar();
        }
        private void limpiar()
        {
            try
            {
                txtEditor.Clear();
                txtTokens.Clear();
                txtEstatus.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al limpiar los campos: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            try
            {
                openFileDialog1.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    limpiar();
                    string filePath = openFileDialog1.FileName;
                    string fileContent = File.ReadAllText(filePath);
                    txtEditor.Text = fileContent;

                    txtEstatus.AppendText("Archivo abierto: " + filePath + Environment.NewLine);
                }
                else
                {
                    txtEstatus.AppendText("Operación de apertura cancelada." + Environment.NewLine);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al abrir el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            try
            {
                saveFileDialog1.Filter = "Archivos de texto (*.txt)|*.txt|Todos los archivos (*.*)|*.*";
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string filePath = saveFileDialog1.FileName;
                    File.WriteAllText(filePath, txtEditor.Text);
                    txtEstatus.AppendText("Archivo guardado: " + filePath + Environment.NewLine);
                }
                else
                {
                    txtEstatus.AppendText("Operación de guardado cancelada." + Environment.NewLine);
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar el archivo: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnCompilar_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Limpiar resultados anteriores
                txtTokens.Clear();
                txtEstatus.Clear();

                // 2. Obtener el código fuente del editor
                var fuente = CodigoFuente.DesdeTexto(txtEditor.Text);

                // 3. Ejecutar el analizador léxico
                var analizador = new AnalizadorLexico();
                var resultado = analizador.Analizar(fuente);

                // 4. Mostrar avisos en txtEstatus
                foreach (var aviso in resultado.Avisos)
                    txtEstatus.AppendText(aviso + Environment.NewLine);

                txtEstatus.AppendText("Fase 2 [Sintáctico] INICIADO" + Environment.NewLine);

                var sintactico = new AnalizadorSintactico();
                sintactico.Parse(resultado.Tokens);

                if (sintactico.Errores.Count == 0)
                {
                    txtEstatus.AppendText("Análisis Sintáctico finalizado con éxito" + Environment.NewLine);
                }
                else
                {
                    foreach (var error in sintactico.Errores)
                    {
                        txtEstatus.AppendText(error + Environment.NewLine);
                    }
                }

                // 5. Agrupar tokens por número de línea y mostrarlos
                //    Formato: [numLinea] [tok1] [tok2] ...
                var porLinea = new SortedDictionary<int, System.Collections.Generic.List<int>>();
                foreach (var token in resultado.Tokens)
                {
                    if (!porLinea.ContainsKey(token.Linea))
                        porLinea[token.Linea] = new System.Collections.Generic.List<int>();
                    porLinea[token.Linea].Add(token.Tipo);
                }

                foreach (var kvp in porLinea)
                {
                    var sb = new System.Text.StringBuilder();
                    sb.Append($"[{kvp.Key}]");
                    foreach (int tipo in kvp.Value)
                        sb.Append($" [{tipo}]");
                    txtTokens.AppendText(sb.ToString() + Environment.NewLine);
                }

                txtTokens.AppendText(Environment.NewLine);
                txtTokens.AppendText($"Total de tokens: {resultado.Tokens.Count}" + Environment.NewLine);

                txtEstatus.AppendText($"Compilación exitosa. Tokens encontrados: {resultado.Tokens.Count}" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string ObtenerNombreTipo(int tipo)
        {
            switch (tipo)
            {
                case Token.ID: return "ID";
                case Token.VAR: return "VAR";
                case Token.INT: return "INT";
                case Token.FLOAT_KW: return "FLOAT";
                case Token.IF: return "IF";
                case Token.THEN: return "THEN";
                case Token.ELSE: return "ELSE";
                case Token.WHILE: return "WHILE";
                case Token.PRINT: return "PRINT";
                case Token.NUM_INT: return "NUM_INT";
                case Token.NUM_REAL: return "NUM_REAL";
                case Token.SEMICOLON: return "SEMICOLON";
                case Token.ASSIGN: return "ASSIGN";
                case Token.EQ: return "EQ";
                case Token.DIVIDE: return "DIVIDE";
                case Token.PLUS: return "PLUS";
                case Token.MINUS: return "MINUS";
                case Token.MULTIPLY: return "MULTIPLY";
                case Token.GT: return "GT";
                case Token.GTE: return "GTE";
                case Token.LT: return "LT";
                case Token.LTE: return "LTE";
                case Token.COLON: return "COLON";
                case Token.LPAREN: return "LPAREN";
                case Token.RPAREN: return "RPAREN";
                case Token.LBRACE: return "LBRACE";
                case Token.RBRACE: return "RBRACE";
                case Token.COMMA: return "COMMA";
                case Token.QUOTE: return "QUOTE";
                case Token.COMMENT: return "COMMENT";
                default: return tipo >= 500 ? "ERROR" : "DESCONOCIDO";
            }
        }

        private void txtTokens_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
