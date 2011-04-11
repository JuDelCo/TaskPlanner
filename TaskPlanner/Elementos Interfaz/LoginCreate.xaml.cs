using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class LoginCreate : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;

        #region Constructores
            public LoginCreate()
            {
                InitializeComponent();
            }
            public LoginCreate(Formulario Padre, MySQL_DB Connection)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;

                this.Padre.Size = new System.Drawing.Size(476, 282);
            }
        #endregion

        #region Métodos
            private void BTN_CREATE_ACCOUNT_Click(object sender, RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (Connection.testConnection())
                {
                    if (!(string.IsNullOrEmpty(TXT_NICK.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_PASSWORD.Password.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_RE_PASSWORD.Password.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_NOMBRE.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_EMAIL.Text.Trim().Replace('\'', ' '))))
                    {
                        if (TXT_PASSWORD.Password.Trim().Replace('\'', ' ') == TXT_RE_PASSWORD.Password.Trim().Replace('\'', ' '))
                        {
                            Connection.ChangeTable("USR_USUARIOS", null);
                            Connection.ChangeSelect("SELECT USR_NICK FROM USR_USUARIOS WHERE BINARY UPPER(USR_NICK) = '" + TXT_NICK.Text.ToUpper().Trim().Replace('\'', ' ') + "'");
                            if (Connection.Conectar()) Connection.Desconectar();
                            if (!(Connection.countRows() > 0))
                            {
                                Connection.Conectar();
                                Connection.Insert_String("INSERT INTO USR_USUARIOS (USR_NICK,USR_PASSWORD,USR_NOMBRE,USR_EMAIL,USR_F_REGISTRO,USR_F_ULTIMA_V,USR_LAST_IP) VALUES (" +
                                    "'" + TXT_NICK.Text.Trim().Replace('\'', ' ') + "','" + EncriptarMD5.Encript(TXT_PASSWORD.Password.Trim().Replace('\'', ' ')) + "','" + TXT_NOMBRE.Text.Trim().Replace('\'', ' ') + "','" + TXT_EMAIL.Text.Trim().Replace('\'', ' ') + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + DateTime.Now.ToString("yyyyMMdd") + "','" + ObtenerIP.ExternalIPAddress.ToString() + "')");
                                Connection.Insert_Execute();
                                Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                                    "'Register User','USR: " + TXT_NICK.Text.Trim().Replace('\'', ' ') + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "', 0)");
                                Connection.Insert_Execute();
                                Connection.Desconectar();
                                MessageBox.Show("¡Cuenta creada!\nRecuerda que es necesario que un administrador la active para poder usarla", "Información");
                                Padre.elementHost.Child = new Login(Padre, Connection);
                            }
                            else
                            {
                                if (Connection.Conectar())
                                {
                                    Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                                        "'Bad Register User','USR: " + TXT_NICK.Text.Trim().Replace('\'', ' ') + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "', 0)");
                                    Connection.Insert_Execute();
                                    Connection.Desconectar();
                                }
                                MessageBox.Show("Lo siento, ese nick ya está siendo utilizado", "Información");
                                TXT_NICK.Text = "";
                                TXT_PASSWORD.Password = "";
                                TXT_RE_PASSWORD.Password = "";
                            }
                        }
                        else
                        {
                            MessageBox.Show("¡No has introducido correctamente ambas contraseñas!", "Información");
                            TXT_PASSWORD.Password = "";
                            TXT_RE_PASSWORD.Password = "";
                        }
                    }
                    else
                    {
                        MessageBox.Show("¡Debes rellenar todos los datos!", "Información");
                        TXT_PASSWORD.Password = "";
                        TXT_RE_PASSWORD.Password = "";
                    }
                }
                else
                {
                    MessageBox.Show("Error, no se ha podido establecer conexión con el servidor", "Información");
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BACK_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new Login(Padre, Connection);
            }
        #endregion
    }
}
