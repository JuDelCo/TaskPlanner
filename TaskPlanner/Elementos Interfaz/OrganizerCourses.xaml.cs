using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class OrganizerCourses : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private bool old_courses = false;

        #region Constructores
            public OrganizerCourses()
            {
                InitializeComponent();
            }
            public OrganizerCourses(Formulario Padre, MySQL_DB Connection, bool old_courses)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.old_courses = old_courses;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                LISTA.Children.Add(new TaskBlankSpace());
                if (old_courses)
                {
                    BTN_OLDCOURSES.Content = "Cursos actuales";
                    Connection.ChangeTable("CUR_CURSOS", null);
                    Connection.ChangeSelect("SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_F_FINAL < DATE_FORMAT(NOW(),'%Y-%m-%d') AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + " ORDER BY CUR_NOMBRE ASC");
                    if (Connection.Conectar()) Connection.Desconectar();
                    int num_courses = Connection.countRows();
                    if (num_courses > 0)
                        for (int x = 0; x < num_courses; x++)
                        {
                            Connection.ChangeTable("CUR_CURSOS", null);
                            Connection.ChangeSelect("SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_F_FINAL < DATE_FORMAT(NOW(),'%Y-%m-%d') AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + " ORDER BY CUR_NOMBRE ASC");
                            if (Connection.Conectar()) Connection.Desconectar();
                            LISTA.Children.Add(new Course(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                        }
                    else LBL_NOTFOUND.Visibility = Visibility.Visible;
                }
                else
                {
                    Connection.ChangeTable("CUR_CURSOS", null);
                    Connection.ChangeSelect("SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_F_FINAL >= DATE_FORMAT(NOW(),'%Y-%m-%d') AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + " ORDER BY CUR_NOMBRE ASC");
                    if (Connection.Conectar()) Connection.Desconectar();
                    int num_courses = Connection.countRows();
                    if (num_courses > 0)
                        for (int x = 0; x < num_courses; x++)
                        {
                            Connection.ChangeTable("CUR_CURSOS", null);
                            Connection.ChangeSelect("SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_F_FINAL >= DATE_FORMAT(NOW(),'%Y-%m-%d') AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + " ORDER BY CUR_NOMBRE ASC");
                            if (Connection.Conectar()) Connection.Desconectar();
                            LISTA.Children.Add(new Course(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                        }
                    else LBL_NOTFOUND.Visibility = Visibility.Visible;
                }
                LISTA.Children.Add(new TaskBlankSpace());
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_ADDNEWCOURSE_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerNewCourse(Padre, Connection, -1);
            }
            private void BTN_OLDCOURSES_Click(object sender, RoutedEventArgs e)
            {
                if (old_courses) Padre.elementHost.Child = new OrganizerCourses(Padre, Connection, false);
                else Padre.elementHost.Child = new OrganizerCourses(Padre, Connection, true);
            }
        #endregion
    }
}
