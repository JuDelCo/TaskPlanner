using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class OrganizerSchedules : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int subject_ID = -1;
        private bool first_time = true;

        #region Constructores
            public OrganizerSchedules()
            {
                InitializeComponent();
            }
            public OrganizerSchedules(Formulario Padre, MySQL_DB Connection, int subject_ID)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.subject_ID = subject_ID;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                Connection.ChangeTable("ASG_ASIGNATURAS", null);
                Connection.ChangeSelect("SELECT ASG_NOMBRE FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK IN (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ")");
                if (Connection.Conectar()) Connection.Desconectar();
                for (int x = 0; x < Connection.countRows(); x++) FILTRO.Items.Add(Connection.getData(x, 0));

                LISTA.Children.Add(new TaskBlankSpace());
                if (subject_ID == -1)
                {
                    Connection.ChangeTable("HOR_HORARIOS", null);
                    Connection.ChangeSelect("SELECT HOR_ID_PK FROM HOR_HORARIOS WHERE HOR_ASG_ID_PK IN (SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK IN (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ")) ORDER BY HOR_ASG_ID_PK ASC, HOR_F_COMIENZO ASC");
                    if (Connection.Conectar()) Connection.Desconectar();
                    int num_schedules = Connection.countRows();
                    if (num_schedules > 0)
                        for (int x = 0; x < num_schedules; x++)
                        {
                            Connection.ChangeTable("HOR_HORARIOS", null);
                            Connection.ChangeSelect("SELECT HOR_ID_PK FROM HOR_HORARIOS WHERE HOR_ASG_ID_PK IN (SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK IN (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ")) ORDER BY HOR_ASG_ID_PK ASC, HOR_F_COMIENZO ASC");
                            if (Connection.Conectar()) Connection.Desconectar();
                            LISTA.Children.Add(new Schedule(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                        }
                    else LBL_NOTFOUND.Visibility = Visibility.Visible;
                }
                else
                {
                    Connection.ChangeTable("HOR_HORARIOS", null);
                    Connection.ChangeSelect("SELECT HOR_ID_PK FROM HOR_HORARIOS WHERE HOR_ASG_ID_PK = " + this.subject_ID + " ORDER BY HOR_ASG_ID_PK ASC, HOR_F_COMIENZO ASC");
                    if (Connection.Conectar()) Connection.Desconectar();
                    int num_subjects = Connection.countRows();
                    if (num_subjects > 0)
                        for (int x = 0; x < num_subjects; x++)
                        {
                            Connection.ChangeTable("HOR_HORARIOS", null);
                            Connection.ChangeSelect("SELECT HOR_ID_PK FROM HOR_HORARIOS WHERE HOR_ASG_ID_PK = " + this.subject_ID + " ORDER BY HOR_ASG_ID_PK ASC, HOR_F_COMIENZO ASC");
                            if (Connection.Conectar()) Connection.Desconectar();
                            LISTA.Children.Add(new Schedule(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                        }
                    else LBL_NOTFOUND.Visibility = Visibility.Visible;
                }
                LISTA.Children.Add(new TaskBlankSpace());
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_ADDNEWSCHEDULE_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerNewSchedule(Padre, Connection, -1);
            }
            private void FILTRO_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!first_time)
                {
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT ASG_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_NOMBRE LIKE '" + FILTRO.Items[FILTRO.SelectedIndex].ToString() + "'");
                    if (Connection.Conectar()) Connection.Desconectar();
                    Padre.elementHost.Child = new OrganizerSchedules(Padre, Connection, int.Parse(Connection.getData(0, 0)));
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
