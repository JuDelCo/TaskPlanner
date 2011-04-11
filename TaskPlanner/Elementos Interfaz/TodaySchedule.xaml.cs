using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class TodaySchedule : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int schedule_ID;

        #region Constructores
            public TodaySchedule()
            {
                InitializeComponent();
            }
            public TodaySchedule(Formulario Padre, MySQL_DB Connection, int ID)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.schedule_ID = ID;

                Connection.ChangeTable("HOR_HORARIOS", null);
                Connection.ChangeSelect("SELECT HOR_F_COMIENZO,HOR_F_FINAL,ASG_NOMBRE,ASG_LUGAR FROM HOR_HORARIOS,ASG_ASIGNATURAS WHERE HOR_ID_PK = " + this.schedule_ID.ToString() + " AND HOR_ASG_ID_PK = ASG_ID_PK");
                if (Connection.Conectar())
                {
                    LBL_HORARIO.Content = Connection.getData(0, 0) + " - " + Connection.getData(0, 1);
                    LBL_ASIGNATURA.Content = Connection.getData(0, 2);
                    LBL_LUGAR.Content = Connection.getData(0, 3);
                    Connection.Desconectar();
                }
            }
        #endregion
    }
}
