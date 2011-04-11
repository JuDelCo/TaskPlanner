namespace TaskPlanner
{
    partial class Formulario
    {
        /// <summary>
        /// Variable del diseñador requerida.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén utilizando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben eliminar; false en caso contrario, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido del método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Formulario));
            this.MenuTask = new System.Windows.Forms.MenuStrip();
            this.menuHoy = new System.Windows.Forms.ToolStripMenuItem();
            this.menuTareas = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPlanificador = new System.Windows.Forms.ToolStripMenuItem();
            this.menuPerfil = new System.Windows.Forms.ToolStripMenuItem();
            this.PANEL = new System.Windows.Forms.Panel();
            this.TXT_USUARIO = new System.Windows.Forms.Label();
            this.BTN_Exit = new System.Windows.Forms.Button();
            this.MenuTask.SuspendLayout();
            this.SuspendLayout();
            // 
            // MenuTask
            // 
            this.MenuTask.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.MenuTask.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuHoy,
            this.menuTareas,
            this.menuPlanificador,
            this.menuPerfil});
            this.MenuTask.Location = new System.Drawing.Point(0, 0);
            this.MenuTask.Name = "MenuTask";
            this.MenuTask.Size = new System.Drawing.Size(470, 24);
            this.MenuTask.TabIndex = 1;
            this.MenuTask.Text = "menuStrip1";
            // 
            // menuHoy
            // 
            this.menuHoy.Name = "menuHoy";
            this.menuHoy.Size = new System.Drawing.Size(41, 20);
            this.menuHoy.Text = "&Hoy";
            this.menuHoy.Visible = false;
            this.menuHoy.Click += new System.EventHandler(this.menuHoy_Click);
            // 
            // menuTareas
            // 
            this.menuTareas.Name = "menuTareas";
            this.menuTareas.Size = new System.Drawing.Size(53, 20);
            this.menuTareas.Text = "&Tareas";
            this.menuTareas.Visible = false;
            this.menuTareas.Click += new System.EventHandler(this.menuTareas_Click);
            // 
            // menuPlanificador
            // 
            this.menuPlanificador.Name = "menuPlanificador";
            this.menuPlanificador.Size = new System.Drawing.Size(82, 20);
            this.menuPlanificador.Text = "&Planificador";
            this.menuPlanificador.Visible = false;
            this.menuPlanificador.Click += new System.EventHandler(this.menuPlanificador_Click);
            // 
            // menuPerfil
            // 
            this.menuPerfil.Name = "menuPerfil";
            this.menuPerfil.Size = new System.Drawing.Size(46, 20);
            this.menuPerfil.Text = "P&erfil";
            this.menuPerfil.Visible = false;
            this.menuPerfil.Click += new System.EventHandler(this.menuPerfil_Click);
            // 
            // PANEL
            // 
            this.PANEL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PANEL.Location = new System.Drawing.Point(0, 24);
            this.PANEL.Name = "PANEL";
            this.PANEL.Size = new System.Drawing.Size(470, 248);
            this.PANEL.TabIndex = 2;
            // 
            // TXT_USUARIO
            // 
            this.TXT_USUARIO.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.TXT_USUARIO.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TXT_USUARIO.Location = new System.Drawing.Point(254, 8);
            this.TXT_USUARIO.Name = "TXT_USUARIO";
            this.TXT_USUARIO.Size = new System.Drawing.Size(131, 17);
            this.TXT_USUARIO.TabIndex = 3;
            this.TXT_USUARIO.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // BTN_Exit
            // 
            this.BTN_Exit.Location = new System.Drawing.Point(385, 4);
            this.BTN_Exit.Name = "BTN_Exit";
            this.BTN_Exit.Size = new System.Drawing.Size(73, 22);
            this.BTN_Exit.TabIndex = 4;
            this.BTN_Exit.Text = "Desloguear";
            this.BTN_Exit.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BTN_Exit.UseVisualStyleBackColor = false;
            this.BTN_Exit.Visible = false;
            this.BTN_Exit.Click += new System.EventHandler(this.BTN_Exit_Click);
            // 
            // Formulario
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(470, 272);
            this.Controls.Add(this.BTN_Exit);
            this.Controls.Add(this.PANEL);
            this.Controls.Add(this.TXT_USUARIO);
            this.Controls.Add(this.MenuTask);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuTask;
            this.MaximizeBox = false;
            this.Name = "Formulario";
            this.Text = "Task Planner";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Formulario_FormClosing);
            this.MenuTask.ResumeLayout(false);
            this.MenuTask.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Panel PANEL;
        public System.Windows.Forms.Integration.ElementHost elementHost = new System.Windows.Forms.Integration.ElementHost();
        public System.Windows.Forms.MenuStrip MenuTask;
        public System.Windows.Forms.Label TXT_USUARIO;
        public System.Windows.Forms.ToolStripMenuItem menuTareas;
        public System.Windows.Forms.ToolStripMenuItem menuPlanificador;
        public System.Windows.Forms.ToolStripMenuItem menuPerfil;
        public System.Windows.Forms.ToolStripMenuItem menuHoy;
        public System.Windows.Forms.Button BTN_Exit;
    }
}

