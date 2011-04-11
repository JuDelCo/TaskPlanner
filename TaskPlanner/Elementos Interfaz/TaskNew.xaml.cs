using System;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Text.RegularExpressions;

namespace TaskPlanner
{
    public partial class TaskNew : System.Windows.Controls.UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int task_ID = -1;

        #region Constructores
            public TaskNew()
            {
                InitializeComponent();
            }
            public TaskNew(Formulario Padre, MySQL_DB Connection, int ID)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.task_ID = ID;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                Connection.ChangeTable("CUR_CURSOS", null);
                Connection.ChangeSelect("SELECT CUR_NOMBRE FROM CUR_CURSOS WHERE CUR_USR_ID_PK = " + Padre.getUsrID().ToString());
                if (Connection.Conectar()) Connection.Desconectar();
                for (int x = 0; x < Connection.countRows(); x++) TXT_COMBO_CURSO.Items.Add(Connection.getData(x, 0));

                if (task_ID != -1)
                {
                    this.BTN_CREARTAREA.Content = "Guardar los cambios";
                    this.BTN_BORRAR.Visibility = System.Windows.Visibility.Visible;

                    Connection.ChangeTable("TAR_TAREAS", null);
                    Connection.ChangeSelect("SELECT * FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                    if (Connection.Conectar()) Connection.Desconectar();

                    TXT_TITULO.Text = Connection.getData(0, 1);
                    TXT_DESCRIPCION.Text = Connection.getData(0, 2);
                    if(!(string.IsNullOrEmpty(Connection.getData(0, 3)))) TXT_F_OBJETIVO.Text = DateTime.Parse(Connection.getData(0, 3)).ToShortDateString();
                    TXT_COMBO_PRIORIDAD.SelectedIndex = int.Parse(Connection.getData(0, 6));
                    TXT_COMBO_TIPO.SelectedIndex = int.Parse(Connection.getData(0, 7));

                    if (int.Parse(Connection.getData(0, 5)) != -1)
                    {
                        string asg_ID = Connection.getData(0, 5);
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
                        Connection.ChangeSelect("SELECT ASG_NOMBRE FROM ASG_ASIGNATURAS WHERE ASG_ID_PK = (SELECT TAR_ASG_ID_PK FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString() + ")");
                        if (Connection.Conectar()) Connection.Desconectar();
                        TXT_COMBO_ASIGNATURA.Text = Connection.getData(0, 0);
                    }
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_CREARTAREA_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!(string.IsNullOrEmpty(TXT_TITULO.Text.Trim().Replace('\'', ' '))))
                {
                    Regex regex = new Regex(@"^(((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((20[0-9][0-9]))|((((0[1-9])|(1\d)|(2[0-8]))/((0[1-9])|(1[0-2])))|((31/((0[13578])|(1[02])))|((29|30)/((0[1,3-9])|(1[0-2])))))/((19[0-9][0-9]))|(29/02/20(([02468][048])|([13579][26])))|(29/02/19(([02468][048])|([13579][26]))))$");
                    Match result_match_f_objetivo = regex.Match(TXT_F_OBJETIVO.Text);
                    string asg_ID = "-1";
                    if (TXT_COMBO_ASIGNATURA.SelectedIndex != -1) asg_ID = "(SELECT ASG_ID_PK FROM ASG_ASIGNATURAS,CUR_CURSOS WHERE ASG_NOMBRE LIKE '" + TXT_COMBO_ASIGNATURA.Items[TXT_COMBO_ASIGNATURA.SelectedIndex].ToString() + "' AND ASG_CUR_ID_PK = (SELECT CUR_ID_PK FROM CUR_CURSOS WHERE CUR_NOMBRE LIKE '" + TXT_COMBO_CURSO.Items[TXT_COMBO_CURSO.SelectedIndex].ToString() + "' AND CUR_USR_ID_PK = " + Padre.getUsrID().ToString() + ") AND ASG_CUR_ID_PK = CUR_ID_PK)";
                    string fecha_objetivo = "NULL";
                    bool continuar = true;
                    if (!(string.IsNullOrEmpty(TXT_F_OBJETIVO.Text)))
                    {
                        if (result_match_f_objetivo.Success)
                            fecha_objetivo = "'" + DateTime.Parse(TXT_F_OBJETIVO.Text).ToString("yyyyMMdd") + "'";
                        else
                        {
                            MessageBox.Show("Por favor, introduce correctamente la fecha.\nFormato: (DD/MM/AAAA)", "Información");
                            continuar = false;
                        }
                    }
                    if(continuar)
                    {
                        if (task_ID != -1)
                        {
                            Connection.ChangeTable("TAR_TAREAS", null);
                            Connection.ChangeSelect("SELECT * FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                            if (Connection.Conectar())
                            {
                                Connection.Update_String("UPDATE TAR_TAREAS SET TAR_TITULO='" + TXT_TITULO.Text.Trim().Replace('\'', ' ') + "',TAR_DESCRIPCION='" + TXT_DESCRIPCION.Text.Replace('\'', ' ') + "',TAR_F_OBJETIVO=" + 
                                    fecha_objetivo + ",TAR_ASG_ID_PK=" + asg_ID + ",TAR_PRIORIDAD=" + TXT_COMBO_PRIORIDAD.SelectedIndex + ",TAR_TIPO=" + TXT_COMBO_TIPO.SelectedIndex  + " WHERE TAR_ID_PK = " + this.task_ID.ToString());
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
                                Connection.Insert_String("INSERT INTO TAR_TAREAS (TAR_TITULO,TAR_DESCRIPCION,TAR_F_OBJETIVO,TAR_ASG_ID_PK,TAR_PRIORIDAD,TAR_TIPO,TAR_USR_ID_PK) VALUES (" +
                                    "'" + TXT_TITULO.Text.Trim().Replace('\'', ' ') + "','" + TXT_DESCRIPCION.Text.Replace('\'', ' ') + "'," + fecha_objetivo + "," + asg_ID + "," + TXT_COMBO_PRIORIDAD.SelectedIndex + "," + TXT_COMBO_TIPO.SelectedIndex + "," + Padre.getUsrID().ToString() + ")");
                                Connection.Insert_Execute();
                                Connection.Desconectar();
                            }
                        }
                        Padre.elementHost.Child = new TaskList(Padre, Connection, 0);
                    }
                }
                else
                {
                    MessageBox.Show("¡Debes introducir un título!", "Información");
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
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
            private void BTN_LIMPIAR_ASG_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                TXT_COMBO_ASIGNATURA.SelectedIndex = -1;
            }
            private void BTN_BORRAR_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (MessageBox.Show("¿Estás seguro de que quieres borrar la tarea?", "Información", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Connection.ChangeTable("TAR_TAREAS", null);
                    Connection.ChangeSelect("SELECT * FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                    if (Connection.Conectar())
                    {
                        Connection.Delete_String("DELETE FROM TAR_TAREAS WHERE TAR_ID_PK = " + this.task_ID.ToString());
                        Connection.Delete_Execute();
                        Connection.Desconectar();
                    }
                    Padre.elementHost.Child = new TaskList(Padre, Connection, 0);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BACK_Click(object sender, System.Windows.RoutedEventArgs e)
            {
                Padre.elementHost.Child = new TaskList(Padre, Connection, 0);
            }
        #endregion
    }
}
