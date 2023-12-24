using AutoLibraryUI.Entities;
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
    /// Логика взаимодействия для CreateBookWindow.xaml
    /// </summary>
    public partial class CreateBookWindow : Window
    {
        public event Action<Actions>? OnAction;

        public CreateBookWindow()
        {
            InitializeComponent();
        }

        public Book ReadBook()
        {
            return new Book
            {
                Code = Isbn.Text,
                Name = BookName.Text,
                AvailableCount = BookCount.Value ?? 0,
                PublicationLocation = PublishLocation.Text,
                PublicationYear = PublicationYear.Value ?? 0,
                
            };
        }

    }
}
