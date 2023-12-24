using AutoLibraryUI.Services;
using AutoLibraryUI.Views.SystemViews;
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

namespace AutoLibraryUI.Views
{
    /// <summary>
    /// Логика взаимодействия для NavigationWindow.xaml
    /// </summary>
    public partial class NavigationWindow : Window
    {
        private readonly RegistationList _accountsService;
        private readonly CatalogService _catalogService;

        public NavigationWindow(RegistationList accountsService, CatalogService catalogService)
        {
            _accountsService = accountsService;
            _catalogService = catalogService;

            InitializeComponent();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var identity = System.Threading.Thread.CurrentPrincipal!.Identity;

            LoginInfo.Content = $"{identity!.Name}";

            AdminBtn.IsEnabled = Roles.IsAdministrator();
            ReadersBtn.IsEnabled = Roles.IsLibrarian();
        }

        private void CatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            CatalogWindow window = new CatalogWindow(_catalogService);

            window.ShowDialog();
        }

        private void ReadersBtn_Click(object sender, RoutedEventArgs e)
        {
            ReadersWindow window = new ReadersWindow(_accountsService, _catalogService);

            window.ShowDialog();
        }

        private void AdminBtn_Click(object sender, RoutedEventArgs e)
        {
            RegisterWindow window = new RegisterWindow(_accountsService);
            window.ShowDialog();
        }
    }
}
