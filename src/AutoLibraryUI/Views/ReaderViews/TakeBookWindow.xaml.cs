using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AutoLibraryUI.Views.ReaderViews
{
    /// <summary>
    /// Логика взаимодействия для TakeBookWindow.xaml
    /// </summary>
    public partial class TakeBookWindow : Window
    {
        public event Action<Actions, DateTime, DateTime>? OnAction;

        public TakeBookWindow()
        {
            InitializeComponent();
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if(Dt1.SelectedDate == null || Dt2.SelectedDate == null)
            {
                MessageBox.Show("Даты не выбраны");
            }
            else
            {
                OnAction?.Invoke(Actions.Select, (DateTime)Dt1.SelectedDate!, (DateTime)Dt2.SelectedDate!);
                Close();
            }           
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
