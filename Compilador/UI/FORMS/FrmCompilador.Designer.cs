namespace Compilador.UI.Forms
{
    partial class FrmCompilador
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();

            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            toolStrip1 = new ToolStrip();
            btnNuevo = new ToolStripButton();
            btnAbrir = new ToolStripButton();
            btnGuardar = new ToolStripButton();
            btnCompilar = new ToolStripButton();
            btnTema = new ToolStripButton();
            btnSalir = new ToolStripButton();
            splitMain = new SplitContainer();
            splitEditor = new SplitContainer();
            txtTokens = new TextBox();
            splitBottom = new SplitContainer();
            txtEstatus = new TextBox();
            gridSimbolos = new DataGridView();
            openFileDialog1 = new OpenFileDialog();
            saveFileDialog1 = new SaveFileDialog();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitMain).BeginInit();
            splitMain.Panel1.SuspendLayout();
            splitMain.Panel2.SuspendLayout();
            splitMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitEditor).BeginInit();
            splitEditor.Panel2.SuspendLayout();
            splitEditor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitBottom).BeginInit();
            splitBottom.Panel1.SuspendLayout();
            splitBottom.Panel2.SuspendLayout();
            splitBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)gridSimbolos).BeginInit();
            SuspendLayout();
            // 
            // toolStrip1
            // 
            toolStrip1.GripStyle = ToolStripGripStyle.Hidden;
            toolStrip1.ImageScalingSize = new Size(24, 24);
            toolStrip1.Items.AddRange(new ToolStripItem[] { btnNuevo, btnAbrir, btnGuardar, btnCompilar, btnTema, btnSalir });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(983, 58);
            toolStrip1.TabIndex = 1;
            // 
            // btnNuevo
            // 
            btnNuevo.Image = Properties.Resources._new;
            btnNuevo.Name = "btnNuevo";
            btnNuevo.Size = new Size(68, 53);
            btnNuevo.Text = "Nuevo";
            btnNuevo.TextImageRelation = TextImageRelation.ImageAboveText;
            btnNuevo.Click += btnNuevo_Click;
            // 
            // btnAbrir
            // 
            btnAbrir.Image = Properties.Resources.open;
            btnAbrir.Name = "btnAbrir";
            btnAbrir.Size = new Size(55, 53);
            btnAbrir.Text = "Abrir";
            btnAbrir.TextImageRelation = TextImageRelation.ImageAboveText;
            btnAbrir.Click += btnAbrir_Click;
            // 
            // btnGuardar
            // 
            btnGuardar.Image = Properties.Resources.save;
            btnGuardar.Name = "btnGuardar";
            btnGuardar.Size = new Size(79, 53);
            btnGuardar.Text = "Guardar";
            btnGuardar.TextImageRelation = TextImageRelation.ImageAboveText;
            btnGuardar.Click += btnGuardar_Click;
            // 
            // btnCompilar
            // 
            btnCompilar.Image = Properties.Resources.compilar;
            btnCompilar.Name = "btnCompilar";
            btnCompilar.Size = new Size(88, 53);
            btnCompilar.Text = "Compilar";
            btnCompilar.TextImageRelation = TextImageRelation.ImageAboveText;
            btnCompilar.Click += btnCompilar_Click;
            // 
            // btnTema
            // 
            btnTema.Image = Properties.Resources.icon_nuevo;
            btnTema.Name = "btnTema";
            btnTema.Size = new Size(57, 53);
            btnTema.Text = "Tema";
            btnTema.TextImageRelation = TextImageRelation.ImageAboveText;
            // 
            // btnSalir
            // 
            btnSalir.Image = Properties.Resources.exit;
            btnSalir.Name = "btnSalir";
            btnSalir.Size = new Size(49, 53);
            btnSalir.Text = "Salir";
            btnSalir.TextImageRelation = TextImageRelation.ImageAboveText;
            // 
            // splitMain
            // 
            splitMain.Dock = DockStyle.Fill;
            splitMain.Location = new Point(0, 58);
            splitMain.Name = "splitMain";
            splitMain.Orientation = Orientation.Horizontal;
            // 
            // splitMain.Panel1
            // 
            splitMain.Panel1.Controls.Add(splitEditor);
            // 
            // splitMain.Panel2
            // 
            splitMain.Panel2.Controls.Add(splitBottom);
            splitMain.Size = new Size(983, 582);
            splitMain.SplitterDistance = 412;
            splitMain.TabIndex = 0;
            // 
            // splitEditor
            // 
            splitEditor.Dock = DockStyle.Fill;
            splitEditor.Location = new Point(0, 0);
            splitEditor.Name = "splitEditor";
            // 
            // splitEditor.Panel2
            // 
            splitEditor.Panel2.Controls.Add(txtTokens);
            splitEditor.Size = new Size(983, 412);
            splitEditor.SplitterDistance = 792;
            splitEditor.TabIndex = 0;
            // 
            // txtTokens
            // 
            txtTokens.BackColor = SystemColors.Control;
            txtTokens.Dock = DockStyle.Fill;
            txtTokens.Font = new Font("Consolas", 10F);
            txtTokens.Location = new Point(0, 0);
            txtTokens.Multiline = true;
            txtTokens.Name = "txtTokens";
            txtTokens.ReadOnly = true;
            txtTokens.ScrollBars = ScrollBars.Vertical;
            txtTokens.Size = new Size(187, 412);
            txtTokens.TabIndex = 0;
            txtTokens.Text = "Listado de tokens";
            // 
            // splitBottom
            // 
            splitBottom.Dock = DockStyle.Fill;
            splitBottom.Location = new Point(0, 0);
            splitBottom.Name = "splitBottom";
            // 
            // splitBottom.Panel1
            // 
            splitBottom.Panel1.Controls.Add(txtEstatus);
            // 
            // splitBottom.Panel2
            // 
            splitBottom.Panel2.Controls.Add(gridSimbolos);
            splitBottom.Size = new Size(983, 166);
            splitBottom.SplitterDistance = 792;
            splitBottom.TabIndex = 0;
            // 
            // txtEstatus
            // 
            txtEstatus.Dock = DockStyle.Fill;
            txtEstatus.Font = new Font("Consolas", 10F);
            txtEstatus.Location = new Point(0, 0);
            txtEstatus.Multiline = true;
            txtEstatus.Name = "txtEstatus";
            txtEstatus.ReadOnly = true;
            txtEstatus.ScrollBars = ScrollBars.Vertical;
            txtEstatus.Size = new Size(792, 166);
            txtEstatus.TabIndex = 0;
            txtEstatus.Text = "Estatus del compilador";
            // 
            // gridSimbolos
            // 
            gridSimbolos.AllowUserToAddRows = false;
            gridSimbolos.AllowUserToDeleteRows = false;
            gridSimbolos.ColumnHeadersHeight = 29;
            gridSimbolos.Dock = DockStyle.Fill;
            gridSimbolos.Location = new Point(0, 0);
            gridSimbolos.Name = "gridSimbolos";
            gridSimbolos.ReadOnly = true;
            gridSimbolos.RowHeadersVisible = false;
            gridSimbolos.RowHeadersWidth = 51;
            gridSimbolos.Size = new Size(187, 166);
            gridSimbolos.TabIndex = 0;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // FrmCompilador
            // 
            ClientSize = new Size(983, 640);
            Controls.Add(splitMain);
            Controls.Add(toolStrip1);
            Name = "FrmCompilador";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Compilador";
            WindowState = FormWindowState.Maximized;
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitMain.Panel1.ResumeLayout(false);
            splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitMain).EndInit();
            splitMain.ResumeLayout(false);
            splitEditor.Panel2.ResumeLayout(false);
            splitEditor.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitEditor).EndInit();
            splitEditor.ResumeLayout(false);
            splitBottom.Panel1.ResumeLayout(false);
            splitBottom.Panel1.PerformLayout();
            splitBottom.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitBottom).EndInit();
            splitBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)gridSimbolos).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip toolStrip1;
        private ToolStripButton btnNuevo;
        private ToolStripButton btnAbrir;
        private ToolStripButton btnGuardar;
        private ToolStripButton btnCompilar;
        private ToolStripButton btnTema;
        private ToolStripButton btnSalir;

        private SplitContainer splitMain;
        private SplitContainer splitEditor;
        private SplitContainer splitBottom;

        private TextBox txtTokens;
        private TextBox txtEstatus;
        private DataGridView gridSimbolos;
        private OpenFileDialog openFileDialog1;
        private SaveFileDialog saveFileDialog1;
    }
}
