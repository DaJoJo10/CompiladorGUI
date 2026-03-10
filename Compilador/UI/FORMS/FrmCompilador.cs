
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
            txtTokens.Clear();
            txtEstatus.Clear();

            // Mensaje inicial
            txtEstatus.AppendText("Ha iniciado el léxico" + Environment.NewLine);

            // Obtener el texto del editor
            var fuente = CodigoFuente.DesdeTexto(txtEditor.Text);
            var analizador = new AnalizadorLexico();
            var resultado = analizador.Analizar(fuente);

            // Mostrar tokens en txtTokens
            foreach (var token in resultado.Tokens)
            {
                txtTokens.AppendText($"Token: {token.Tipo} | Lexema: {token.Lexema} | Línea: {token.Linea}" + Environment.NewLine);
            }

            // Mostrar avisos en txtEstatus
            foreach (var aviso in resultado.Avisos)
            {
                txtEstatus.AppendText(aviso + Environment.NewLine);
            }
        }
    }
}
