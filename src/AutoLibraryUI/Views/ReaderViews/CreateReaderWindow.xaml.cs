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
    /// Логика взаимодействия для CreateReaderWindow.xaml
    /// </summary>
    public partial class CreateReaderWindow : Window
    {
        public event Action<Actions>? OnAction;

        private readonly RegistationList _accountsService;
        private Reader? _reader;

        public CreateReaderWindow(RegistationList accountsService)
        {
            InitializeComponent();

            _accountsService = accountsService;

            SaveBtn.IsEnabled = false;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            SetInputsEmpty();
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

                _reader = _accountsService.CreateReader(firstName, lastName, patronimic);

                MessageBox.Show("Данные успешно сохранены!");

                OnAction?.Invoke(Actions.Created);

                SetInputsWithData();

                FirstNameTxt.IsEnabled = false;
                LastNameTxt.IsEnabled = false;
                PatronimicTxt.IsEnabled = false;
                SaveBtn.IsEnabled = false;
                CancelBtn.IsEnabled = false;
            }
        }

        private void SetInputsEmpty()
        {
            CardIdLbl.Content = "-";
            RegDateLbl.Content = "-";
            FirstNameTxt.Text = "";
            LastNameTxt.Text = "";
            PatronimicTxt.Text = "";
        }

        private void SetInputsWithData()
        {
            if(_reader == null)
                return;

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

        private void DataInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(!HasEmptyInputs())
            {
                SaveBtn.IsEnabled = true;
            }
            else
            {
                SaveBtn.IsEnabled = false;
            }
        }
    }
}
