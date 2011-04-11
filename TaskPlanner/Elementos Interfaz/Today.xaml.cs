using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class Today : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;

        #region Constructores
            public Today()
            {
                InitializeComponent();
            }
            public Today(Formulario Padre, MySQL_DB Connection)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;

                LBL_FECHA.Content = "Hoy es " + DateTime.Now.ToLongDateString();
                string fecha_hoy = DateTime.Now.ToString("yyyyMMdd");
                Connection.ChangeTable("TAR_TAREAS", null);
                Connection.ChangeSelect("SELECT COUNT(*) FROM TAR_TAREAS WHERE TAR_F_OBJETIVO = " + fecha_hoy + " AND TAR_USR_ID_PK = " + Padre.getUsrID().ToString());
                if (Connection.Conectar()) Connection.Desconectar();
                LBL_NUM_TAREAS.Content = Connection.getData(0,0);

                Connection.ChangeTable("HOR_HORARIOS", null);
                Connection.ChangeSelect("SELECT HOR_ID_PK, HOR_D_SEMANA FROM HOR_HORARIOS, ASG_ASIGNATURAS, CUR_CURSOS WHERE HOR_ASG_ID_PK = ASG_ID_PK AND ASG_CUR_ID_PK = CUR_ID_PK AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + " ORDER BY HOR_F_COMIENZO ASC");
                if (Connection.Conectar()) Connection.Desconectar();
                int count = 0;
                LISTA.Children.Add(new TaskBlankSpace());
                if (Connection.countRows() > 0)
                {
                    int iterations = Connection.countRows();
                    for (int x = 0; x < iterations; x++)
                    {
                        Connection.ChangeTable("HOR_HORARIOS", null);
                        Connection.ChangeSelect("SELECT HOR_ID_PK, HOR_D_SEMANA FROM HOR_HORARIOS, ASG_ASIGNATURAS, CUR_CURSOS WHERE HOR_ASG_ID_PK = ASG_ID_PK AND ASG_CUR_ID_PK = CUR_ID_PK AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + " ORDER BY HOR_F_COMIENZO ASC");
                        if (Connection.Conectar()) Connection.Desconectar();
                        if (isToday(Connection.getData(x, 1)))
                        {
                            LISTA.Children.Add(new TodaySchedule(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                            count++;
                        }
                    }
                }
                LISTA.Children.Add(new TaskBlankSpace());
                LBL_INFO_CLASES.Content = "Hoy tienes " + count.ToString() + " clases";
                if (count == 0) LBL_NOTFOUND.Visibility = Visibility.Visible;

                this.Padre.Size = new System.Drawing.Size(476, 503);
                this.Padre.BTN_Exit.Visible = true;
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_TAREAS_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new TaskList(Padre, Connection, 0);
            }
            private bool isToday(string days)
            {
                int day = ((int)DateTime.Now.DayOfWeek) - 1;
                if (((int)DateTime.Now.DayOfWeek) == 0) day = 6;
                if (days[day] == '1') return true;
                return false;
            }
        #endregion
    }
}
