using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class Organizer : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;

        #region Constructores
            public Organizer()
            {
                InitializeComponent();
            }
            public Organizer(Formulario Padre, MySQL_DB Connection)
            {
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;

                this.Padre.Size = new System.Drawing.Size(476, 503);
            }
        #endregion

        #region Métodos
            private void BTN_CURSOS_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerCourses(Padre, Connection, false);
            }
            private void BTN_ASIGNATURAS_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerSubjects(Padre, Connection, -1);
            }
            private void BTN_HORARIOS_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new OrganizerSchedules(Padre, Connection, -1);
            }
        #endregion
    }
}
