using System;
using System.Windows.Forms;

namespace TaskPlanner
{
    public partial class Schedule : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int schedule_ID;

        #region Constructores
            public Schedule()
            {
                InitializeComponent();
            }
            public Schedule(Formulario Padre, MySQL_DB Connection, int ID)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.schedule_ID = ID;

                Connection.ChangeTable("HOR_HORARIOS", null);
                Connection.ChangeSelect("SELECT * FROM HOR_HORARIOS WHERE HOR_ID_PK = " + this.schedule_ID.ToString());
                if (Connection.Conectar())
                {
                    LBL_HORARIO.Content = Connection.getData(0, 2) + " - " + Connection.getData(0, 3);
                    LBL_DIAS.Content = set_Days(Connection.getData(0,4));
                    Connection.Desconectar();
                    Connection.ChangeSelect("SELECT ASG_CUR_ID_PK,ASG_NOMBRE FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + Connection.getData(0, 1));
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    if (Connection.Conectar()) Connection.Desconectar();
                    LBL_ASIGNATURA.Content = Connection.getData(0, 1);
                    Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_ID_PK = " + Connection.getData(0, 0));
                    Connection.ChangeTable("CUR_CURSOS", null);
                    if (Connection.Conectar()) Connection.Desconectar();
                    LBL_CURSO.Content = Connection.getData(0, 0);
                }
            }
        #endregion

        #region Métodos
            private void BTN_HORARIO_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerNewSchedule(Padre, Connection, schedule_ID);
            }
            private void BTN_ELIMINAR_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (MessageBox.Show("¿Estás seguro de que quieres borrar el horario?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Connection.ChangeTable("HOR_HORARIOS", null);
                    Connection.ChangeSelect("SELECT * FROM HOR_HORARIOS WHERE HOR_ID_PK = " + this.schedule_ID.ToString());
                    if (Connection.Conectar())
                    {
                        Connection.Delete_String("DELETE FROM HOR_HORARIOS WHERE HOR_ID_PK = " + this.schedule_ID.ToString());
                        Connection.Delete_Execute();
                        Connection.Desconectar();
                    }
                    Padre.elementHost.Child = new OrganizerSchedules(Padre, Connection, -1);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private string set_Days(string cadena)
            {
                string days = "";
                if (cadena == "1111111") days = "Todos los días";
                else
                {
                    if (cadena[0] == '1') days += "Lunes ";
                    if (cadena[1] == '1') days += "Martes ";
                    if (cadena[2] == '1') days += "Miércoles ";
                    if (cadena[3] == '1') days += "Jueves ";
                    if (cadena[4] == '1') days += "Viernes ";
                    if (cadena[5] == '1') days += "Sábado ";
                    if (cadena[6] == '1') days += "Domingo";
                }
                return days;
            }
        #endregion
    }
}
