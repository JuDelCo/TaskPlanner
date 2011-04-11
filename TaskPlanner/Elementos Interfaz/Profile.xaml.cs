using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class Profile : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;

        #region Constructores
            public Profile()
            {
                InitializeComponent();
            }
            public Profile(Formulario Padre, MySQL_DB Connection)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                Connection.ChangeTable("USR_USUARIOS", "USR_ID_PK");
                Connection.ChangeSelect("SELECT * FROM USR_USUARIOS WHERE BINARY USR_NICK = '" + Padre.TXT_USUARIO.Text + "'");
                if (Connection.Conectar())
                {
                    TXT_EMAIL.Text = Connection.getData(0, 5);
                    TXT_ESTADO.Text = Connection.getData(0, 9);
                    TXT_F_REGISTRO.Text = DateTime.Parse(Connection.getData(0, 6)).ToShortDateString();
                    TXT_ID.Text = Connection.getData(0, 0);
                    TXT_NICK.Text = Connection.getData(0, 1);
                    TXT_NOMBRE.Text = Connection.getData(0, 4);
                    if (int.Parse(Connection.getData(0, 3)) == 1) TXT_TIPO.Text = "Usuario registrado";
                    else if (int.Parse(Connection.getData(0, 3)) == 2) TXT_TIPO.Text = "Administrador";

                    Connection.Desconectar();
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void BTN_ACTUALIZAR_Click(object sender, RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                if (!(string.IsNullOrEmpty(TXT_NOMBRE.Text.Trim().Replace('\'', ' '))) && !(string.IsNullOrEmpty(TXT_EMAIL.Text.Trim().Replace('\'', ' '))))
                {
                    Connection.ChangeTable("USR_USUARIOS", "USR_ID_PK");
                    Connection.ChangeSelect("SELECT * FROM USR_USUARIOS WHERE BINARY USR_NICK = '" + Padre.TXT_USUARIO.Text + "'");
                    Connection.Update_String("UPDATE USR_USUARIOS SET USR_NOMBRE = '" + TXT_NOMBRE.Text.Trim().Replace('\'', ' ') + "', USR_EMAIL = '" + TXT_EMAIL.Text.Trim().Replace('\'', ' ') + "' WHERE BINARY USR_NICK = '" + Padre.TXT_USUARIO.Text + "'");
                    Connection.Conectar();
                    Connection.Update_Execute();
                    Connection.Update_Execute();
                    Connection.Insert_String("INSERT INTO LOG_REGISTROS (LOG_TIPO,LOG_DESCRIPCION,LOG_IP,LOG_FECHA,LOG_USR_ID_PK) VALUES (" +
                        "'UpdateProfile','USR: " + Padre.TXT_USUARIO.Text + "','" + ObtenerIP.ExternalIPAddress.ToString() + "','" + DateTime.Now.ToString("yyyyMMdd") + "'," + Padre.getUsrID() + ")");
                    Connection.Insert_Execute();
                    Connection.Desconectar();
                    MessageBox.Show("Datos modificados correctamente !", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Debes introducir tu nombre y tu email !", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void BTN_CHANGEPASS_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new ProfileChangePass(Padre, Connection);
            }
        #endregion
    }
}
