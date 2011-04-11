using System;
using System.Windows.Forms;

namespace TaskPlanner
{
    public partial class Subject : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int subject_ID;

        #region Constructores
            public Subject()
            {
                InitializeComponent();
            }
            public Subject(Formulario Padre, MySQL_DB Connection, int ID)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.subject_ID = ID;

                Connection.ChangeTable("ASG_ASIGNATURAS", null);
                Connection.ChangeSelect("SELECT * FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + this.subject_ID.ToString());
                if (Connection.Conectar())
                {
                    LBL_ASIGNATURA.Content = Connection.getData(0, 2);
                    LBL_LUGAR.Content = Connection.getData(0, 3);
                    Connection.Desconectar();
                    Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_ID_PK = " + Connection.getData(0, 1));
                    Connection.ChangeTable("CUR_CURSOS", null);
                    if (Connection.Conectar()) Connection.Desconectar();
                    LBL_CURSO.Content = Connection.getData(0, 0);
                }
            }
        #endregion

        #region Métodos
            private void BTN_HORARIO_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerNewSubjects(Padre, Connection, subject_ID);
            }
            private void BTN_ELIMINAR_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (MessageBox.Show("¡Atención!\n¿Estás seguro de que quieres borrar la asignatura?\nTen en cuenta que todos los horarios y tareas vinculadas con la misma también se borrarán !", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT * FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + this.subject_ID.ToString());
                    if (Connection.Conectar())
                    {
                        Connection.Delete_String("DELETE FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + this.subject_ID.ToString());
                        Connection.Delete_Execute();
                        Connection.Delete_String("DELETE FROM HOR_HORARIOS WHERE HOR_ASG_ID_PK = " + this.subject_ID.ToString());
                        Connection.Delete_Execute();
                        Connection.Delete_String("DELETE FROM TAR_TAREAS WHERE TAR_ASG_ID_PK = " + this.subject_ID.ToString());
                        Connection.Delete_Execute();
                        Connection.Desconectar();
                    }
                    Padre.elementHost.Child = new OrganizerSubjects(Padre, Connection, -1);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion
    }
}