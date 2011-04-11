using System;
using System.Windows.Forms;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class OrganizerNewSchedule : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int schedule_ID = -1;

        #region Constructores
            public OrganizerNewSchedule()
            {
                InitializeComponent();
            }
            public OrganizerNewSchedule(Formulario Padre, MySQL_DB Connection, int ID)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.schedule_ID = ID;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                Connection.ChangeTable("CUR_CURSOS", null);
                Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString());
                if (Connection.Conectar()) Connection.Desconectar();
                for (int x = 0; x < Connection.countRows(); x++) TXT_COMBO_CURSO.Items.Add(Connection.getData(x, 0));

                if (schedule_ID != -1)
                {
                    this.BTN_CREARHORARIO.Content = "Guardar los cambios";
                    this.BTN_BORRAR.Visibility = System.Windows.Visibility.Visible;

                    Connection.ChangeTable("HOR_HORARIOS", null);
                    Connection.ChangeSelect("SELECT * FROM HOR_HORARIOS WHERE HOR_ID_PK = " + this.schedule_ID.ToString());
                    if (Connection.Conectar()) Connection.Desconectar();
                    TXT_H_COMIENZO.Text = Connection.getData(0, 2);
                    TXT_H_FINALIZACION.Text = Connection.getData(0, 3);
                    set_Days(Connection.getData(0, 4));

                    string asg_ID = Connection.getData(0, 1);
                    Connection.ChangeTable("CUR_CURSOS", null);
                    Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_ID_PK = (SELECT ASG_CUR_ID_PK FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + asg_ID + ")");
                    if (Connection.Conectar()) Connection.Desconectar();
                    TXT_COMBO_CURSO.Text = Connection.getData(0, 0);
                    TXT_COMBO_ASIGNATURA.Items.Clear();
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT ASG_NOMBRE FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK = (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" + TXT_COMBO_CURSO.Items[TXT_COMBO_CURSO.SelectedIndex].ToString() + "')");
                    if (Connection.Conectar()) Connection.Desconectar();
                    for (int x = 0; x < Connection.countRows(); x++) TXT_COMBO_ASIGNATURA.Items.Add(Connection.getData(x, 0));
                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT ASG_NOMBRE FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = (SELECT HOR_ASG_ID_PK FROM HOR_HORARIOS WHERE HOR_ID_PK = " + this.schedule_ID.ToString() + ")");
                    if (Connection.Conectar()) Connection.Desconectar();
                    TXT_COMBO_ASIGNATURA.Text = Connection.getData(0, 0);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_CREARHORARIO_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!(TXT_COMBO_ASIGNATURA.SelectedIndex == -1) && !(string.IsNullOrEmpty(TXT_H_COMIENZO.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_H_FINALIZACION.Text.Trim().Replace('\'', ' '))) && check_Days())
                {
                    if (schedule_ID != -1)
                    {
                        Connection.ChangeTable("HOR_HORARIOS", null);
                        Connection.ChangeSelect("SELECT * FROM HOR_HORARIOS WHERE HOR_ID_PK = " + this.schedule_ID.ToString());
                        if (Connection.Conectar())
                        {
                            Connection.Update_String("UPDATE HOR_HORARIOS SET HOR_ASG_ID_PK = (SELECT ASG_ID_PK FROM ASG_ASIGNATURAS,CUR_CURSOS WHERE ASG_NOMBRE LIKE '" +
                                TXT_COMBO_ASIGNATURA.Items[TXT_COMBO_ASIGNATURA.SelectedIndex].ToString() + "' AND ASG_CUR_ID_PK = (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" + TXT_COMBO_CURSO.Items[TXT_COMBO_CURSO.SelectedIndex].ToString() + "' AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ") AND ASG_CUR_ID_PK = CUR_ID_PK), HOR_F_COMIENZO = '" + TXT_H_COMIENZO.Text.Trim().Replace('\'', ' ') + "', HOR_F_FINAL = '" + TXT_H_FINALIZACION.Text.Trim().Replace('\'', ' ') + "', HOR_D_SEMANA= '" + get_Days() + "' WHERE HOR_ID_PK = " + this.schedule_ID.ToString());
                            Connection.Update_Execute();
                            Connection.Desconectar();
                        }
                    }
                    else
                    {
                        Connection.ChangeTable("USR_USUARIOS", null);
                        Connection.ChangeSelect("SELECT * FROM USR_USUARIOS WHERE BINARY USR_NICK = '" + Padre.TXT_USUARIO.Text + "'");
                        if (Connection.Conectar())
                        {
                            Connection.Insert_String("INSERT INTO HOR_HORARIOS (HOR_ASG_ID_PK,HOR_F_COMIENZO,HOR_F_FINAL,HOR_D_SEMANA) VALUES (" +
                                "(SELECT ASG_ID_PK FROM ASG_ASIGNATURAS,CUR_CURSOS WHERE ASG_NOMBRE LIKE '" + TXT_COMBO_ASIGNATURA.Items[TXT_COMBO_ASIGNATURA.SelectedIndex].ToString() + "' AND ASG_CUR_ID_PK = (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" + TXT_COMBO_CURSO.Items[TXT_COMBO_CURSO.SelectedIndex].ToString() + "' AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ") AND ASG_CUR_ID_PK = CUR_ID_PK)," +
                                "'" + TXT_H_COMIENZO.Text.Trim().Replace('\'', ' ') + "','" + TXT_H_FINALIZACION.Text.Trim().Replace('\'', ' ') + "','" + get_Days() + "')");
                            Connection.Insert_Execute();
                            Connection.Desconectar();
                        }
                    }
                    Padre.elementHost.Child = new OrganizerSchedules(Padre, Connection, -1);
                }
                else
                {
                    MessageBox.Show("¡Debes rellenar todos los datos!", "Información");
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BACK_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerSchedules(Padre, Connection, -1);
            }
            private void TXT_COMBO_CURSO_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                TXT_COMBO_ASIGNATURA.Items.Clear();
                Connection.ChangeTable("ASG_ASIGNATURAS", null);
                Connection.ChangeSelect("SELECT ASG_NOMBRE FROM ASG_ASIGNATURAS WHERE ASG_CUR_ID_PK = (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" + TXT_COMBO_CURSO.Items[TXT_COMBO_CURSO.SelectedIndex].ToString() + "' AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ")");
                if (Connection.Conectar()) Connection.Desconectar();
                for (int x = 0; x < Connection.countRows(); x++) TXT_COMBO_ASIGNATURA.Items.Add(Connection.getData(x, 0));
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BORRAR_Click(object sender, System.Windows.RoutedEventArgs e)
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
            private bool check_Days()
            {
                if ((CHK_D_1.IsChecked == true) || (CHK_D_2.IsChecked == true) || (CHK_D_3.IsChecked == true) || (CHK_D_4.IsChecked == true) || (CHK_D_5.IsChecked == true) || (CHK_D_6.IsChecked == true) || (CHK_D_7.IsChecked == true))
                    return true;
                else
                    return false;
            }
            private string get_Days()
            {
                System.Text.StringBuilder temp = new System.Text.StringBuilder("0000000");
                if (CHK_D_1.IsChecked == true) temp[0] = '1';
                if (CHK_D_2.IsChecked == true) temp[1] = '1';
                if (CHK_D_3.IsChecked == true) temp[2] = '1';
                if (CHK_D_4.IsChecked == true) temp[3] = '1';
                if (CHK_D_5.IsChecked == true) temp[4] = '1';
                if (CHK_D_6.IsChecked == true) temp[5] = '1';
                if (CHK_D_7.IsChecked == true) temp[6] = '1';
                return temp.ToString();
            }
            private void set_Days(string cadena)
            {
                if (cadena[0] == '1') CHK_D_1.IsChecked = true;
                if (cadena[1] == '1') CHK_D_2.IsChecked = true;
                if (cadena[2] == '1') CHK_D_3.IsChecked = true;
                if (cadena[3] == '1') CHK_D_4.IsChecked = true;
                if (cadena[4] == '1') CHK_D_5.IsChecked = true;
                if (cadena[5] == '1') CHK_D_6.IsChecked = true;
                if (cadena[6] == '1') CHK_D_7.IsChecked = true;
            }
        #endregion
    }
}
