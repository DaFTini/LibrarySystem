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

namespace AutoLibraryUI.Views.SystemViews
{
    /// <summary>
    /// Логика взаимодействия для RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly RegistationList _accountsService;

        public RegisterWindow(RegistationList accountsService)
        {
            _accountsService = accountsService;
            InitializeComponent();

            RoleSelect.ItemsSource = new string[]
            {
                Roles.Administrator,
                Roles.Librarian
            };
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            var username = Login.Text;
            var password = Password.Text;
            var name = FullName.Text;
            var role = RoleSelect.SelectedItem as string;

            if(string.IsNullOrEmpty(username) || string.IsNullOrWhiteSpace(password) || 
                    string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Заполнены не все обязательные поля");
            }
            else
            {
                if(_accountsService.Register(new Librarian
                {
                    FullName = name,
                    Role = role,
                    Username = username,
                }, password))
                {
                    MessageBox.Show("Пользователь зарегистрирован");
                }
                else
                {
                    MessageBox.Show("Регистрационные данные некорректны");
                }             
            }
        }
    }
}
