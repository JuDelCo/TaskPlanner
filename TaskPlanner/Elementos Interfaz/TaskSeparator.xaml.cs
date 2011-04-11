using System;
using System.Windows;
using System.Windows.Controls;

namespace TaskPlanner
{
    public partial class TaskSeparator : UserControl
    {
        #region Constructores
            public TaskSeparator()
            {
                InitializeComponent();
            }
            public TaskSeparator(string text)
            {
                InitializeComponent();
                this.TXT_SEPARADOR.Content = text;
            }
        #endregion
    }
}
