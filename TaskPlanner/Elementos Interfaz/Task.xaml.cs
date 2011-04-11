using System;
using System.Windows.Forms;

namespace TaskPlanner
{
    public partial class Task : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int task_ID;

        #region Constructores
            public Task()
            {
                InitializeComponent();
            }
            public Task(Formulario Padre, MySQL_DB Connection, int ID)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.task_ID = ID;

                Connection.ChangeTable("TAR_TAREAS", null);
                Connection.ChangeSelect("SELECT * FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                if (Connection.Conectar())
                {
                    if (!(string.IsNullOrEmpty(Connection.getData(0, 4)))) BTN_COMPLETADO.Visibility = System.Windows.Visibility.Hidden;
                    LBL_TITULO.Content = Connection.getData(0, 1);
                    char[] del = { '\n' }; int len = Connection.getData(0, 2).Split(del, StringSplitOptions.None).GetValue(0).ToString().Length;
                    LBL_PREVIEW.Content = Connection.getData(0, 2).Split(del, StringSplitOptions.None).GetValue(0).ToString().Substring(0, (len >= 50 ? 50 : len)); if (len > 50) LBL_PREVIEW.Content += "...";
                    for (int x = 0; x < int.Parse(Connection.getData(0, 6)); x++) LBL_PRIORIDAD.Content += "!";
                    Connection.Desconectar();
                    LBL_ASIGNATURA.Content = "";
                    if (int.Parse(Connection.getData(0, 5)) != -1)
                    {
                        Connection.ChangeSelect("SELECT ASG_NOMBRE FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + Connection.getData(0, 5));
                        Connection.ChangeTable("ASG_ASIGNATURAS", null);
                        if (Connection.Conectar()) Connection.Desconectar();
                        LBL_ASIGNATURA.Content = Connection.getData(0, 0);
                    }
                }
            }
        #endregion

        #region Métodos
            private void BTN_TAREA_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new TaskNew(Padre, Connection, task_ID);
            }
            private void BTN_COMPLETADO_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (MessageBox.Show("¿Estás seguro de que quieres completar la tarea?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Connection.ChangeTable("TAR_TAREAS", null);
                    Connection.ChangeSelect("SELECT * FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                    if (Connection.Conectar())
                    {
                        Connection.Update_String("UPDATE TAR_TAREAS SET TAR_F_COMPLETADA='" + DateTime.Now.ToString("yyyyMMdd") + "' WHERE TAR_ID_PK = " + this.task_ID.ToString());
                        Connection.Update_Execute();
                        Connection.Desconectar();
                    }
                    Padre.elementHost.Child = new TaskList(Padre, Connection, 0);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_ELIMINAR_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (MessageBox.Show("¿Estás seguro de que quieres borrar la tarea?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Connection.ChangeTable("TAR_TAREAS", null);
                    Connection.ChangeSelect("SELECT * FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                    if (Connection.Conectar())
                    {
                        Connection.Delete_String("DELETE FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                        Connection.Delete_Execute();
                        Connection.Desconectar();
                    }
                    Padre.elementHost.Child = new TaskList(Padre, Connection, 0);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion
    }
}