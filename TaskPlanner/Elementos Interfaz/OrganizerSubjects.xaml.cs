using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class OrganizerSubjects : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int course_ID = -1;
        private bool first_time = true;

        #region Constructores
            public OrganizerSubjects()
            {
                InitializeComponent();
            }
            public OrganizerSubjects(Formulario Padre, MySQL_DB Connection, int course_ID)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.course_ID = course_ID;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                Connection.ChangeTable("CUR_CURSOS", null);
                Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString());
                if (Connection.Conectar()) Connection.Desconectar();
                for (int x = 0; x < Connection.countRows(); x++) FILTRO.Items.Add(Connection.getData(x, 0));

                LISTA.Children.Add(new TaskBlankSpace());
                if (course_ID == -1)
                {
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK IN (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ") ORDER BY ASG_CUR_ID_PK ASC, ASG_NOMBRE ASC");
                    if (Connection.Conectar()) Connection.Desconectar();
                    int num_subjects = Connection.countRows();
                    if (num_subjects > 0)
                        for (int x = 0; x < num_subjects; x++)
                        {
                            Connection.ChangeTable("ASG_ASIGNATURAS", null);
                            Connection.ChangeSelect("SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK IN (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ") ORDER BY ASG_CUR_ID_PK ASC, ASG_NOMBRE ASC");
                            if (Connection.Conectar()) Connection.Desconectar();
                            LISTA.Children.Add(new Subject(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                        }
                    else LBL_NOTFOUND.Visibility = Visibility.Visible;
                }
                else
                {
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK = " + this.course_ID.ToString() + " ORDER BY ASG_CUR_ID_PK ASC, ASG_NOMBRE ASC");
                    if (Connection.Conectar()) Connection.Desconectar();
                    int num_subjects = Connection.countRows();
                    if (num_subjects > 0)
                        for (int x = 0; x < num_subjects; x++)
                        {
                            Connection.ChangeTable("ASG_ASIGNATURAS", null);
                            Connection.ChangeSelect("SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK = " + this.course_ID.ToString() + " ORDER BY ASG_CUR_ID_PK ASC, ASG_NOMBRE ASC");
                            if (Connection.Conectar()) Connection.Desconectar();
                            LISTA.Children.Add(new Subject(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                        }
                    else LBL_NOTFOUND.Visibility = Visibility.Visible;
                }
                LISTA.Children.Add(new TaskBlankSpace());
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_ADDNEWSUBJECT_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerNewSubjects(Padre, Connection, -1);
            }
            private void FILTRO_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!first_time)
                {
                    Connection.ChangeTable("CUR_CURSOS", null);
                    Connection.ChangeSelect("SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" + FILTRO.Items[FILTRO.SelectedIndex].ToString() + "' AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString());
                    if (Connection.Conectar()) Connection.Desconectar();
                    Padre.elementHost.Child = new OrganizerSubjects(Padre, Connection, int.Parse(Connection.getData(0, 0)));
                    System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
                }
                else
                {
                    FILTRO.SelectedIndex = -1;
                    first_time = false;
                }
            }
        #endregion
    }
}
