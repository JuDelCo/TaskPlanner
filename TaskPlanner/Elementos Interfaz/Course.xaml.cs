using System;
using System.Windows.Forms;

namespace TaskPlanner
{
    public partial class Course : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int course_ID;

        #region Constructores
            public Course()
            {
                InitializeComponent();
            }
            public Course(Formulario Padre, MySQL_DB Connection, int ID)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.course_ID = ID;

                Connection.ChangeTable("CUR_CURSOS", null);
                Connection.ChangeSelect("SELECT * FROM CUR_CURSOS WHERE CUR_ID_PK = " + this.course_ID.ToString());
                if (Connection.Conectar())
                {
                    LBL_TITULO.Content = Connection.getData(0, 2);
                    LBL_FECHAS.Content = DateTime.Parse(Connection.getData(0, 4)).ToShortDateString() + " - " + DateTime.Parse(Connection.getData(0, 5)).ToShortDateString();
                    Connection.Desconectar();
                }
            }
        #endregion

        #region Métodos
            private void BTN_CURSO_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerNewCourse(Padre, Connection, course_ID);
            }
            private void BTN_ELIMINAR_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (MessageBox.Show("¡Atención!\n¿Estás seguro de que quieres borrar el curso?\nTen en cuenta que todas las asignaturas, horarios y tareas vinculadas con el mismo también se borrarán !", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                    Connection.ChangeTable("CUR_CURSOS", null);
                    Connection.ChangeSelect("SELECT * FROM CUR_CURSOS WHERE CUR_ID_PK = " + this.course_ID.ToString());
                    if (Connection.Conectar())
                    {
                        Connection.Delete_String("DELETE FROM CUR_CURSOS WHERE CUR_ID_PK = " + this.course_ID.ToString());
                        Connection.Delete_Execute();
                        Connection.Desconectar();
                    }
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK = " + this.course_ID.ToString());
                    if (Connection.Conectar()) Connection.Desconectar();
                    int iterations = Connection.countRows();
                    if (iterations > 0)
                    {
                        for (int x = 0; x < iterations; x++)
                        {
                            Connection.ChangeTable("ASG_ASIGNATURAS", null);
                            Connection.ChangeSelect("SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK = " + this.course_ID.ToString());
                            if (Connection.Conectar())
                            {
                                string asg_ID = Connection.getData(0, 0);

                                Connection.Delete_String("DELETE FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + asg_ID);
                                Connection.Delete_Execute();
                                Connection.Delete_String("DELETE FROM HOR_HORARIOS WHERE HOR_ASG_ID_PK = " + asg_ID);
                                Connection.Delete_Execute();
                                Connection.Delete_String("DELETE FROM TAR_TAREAS WHERE TAR_ASG_ID_PK = " + asg_ID);
                                Connection.Delete_Execute();

                                Connection.Desconectar();
                            }
                        }
                    }
                    Padre.elementHost.Child = new OrganizerCourses(Padre, Connection, false);
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion
    }
}