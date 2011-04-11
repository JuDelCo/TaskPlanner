using System;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TaskPlanner
{
    public partial class OrganizerNewCourse : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int course_ID = -1;

        #region Constructores
            public OrganizerNewCourse()
            {
                InitializeComponent();
            }
            public OrganizerNewCourse(Formulario Padre, MySQL_DB Connection, int ID)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.course_ID = ID;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                if (course_ID != -1)
                {
                    this.BTN_CREARCURSO.Content = "Guardar los cambios";
                    this.BTN_BORRAR.Visibility = System.Windows.Visibility.Visible;

                    Connection.ChangeTable("CUR_CURSOS", null);
                    Connection.ChangeSelect("SELECT * FROM CUR_CURSOS WHERE CUR_ID_PK = " + this.course_ID.ToString());
                    if (Connection.Conectar()) Connection.Desconectar();
                    TXT_NOMBRE.Text = Connection.getData(0, 2);
                    TXT_DESCRIPCION.Text = Connection.getData(0, 3);
                    TXT_F_COMIENZO.Text = DateTime.Parse(Connection.getData(0, 4)).ToShortDateString();
                    TXT_F_FINALIZACION.Text = DateTime.Parse(Connection.getData(0, 5)).ToShortDateString();
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_CREARCURSO_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!(string.IsNullOrEmpty(TXT_NOMBRE.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_DESCRIPCION.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_F_COMIENZO.Text)) && !(string.IsNullOrEmpty(TXT_F_FINALIZACION.Text)))
                {
                    Regex regex = new Regex(@"^(((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((20[0-9][0-9]))|((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((19[0-9][0-9]))|(29/02/20(([02468][048])|([13579][26])))|(29/02/19(([02468][048])|([13579][26]))))$");
                    Match result_match_f_inicio = regex.Match(TXT_F_COMIENZO.Text);
                    Match result_match_f_fin = regex.Match(TXT_F_FINALIZACION.Text);
                    if ( result_match_f_inicio.Success && result_match_f_fin.Success )
                    {
                        string fecha_inicio = DateTime.Parse(TXT_F_COMIENZO.Text).ToString("yyyyMMdd");
                        string fecha_fin = DateTime.Parse(TXT_F_FINALIZACION.Text).ToString("yyyyMMdd");
                        if (course_ID != -1)
                        {
                            Connection.ChangeTable("CUR_CURSOS", null);
                            Connection.ChangeSelect("SELECT * FROM CUR_CURSOS WHERE CUR_ID_PK = " + this.course_ID.ToString());
                            if (Connection.Conectar())
                            {
                                Connection.Update_String("UPDATE CUR_CURSOS SET CUR_NOMBRE='" + TXT_NOMBRE.Text.Trim().Replace('\'', ' ') + "',CUR_DESCRIPCION='" + TXT_DESCRIPCION.Text.Trim().Replace('\'', ' ') +
                                    "',CUR_F_COMIENZO='" + fecha_inicio + "',CUR_F_FINAL='" + fecha_fin + "' WHERE CUR_ID_PK = " + this.course_ID.ToString());
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
                                Connection.Insert_String("INSERT INTO CUR_CURSOS (CUR_USR_ID_PK,CUR_NOMBRE,CUR_DESCRIPCION,CUR_F_COMIENZO,CUR_F_FINAL) VALUES (" +
                                    Connection.getData(0, 0) + "," + "'" + TXT_NOMBRE.Text.Trim().Replace('\'', ' ') + "'," + "'" + TXT_DESCRIPCION.Text.Trim().Replace('\'', ' ') + "'," + "'" + fecha_inicio + "'," + "'" + fecha_fin + "')");
                                Connection.Insert_Execute();
                                Connection.Desconectar();
                            }
                        }
                        Padre.elementHost.Child = new OrganizerCourses(Padre, Connection, false);
                    }
                    else
                    {
                        MessageBox.Show("¡Debes rellenar todos los datos!", "Información");
                    }
                }
                else
                {
                    MessageBox.Show("Por favor, introduce correctamente ambas fechas.\nFormato: (DD/MM/AAAA)", "Información");
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BORRAR_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (MessageBox.Show("¡Atención!\n¿Estás seguro de que quieres borrar el curso?\nTen en cuenta que todas las asignaturas, horarios y tareas vinculadas con el mismo también se borrarán !", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
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
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BACK_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerCourses(Padre, Connection, false);
            }
        #endregion
    }
}