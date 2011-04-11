using System;
using System.Windows.Forms;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class OrganizerNewSubjects : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int subject_ID = -1;

        #region Constructores
            public OrganizerNewSubjects()
            {
                InitializeComponent();
            }
            public OrganizerNewSubjects(Formulario Padre, MySQL_DB Connection, int ID)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.subject_ID = ID;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                Connection.ChangeTable("CUR_CURSOS", null);
                Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString());
                if (Connection.Conectar()) Connection.Desconectar();
                for (int x = 0; x < Connection.countRows(); x++) TXT_COMBO_CURSO.Items.Add(Connection.getData(x,0));

                if (subject_ID != -1)
                {
                    this.BTN_CREARASIGNATURA.Content = "Guardar los cambios";
                    this.BTN_BORRAR.Visibility = System.Windows.Visibility.Visible;

                    Connection.ChangeTable("ASG_ASIGNATURAS", null);
                    Connection.ChangeSelect("SELECT * FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + this.subject_ID.ToString());
                    if (Connection.Conectar()) Connection.Desconectar();
                    TXT_NOMBRE.Text = Connection.getData(0, 2);
                    TXT_LUGAR.Text = Connection.getData(0, 3);
                    Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_ID_PK = " + Connection.getData(0, 1));
                    Connection.ChangeTable("CUR_CURSOS", null);
                    if (Connection.Conectar()) Connection.Desconectar();
                    TXT_COMBO_CURSO.Text = Connection.getData(0, 0);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_CREARASIGNATURA_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!(TXT_COMBO_CURSO.SelectedIndex == -1) && !(string.IsNullOrEmpty(TXT_NOMBRE.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_LUGAR.Text.Trim().Replace('\'', ' '))))
                {
                    if (subject_ID != -1)
                    {
                        Connection.ChangeTable("ASG_ASIGNATURAS", null);
                        Connection.ChangeSelect("SELECT * FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = " + this.subject_ID.ToString());
                        if (Connection.Conectar())
                        {
                            Connection.Update_String("UPDATE ASG_ASIGNATURAS SET ASG_CUR_ID_PK = (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" +
                                TXT_COMBO_CURSO.Items[TXT_COMBO_CURSO.SelectedIndex].ToString() + "' AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + "), ASG_NOMBRE = '" + TXT_NOMBRE.Text.Trim().Replace('\'', ' ') + "', ASG_LUGAR = '" + TXT_LUGAR.Text.Trim().Replace('\'', ' ') + "' WHERE ASG_ID_PK = " + this.subject_ID.ToString());
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
                            Connection.Insert_String("INSERT INTO ASG_ASIGNATURAS (ASG_CUR_ID_PK,ASG_NOMBRE,ASG_LUGAR) VALUES (" +
                                "(SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" + TXT_COMBO_CURSO.Items[TXT_COMBO_CURSO.SelectedIndex].ToString() + "' AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ")," +
                                "'" + TXT_NOMBRE.Text.Trim().Replace('\'', ' ') + "','" + TXT_LUGAR.Text.Trim().Replace('\'', ' ') + "')");
                            Connection.Insert_Execute();
                            Connection.Desconectar();
                        }
                    }
                    Padre.elementHost.Child = new OrganizerSubjects(Padre, Connection, -1);
                }
                else
                {
                    MessageBox.Show("¡Debes rellenar todos los datos!", "Información");
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BORRAR_Click(object sender, System.Windows.RoutedEventArgs e)
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
            private void BTN_BACK_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerSubjects(Padre, Connection, -1);
            }
        #endregion
    }
}
