using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class TaskList : UserControl
    {
        private Formulario Padre;
        private MySQL_DB Connection;
        private int ID_Filtro;

        #region Constructores
            public TaskList()
            {
                InitializeComponent();
            }
            public TaskList(Formulario Padre, MySQL_DB Connection, int ID_Filtro)
            {
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
                InitializeComponent();
                this.Padre = Padre;
                this.Connection = Connection;
                this.ID_Filtro = ID_Filtro;

                this.Padre.Size = new System.Drawing.Size(476, 503);

                LISTA.Children.Add(new TaskBlankSpace());
                switch (ID_Filtro)
                {
                    case 0: //Fecha-Limite
                        Connection.ChangeTable("TAR_TAREAS", null);
                        Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_F_OBJETIVO FROM TAR_TAREAS WHERE TAR_USR_ID_PK = " + Padre.getUsrID().ToString() + " AND ISNULL(TAR_F_COMPLETADA) ORDER BY TAR_F_OBJETIVO ASC");
                        if (Connection.Conectar()) Connection.Desconectar();
                        int num_task = Connection.countRows();
                        if (num_task > 0)
                        {
                            string separatordate = "Sin fecha";
                            if (!(string.IsNullOrEmpty(Connection.getData(0, 1))))
                            {
                                separatordate = DateTime.Parse(Connection.getData(0, 1)).ToLongDateString();
                                if (DateTime.Parse(separatordate).CompareTo(DateTime.Now.Date) == 0) LISTA.Children.Add(new TaskSeparator("Hoy, " + separatordate));
                                else LISTA.Children.Add(new TaskSeparator(separatordate));
                            }
                            else LISTA.Children.Add(new TaskSeparator(separatordate));
                            for (int x = 0; x < num_task; x++)
                            {
                                Connection.ChangeTable("TAR_TAREAS", null);
                                Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_F_OBJETIVO FROM TAR_TAREAS WHERE TAR_USR_ID_PK = " + Padre.getUsrID().ToString() + " AND ISNULL(TAR_F_COMPLETADA) ORDER BY TAR_F_OBJETIVO ASC");
                                if (Connection.Conectar()) Connection.Desconectar();
                                if (!(string.IsNullOrEmpty(Connection.getData(x, 1))))
                                {
                                    if (separatordate != DateTime.Parse(Connection.getData(x, 1)).ToLongDateString())
                                    {
                                        separatordate = DateTime.Parse(Connection.getData(x, 1)).ToLongDateString();
                                        LISTA.Children.Add(new TaskBlankSpace());
                                        if (DateTime.Parse(separatordate).CompareTo(DateTime.Now.Date) == 0) LISTA.Children.Add(new TaskSeparator("Hoy, " + separatordate));
                                        else LISTA.Children.Add(new TaskSeparator(separatordate));
                                    }
                                }
                                LISTA.Children.Add(new Task(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                            }
                        }
                        else LBL_NOTFOUND.Visibility = Visibility.Visible;
                        break;
                    case 1: //Asignatura
                        Connection.ChangeTable("TAR_TAREAS", null);
                        Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_F_OBJETIVO,ASG_NOMBRE FROM TAR_TAREAS LEFT OUTER JOIN ASG_ASIGNATURAS ON ASG_ID_PK = TAR_ASG_ID_PK WHERE TAR_USR_ID_PK =  " + Padre.getUsrID().ToString() + " AND ISNULL(TAR_F_COMPLETADA) ORDER BY ASG_NOMBRE ASC, TAR_F_OBJETIVO ASC");
                        if (Connection.Conectar()) Connection.Desconectar();
                        num_task = Connection.countRows();
                        if (num_task > 0)
                        {
                            string separatorcourse = Connection.getData(0, 2);
                            if (string.IsNullOrEmpty(separatorcourse)) LISTA.Children.Add(new TaskSeparator("Otras tareas"));
                            else LISTA.Children.Add(new TaskSeparator(Connection.getData(0, 2)));
                            for (int x = 0; x < num_task; x++)
                            {
                                Connection.ChangeTable("TAR_TAREAS", null);
                                Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_F_OBJETIVO,ASG_NOMBRE FROM TAR_TAREAS LEFT OUTER JOIN ASG_ASIGNATURAS ON ASG_ID_PK = TAR_ASG_ID_PK WHERE TAR_USR_ID_PK =  " + Padre.getUsrID().ToString() + " AND ISNULL(TAR_F_COMPLETADA) ORDER BY ASG_NOMBRE ASC, TAR_F_OBJETIVO ASC");
                                if (Connection.Conectar()) Connection.Desconectar();
                                if (separatorcourse != Connection.getData(x, 2))
                                {
                                    separatorcourse = Connection.getData(x, 2);
                                    LISTA.Children.Add(new TaskBlankSpace());
                                    if (string.IsNullOrEmpty(separatorcourse)) LISTA.Children.Add(new TaskSeparator("Otras tareas"));
                                    else LISTA.Children.Add(new TaskSeparator(separatorcourse));
                                }
                                LISTA.Children.Add(new Task(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                            }
                        }
                        else LBL_NOTFOUND.Visibility = Visibility.Visible;
                        break;
                    case 2: //Prioridad
                        Connection.ChangeTable("TAR_TAREAS", null);
                        Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_PRIORIDAD FROM TAR_TAREAS WHERE TAR_USR_ID_PK = " + Padre.getUsrID().ToString() + " AND ISNULL(TAR_F_COMPLETADA) ORDER BY TAR_PRIORIDAD DESC, TAR_F_OBJETIVO ASC");
                        if (Connection.Conectar()) Connection.Desconectar();
                        num_task = Connection.countRows();
                        if (num_task > 0)
                        {
                            string separatorprioridad = Connection.getData(0, 1);
                            switch (int.Parse(separatorprioridad))
                            {
                                case 2:
                                    LISTA.Children.Add(new TaskSeparator("Prioridad Alta"));
                                    break;
                                case 1:
                                    LISTA.Children.Add(new TaskSeparator("Prioridad Media"));
                                    break;
                                case 0:
                                    LISTA.Children.Add(new TaskSeparator("Prioridad Baja"));
                                    break;
                            }
                            for (int x = 0; x < num_task; x++)
                            {
                                Connection.ChangeTable("TAR_TAREAS", null);
                                Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_PRIORIDAD FROM TAR_TAREAS WHERE TAR_USR_ID_PK = " + Padre.getUsrID().ToString() + " AND ISNULL(TAR_F_COMPLETADA) ORDER BY TAR_PRIORIDAD DESC, TAR_F_OBJETIVO ASC");
                                if (Connection.Conectar()) Connection.Desconectar();
                                if (separatorprioridad != Connection.getData(x, 1))
                                {
                                    separatorprioridad = Connection.getData(x, 1);
                                    LISTA.Children.Add(new TaskBlankSpace());
                                    switch (int.Parse(separatorprioridad))
                                    {
                                        case 2:
                                            LISTA.Children.Add(new TaskSeparator("Prioridad Alta"));
                                            break;
                                        case 1:
                                            LISTA.Children.Add(new TaskSeparator("Prioridad Media"));
                                            break;
                                        case 0:
                                            LISTA.Children.Add(new TaskSeparator("Prioridad Baja"));
                                            break;
                                    }
                                }
                                LISTA.Children.Add(new Task(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                            }
                        }
                        else LBL_NOTFOUND.Visibility = Visibility.Visible;
                        break;
                    case 3: //Completadas
                        BTN_COMPLETADAS.Content = "Ver Pendientes";
                        Connection.ChangeTable("TAR_TAREAS", null);
                        Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_F_OBJETIVO FROM TAR_TAREAS WHERE TAR_USR_ID_PK = " + Padre.getUsrID().ToString() + " AND NOT ISNULL(TAR_F_COMPLETADA) ORDER BY TAR_F_OBJETIVO ASC");
                        if (Connection.Conectar()) Connection.Desconectar();
                        int num_task_completed = Connection.countRows();
                        if (num_task_completed > 0)
                        {
                            string separatordate = "Sin fecha";
                            if (!(string.IsNullOrEmpty(Connection.getData(0, 1))))
                            {
                                separatordate = DateTime.Parse(Connection.getData(0, 1)).ToLongDateString();
                                if (DateTime.Parse(separatordate).CompareTo(DateTime.Now.Date) == 0) LISTA.Children.Add(new TaskSeparator("Hoy, " + separatordate));
                                else LISTA.Children.Add(new TaskSeparator(separatordate));
                            }
                            else LISTA.Children.Add(new TaskSeparator(separatordate));
                            for (int x = 0; x < num_task_completed; x++)
                            {
                                Connection.ChangeTable("TAR_TAREAS", null);
                                Connection.ChangeSelect("SELECT TAR_ID_PK,TAR_F_OBJETIVO FROM TAR_TAREAS WHERE TAR_USR_ID_PK = " + Padre.getUsrID().ToString() + " AND NOT ISNULL(TAR_F_COMPLETADA) ORDER BY TAR_F_OBJETIVO ASC");
                                if (Connection.Conectar()) Connection.Desconectar();
                                if (!(string.IsNullOrEmpty(Connection.getData(x, 1))))
                                {
                                    if (separatordate != DateTime.Parse(Connection.getData(x, 1)).ToLongDateString())
                                    {
                                        separatordate = DateTime.Parse(Connection.getData(x, 1)).ToLongDateString();
                                        LISTA.Children.Add(new TaskBlankSpace());
                                        if (DateTime.Parse(separatordate).CompareTo(DateTime.Now.Date) == 0) LISTA.Children.Add(new TaskSeparator("Hoy, " + separatordate));
                                        else LISTA.Children.Add(new TaskSeparator(separatordate));
                                    }
                                }
                                LISTA.Children.Add(new Task(Padre, Connection, int.Parse(Connection.getData(x, 0))));
                            }
                        }
                        else
                        {
                            LBL_NOTFOUND.Content = "No tienes tareas completadas";
                            LBL_NOTFOUND.Visibility = Visibility.Visible;
                        }
                        break;
                }
                LISTA.Children.Add(new TaskBlankSpace());
                System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
            }
        #endregion

        #region Métodos
            private void FILTRO_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                Padre.elementHost.Child = new TaskList(Padre, Connection, FILTRO.SelectedIndex);
            }
            private void BTN_ADDNEWTASK_Click(object sender, RoutedEventArgs e)
            {
                Padre.elementHost.Child = new TaskNew(Padre, Connection, -1);
            }
            private void BTN_COMPLETADAS_Click(object sender, RoutedEventArgs e)
            {
                if(ID_Filtro == 3) Padre.elementHost.Child = new TaskList(Padre, Connection, 0);
                else Padre.elementHost.Child = new TaskList(Padre, Connection, 3);
            }
        #endregion
    }
}