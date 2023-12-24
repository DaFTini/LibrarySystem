using AutoLibraryUI.Entities;
using AutoLibraryUI.Services;
using AutoLibraryUI.Views.ReaderViews;
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
    /// Логика взаимодействия для EditReaderWindow.xaml
    /// </summary>
    public partial class EditReaderWindow : Window
    {
        public event Action<Actions>? OnAction;

        private readonly RegistationList _accountsService; 
        private readonly CatalogService _catalogService;
        private readonly int _id;
        private Reader _reader;

        public EditReaderWindow(RegistationList accountsService, CatalogService catalogService, int id)
        {
            InitializeComponent();

            _accountsService = accountsService;
            _catalogService = catalogService;
            _id = id;
            _reader = new Reader();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _reader = _accountsService.GetReaderById(_id);
            BookingTableDg.ItemsSource = _accountsService.GetBookings(_id);
            SetInputsWithData();
        }

        private void SetInputsWithData()
        {
            CardIdLbl.Content = _reader.Id;
            RegDateLbl.Content = _reader.RegistrationDateTime.ToLocalTime().ToString("d");
            FirstNameTxt.Text = _reader.FirstName;
            LastNameTxt.Text = _reader.LastName;
            PatronimicTxt.Text = _reader.Patronimic;
        }

        private bool HasEmptyInputs()
        {
            return FirstNameTxt.Text.Trim().Length == 0 ||
                   LastNameTxt.Text.Trim().Length == 0 ||
                   PatronimicTxt.Text.Trim().Length == 0;
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены что хотите удалить читателя?", "Удаление читателя", MessageBoxButton.YesNo);

            if(result == MessageBoxResult.Yes)
            {
                _accountsService.DeleteReader(_reader!.Id);
                OnAction?.Invoke(Actions.Deleted);
                Close();
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            SetInputsWithData();
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if(HasEmptyInputs())
            {
                MessageBox.Show("Некорректно заполнены поля. Сохранение невозможно!");
            }
            else
            {
                var firstName = FirstNameTxt.Text;
                var lastName = LastNameTxt.Text;
                var patronimic = PatronimicTxt.Text;

                _reader = _accountsService.EditReaderPersonalData(_id, firstName, lastName, patronimic);
                OnAction?.Invoke(Actions.Edited);
                MessageBox.Show("Данные успешно сохранены!");

                SetInputsWithData();
            }
        }

        private void GetBookBtn_Click(object sender, RoutedEventArgs e)
        {
            Book? selectedBook = null;
            bool bookIsSelected = false;
            bool datesIsSelected = false;
            DateTime? d1 = null, d2 = null;
            CatalogWindow catalogWindow = new CatalogWindow(_catalogService, true);

            catalogWindow.OnAction += (action, book) =>
            {
                if(action is Actions.Select)
                {
                    selectedBook = book;
                    bookIsSelected = true;
                }
            };
            catalogWindow.ShowDialog();

            if(!bookIsSelected)
            {
                MessageBox.Show("Взятие книги было отменено!");
                return;
            }
            
            TakeBookWindow takeBookWindow = new TakeBookWindow();

            takeBookWindow.OnAction += (action, takeTime, returnTime) =>
            {
                d1 = takeTime;
                d2 = returnTime;
                datesIsSelected = true;
            };

            takeBookWindow.ShowDialog();

            if(!datesIsSelected)
            {
                MessageBox.Show("Взятие книги было отменено!");
                return;
            }

            if (datesIsSelected && bookIsSelected && selectedBook != null && d1 != null && d2 != null)
            {
                _accountsService.TakeBook(_id, selectedBook.Id, (DateTime)d1, (DateTime)d2);
                BookingTableDg.ItemsSource = _accountsService.GetBookings(_id);
            }
        }

        private void ReturnBookBtn_Click(object sender, RoutedEventArgs e)
        {
            if(BookingTableDg.SelectedItem is Booking booking)
            {
                if(booking.IsReturned)
                {
                    MessageBox.Show("Книга уже возвращена!");               
                }

                _accountsService.ReturnBook(booking.Id);
                BookingTableDg.ItemsSource = _accountsService.GetBookings(_id);
            }
            else
            {
                MessageBox.Show("Книга для возврата не выбрана!");
            }
            
        }
    }
}
