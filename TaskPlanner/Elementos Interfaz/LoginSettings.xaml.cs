using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class LoginSettings : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;

        #region Constructores
            public LoginSettings()
            {
                InitializeComponent();
            }
            public LoginSettings(Formulario Padre, MySQL_DB Connection)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;

                this.Padre.Size = new System.Drawing.Size(476, 282);
            }
        #endregion

        #region Métodos
            private void BTN_SAVE_Click(object sender, RoutedEventArgs e)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                Connection.SetConnection(TXT_SERVIDOR.Text.Trim(), TXT_PUERTO.Text.Trim(), TXT_USUARIO.Text.Trim(), TXT_PASSWORD.Password, TXT_DATABASE.Text.Trim());

                if (Connection.testConnection()) MessageBox.Show("Se ha podido establecer conexión con el servidor satisfactoriamente", "Información", MessageBoxButton.OK, MessageBoxImage.Information);
                else MessageBox.Show("Error, no se ha podido establecer conexión con el servidor. \nComprueba tu configuración e intentalo de nuevo.", "Información", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
            private void TXT_DATABASE_GotFocus(object sender, RoutedEventArgs e)
            {
                this.TXT_DATABASE.SelectAll();
            }
            private void TXT_PASSWORD_GotFocus(object sender, RoutedEventArgs e)
            {
                this.TXT_PASSWORD.SelectAll();
            }
            private void TXT_USUARIO_GotFocus(object sender, RoutedEventArgs e)
            {
                this.TXT_USUARIO.SelectAll();
            }
            private void TXT_PUERTO_GotFocus(object sender, RoutedEventArgs e)
            {
                this.TXT_PUERTO.SelectAll();
            }
            private void TXT_SERVIDOR_GotFocus(object sender, RoutedEventArgs e)
            {
                TXT_SERVIDOR.SelectAll();
            }
            private void BTN_BACK_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new Login(Padre, Connection);
            }
        #endregion
    }
}
