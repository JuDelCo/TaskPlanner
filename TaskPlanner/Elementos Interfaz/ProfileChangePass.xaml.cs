using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class ProfileChangePass : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;

        #region Constructores
            public ProfileChangePass()
            {
                InitializeComponent();
            }
            public ProfileChangePass(Formulario Padre, MySQL_DB Connection)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;

                this.Padre.Size = new System.Drawing.Size(476, 503);
            }
        #endregion

        #region Métodos
            private void BTN_CHANGEPASS_Click(object sender, RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!(string.IsNullOrEmpty(TXT_OLDPASS.Password.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_NEWPASS.Password.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_RE_NEWPASS.Password.Trim().Replace('\'', ' '))))
                {
                    Connection.ChangeTable("USR_USUARIOS", "USR_ID_PK");
                    Connection.ChangeSelect("SELECT * FROM USR_USUARIOS WHERE BINARY USR_NICK = '" + Padre.TXT_USUARIO.Text + "'");
                    Connection.Update_String("UPDATE USR_USUARIOS SET USR_PASSWORD = '" + EncriptarMD5.Encript(TXT_NEWPASS.Password.Trim().Replace('\'', ' ')) + "' WHERE BINARY USR_NICK = '" + Padre.TXT_USUARIO.Text + "'");
                    Connection.Conectar();
                    if ((Connection.getData(0, 2) == EncriptarMD5.Encript(TXT_OLDPASS.Password.Trim().Replace('\'', ' '))) && (TXT_NEWPASS.Password.Trim().Replace('\'', ' ') == TXT_RE_NEWPASS.Password.Trim().Replace('\'', ' ')))
                    {
                        Connection.Update_Execute();
                        Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                            "'ChangePass','USR: " + Padre.TXT_USUARIO.Text + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "'," + Padre.getUsrID() + ")");
                        Connection.Insert_Execute();
                        Connection.Desconectar();
                        MessageBox.Show("Contraseña cambiada correctamente !", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                        Padre.elementHost.Child = new Profile(Padre, Connection);
                    }
                    else
                    {
                        Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                            "'Bad ChangePass','USR: " + Padre.TXT_USUARIO.Text + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "'," + Padre.getUsrID() + ")");
                        Connection.Insert_Execute();
                        Connection.Desconectar();
                        MessageBox.Show("Error, comprueba si has escrito bien las contraseñas !", "Información", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Error, debes rellenar todos los campos !", "Información", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_BACK_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new Profile(Padre, Connection);
            }
        #endregion
    }
}
