using AutoLibraryUI.Entities;
using AutoLibraryUI.Services;
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
    /// Логика взаимодействия для ReadersWindow.xaml
    /// </summary>
    public partial class ReadersWindow : Window
    {
        private readonly RegistationList _accountsService;
        private readonly CatalogService _catalogService;

        public ReadersWindow(RegistationList accountsService, CatalogService catalogService)
        {
            _accountsService = accountsService;
            _catalogService = catalogService;

            InitializeComponent();
        }
        private void Window_Closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void ReadersDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            SetReaders();
        }

        private void AddReaderButton_Click(object sender, RoutedEventArgs e)
        {
            CreateReaderWindow window = new CreateReaderWindow(_accountsService);
            window.OnAction += action =>
            {
                if(action is Actions.Created)
                {
                    SetReaders();
                }
            };

            window.ShowDialog();
        }

        private void EditReaderButton_Click(object sender, RoutedEventArgs e)
        {
            if(ReadersDataGrid.SelectedItem is Reader reader)
            {
                EditReaderWindow window = new EditReaderWindow(_accountsService, _catalogService, reader.Id);
                window.OnAction += action =>
                {
                    if(action is Actions.Edited)
                    {
                        SetReaders();
                    }

                    if(action is Actions.Deleted)
                    {
                        SetReaders();
                    }
                };


                window.ShowDialog();
            }
            else
            {
                MessageBox.Show("Читатель не выбран!", "Сообщение");
            }
        }

        void SetReaders(bool useSearch = false)
        {
            if(useSearch)
            {
                var query = SearchReadersTxt.Text.ToUpper().Trim();

                ReadersDataGrid.ItemsSource = _accountsService.GetReaders()
                    .Where(x => string.IsNullOrEmpty(query.Trim()) ||
                                query.Contains(x.FirstName.ToUpper()) ||
                                query.Contains(x.LastName.ToUpper()) ||
                                query.Contains(x.Patronimic.ToUpper()));
            }
            else
            {
                SearchReadersTxt.Clear();
                ReadersDataGrid.ItemsSource = _accountsService.GetReaders();
            }
        }

        private void SearchReadersBtn_Click(object sender, RoutedEventArgs e)
        {
            SetReaders(useSearch: true);
        }
    }
}
