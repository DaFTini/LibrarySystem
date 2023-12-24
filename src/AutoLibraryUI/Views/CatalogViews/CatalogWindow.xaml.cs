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
    /// Логика взаимодействия для CatalogWindow.xaml
    /// </summary>
    public partial class CatalogWindow : Window
    {
        public event Action<Actions, Book?>? OnAction;

        private readonly CatalogService _catalogService;
        private readonly bool _isSelectBook;

        private void SetOptions(SearchOptions options)
        {
            SearchReadersTxt.Text = options.Query;
            MinDataNud.Value = options.MinDate;
            MaxDataNud.Value = options.MaxDate;
            NameTxt.Text = options.Name;
            CodeIdTxt.Text = options.Isbn;
            AuthorsTxt.Text = options.Authors;

            if(_isSelectBook)
            {
                HasAvailableCb.IsChecked = true;
            }
            else
            {
                HasAvailableCb.IsChecked = options.HasAvailable;
            }
            
        }

        private SearchOptions GetOptions()
        {
            return new SearchOptions()
            {
                Query = SearchReadersTxt.Text.Trim(),
                MinDate = (int)MinDataNud.Value!,
                MaxDate = (int)MaxDataNud.Value!,
                Name = NameTxt.Text.Trim(),
                Isbn = CodeIdTxt.Text.Trim(),
               
                Authors = AuthorsTxt.Text.Trim(),
                HasAvailable = (bool)HasAvailableCb.IsChecked!  
            };
        }

        public CatalogWindow(CatalogService catalogService, bool isSelectBook = false)
        {
            _isSelectBook = isSelectBook;
            _catalogService = catalogService;

            InitializeComponent();
        }

        private void CatalogDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            if (_isSelectBook)
            {
                HasAvailableCb.Visibility = Visibility.Collapsed;
            }

            SetOptions(SearchOptions.Default);
            SetDataGrid();
        }

        private void SearchInCatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            SetDataGrid();
        }

        private void ClearBtn_Click(object sender, RoutedEventArgs e)
        {
            SetOptions(SearchOptions.Default);
            SetDataGrid();
        }

        private void SetDataGrid()
        {
            CatalogDataGrid.ItemsSource = _catalogService.Search(GetOptions());
        }

        private void CatalogDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (CatalogDataGrid.SelectedItem is Book book && _isSelectBook)
            {
                var res = MessageBox.Show($"Вы уверены, что хотите выбрать книгу {book.Name}?", "Подтверждение", MessageBoxButton.YesNo);

                if(res == MessageBoxResult.Yes)
                {
                    OnAction?.Invoke(Actions.Select, book);
                    Close();
                }
            }
        }
    }
}
