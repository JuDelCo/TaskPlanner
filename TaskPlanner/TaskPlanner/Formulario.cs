using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace TaskPlanner
{
    public partial class Formulario : Form
    {
        #region Variables
            private int usr_ID = -1;
            public int getUsrID() { return this.usr_ID; }
            public void setUsrID(int usr_ID) { this.usr_ID = usr_ID; }
            public MySQL_DB MySQL_Connection;
        #endregion

        #region Constructor
            public Formulario()
            {
                InitializeComponent();

                MySQL_Connection = new MySQL_DB();
                MySQL_Connection.SetConnection("db4free.net", "3306", "taskuser", "task747", "taskplanner");
                //MySQL_Connection.debug = true;
                this.PANEL.Controls.Add(this.elementHost);
                this.elementHost.Dock = System.Windows.Forms.DockStyle.Fill;
                this.elementHost.Child = new Login(this, MySQL_Connection);
            }
        #endregion

        #region Métodos
            private void menuHoy_Click(object sender, EventArgs e)
            {
                this.elementHost.Child = new Today(this, MySQL_Connection);
            }
            private void menuTareas_Click(object sender, EventArgs e)
            {
                this.elementHost.Child = new TaskList(this, MySQL_Connection, 0);
            }
            private void menuPlanificador_Click(object sender, EventArgs e)
            {
                this.elementHost.Child = new Organizer(this, MySQL_Connection);
            }
            private void menuPerfil_Click(object sender, EventArgs e)
            {
                this.elementHost.Child = new Profile(this, MySQL_Connection);
            }
            private void BTN_Exit_Click(object sender, EventArgs e)
            {
                if (MessageBox.Show("¿Estás seguro de que quieres desloguear?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.menuHoy.Visible = false;
                    this.menuTareas.Visible = false;
                    this.menuPlanificador.Visible = false;
                    this.menuPerfil.Visible = false;
                    this.TXT_USUARIO.Text = "";
                    this.usr_ID = -1;
                    this.elementHost.Child = new Login(this, MySQL_Connection);
                }
            }
            private void Formulario_FormClosing(object sender, FormClosingEventArgs e)
            {
                if (MessageBox.Show("¿Estás seguro de que quieres cerrar el programa?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    e.Cancel = true;
            }
        #endregion
    }
}

#region Información (Comentarios)
/*
http://stackoverflow.com/questions/2110148/wpf-groupbox-with-no-header-space
http://geeks.ms/wikis/wpf/wpf-personalizar-nuestra-aplicaci-243-n-con-styles-y-control-templates-i.aspx

LISTA.CanVerticallyScroll = true; //Cuando supere un número MUY grande de elementos

this.TaskRectangle.Fill = System.Windows.Media.Brushes.Aqua;

alter table TABLA auto_increment=1

http://www.regexlib.com/REDetails.aspx?regexp_id=708
http://luauf.com/2008/04/18/sqlite-con-c/
*/
#endregion