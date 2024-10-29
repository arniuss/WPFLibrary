using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data;

namespace WPF_Library
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DatabaseManager _databaseManager;
        public MainWindow()
        {
            InitializeComponent();
            _databaseManager = new DatabaseManager();
            LoadData();
        }

        public void LoadData()
        {
            DataTable books = _databaseManager.GetBooks();
            dgBooks.ItemsSource = books.DefaultView;

            DataTable authors = _databaseManager.GetAuthors();
            dgAuthors.ItemsSource = authors.DefaultView;

            DataTable members = _databaseManager.GetMembers();
            dgMembers.ItemsSource = members.DefaultView;
        }

        private void GetBookCountByAuthor_Click(object sender, RoutedEventArgs e)
        {
            DataTable countOfBooks = _databaseManager.GetBookCountByAuthor();
            dgResult.ItemsSource = countOfBooks.DefaultView;
        }

        private void BooksBorrowedLastYear_Click(object sender, RoutedEventArgs e)
        {
            DataTable booksBorrowedLastYear = _databaseManager.GetbooksBorrowedLastYear();
            dgResult.ItemsSource = booksBorrowedLastYear.DefaultView;
        }

        private void AddMember_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbName.Text))
            {
                MessageBox.Show("Name cannot be empty");
                return;
            }

            _databaseManager.AddMember(tbName.Text);
            LoadData();
        }

        private void DeleteRecordsOlderThan5Years_Click(object sender, RoutedEventArgs e)
        {
            _databaseManager.DeleteRecordsOlderThan5Years();
            LoadData();
        }

        private void JKRowlingBooks_Click(object sender, RoutedEventArgs e)
        {
            DataTable jkRowlingBooks = _databaseManager.GetAllJKRowlingBooks();
            dgResult.ItemsSource = jkRowlingBooks.DefaultView;
        }

        private void BooksWithAuthorsStatus_Click(object sender, RoutedEventArgs e)
        {
            DataTable booksWithAuthorsStatus = _databaseManager.GetBooksWithAuthorsStatus();
            dgResult.ItemsSource = booksWithAuthorsStatus.DefaultView;
        }

        private void LastBorrowedBookByEachMember_Click(object sender, RoutedEventArgs e)
        {
            DataTable lastBorrowedBookByEachMember = _databaseManager.GetLastBorrowedBooks();
            dgResult.ItemsSource = lastBorrowedBookByEachMember.DefaultView;
        }
    }
}