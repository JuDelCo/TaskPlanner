using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class Login : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;

        #region Constructores
            public Login()
            {
                InitializeComponent();
            }
            public Login(Formulario Padre, MySQL_DB Connection)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;

                this.Padre.Size = new System.Drawing.Size(476, 257);
                this.Padre.BTN_Exit.Visible = false;
            }
        #endregion

        #region Métodos
            private void TXT_PASSWORD_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
            {
                if (e.Key == System.Windows.Input.Key.Enter)
                {
                    BTN_Identificarse_Click(sender, e);
                }
            }
            private void BTN_Identificarse_Click(object sender, RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (Connection.testConnection())
                {
                    if (!(string.IsNullOrEmpty(TXT_USUARIO.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_PASSWORD.Password.Trim().Replace('\'', ' '))))
                    {
                        Connection.ChangeTable("USR_USUARIOS", null);
                        Connection.ChangeSelect("SELECT * FROM USR_USUARIOS WHERE BINARY USR_NICK = '" + TXT_USUARIO.Text.Trim().Replace('\'', ' ') + "' AND BINARY USR_PASSWORD  = '" + EncriptarMD5.Encript(TXT_PASSWORD.Password.Replace('\'', ' ')) + "'");
                        if (Connection.Conectar())
                        {
                            if (Connection.countRows() == 1)
                            {
                                if (Connection.getData(0, 9) == "Activo")
                                {
                                    Padre.menuHoy.Visible = true;
                                    Padre.menuTareas.Visible = true;
                                    Padre.menuPlanificador.Visible = true;
                                    Padre.menuPerfil.Visible = true;
                                    Padre.TXT_USUARIO.Text = TXT_USUARIO.Text.Trim().Replace('\'', ' ');
                                    Padre.setUsrID(int.Parse(Connection.getData(0, 0)));

                                    Connection.Update_String("UPDATE USR_USUARIOS SET USR_F_ULTIMA_V = '" + DateTime.Now.ToString("yyyyMMdd") + "', USR_LAST_IP = '" + ObtenerIP.ExternalIPAddress.ToString() + "' WHERE USR_ID_PK = " + Padre.getUsrID());
                                    Connection.Update_Execute();

                                    Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                                        "'Login','USR: " + Padre.TXT_USUARIO.Text + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "'," + Padre.getUsrID() + ")");
                                    Connection.Insert_Execute();

                                    Padre.elementHost.Child = new Today(Padre, Connection);
                                }
                                else
                                {
                                    Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                                        "'Inactive Login','USR: " + TXT_USUARIO.Text.Trim().Replace('\'', ' ') + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "'," + Connection.getData(0, 0) + ")");
                                    Connection.Insert_Execute();
                                    MessageBox.Show("Error, tu cuenta de usuario no está activada.\nSolicita a un administrador su activación", "Información");
                                }
                            }
                            else
                            {
                                Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                                    "'Bad Login','USR: " + TXT_USUARIO.Text.Trim().Replace('\'', ' ') + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "', 0)");
                                Connection.Insert_Execute();
                                MessageBox.Show("Error, usuario y/o contraseña incorrectos.\nRecuerda que ambos campos son sensibles a mayúsculas y minúsculas.", "Información");
                                TXT_PASSWORD.Password = "";
                            }
                            Connection.Desconectar();
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡Debes introducir el usuario y la contraseña!", "Información");
                    }
                }
                else
                {
                    MessageBox.Show("Error, no se ha podido establecer conexión con el servidor", "Información");
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_CREAR_CUENTA_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new LoginCreate(Padre, Connection);
            }
            private void BTN_CONFIGURACION_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new LoginSettings(Padre, Connection);
            }
        #endregion
    }
}
