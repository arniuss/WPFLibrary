using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPF_Library
{
    public class DatabaseManager
    {
        private static readonly string _connectionString = ConfigurationManager.ConnectionStrings["LibraryDB"].ConnectionString;

        public static void ExecuteQuery(string query)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        public DataTable GetBooks()
        {
            DataTable books = new DataTable();

            string query = "SELECT * FROM BooksWithAuthors";

            using(SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(books);
                }
            }

            return books;
        }
        public DataTable GetAuthors()
        {
            DataTable authors = new DataTable();

            string query = "SELECT * FROM Authors";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(authors);
                }
            }

            return authors;
        }
        public DataTable GetMembers()
        {
            DataTable members = new DataTable();

            string query = "SELECT * FROM Members";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(members);
                }
            }

            return members;
        }

        public DataTable GetBookCountByAuthor()
        {
            DataTable result = new DataTable();

            string query = "SELECT a.Name AS Author_Name, COUNT(b.BookID) AS BookCount " +
                            "FROM Authors a " +
                            "LEFT JOIN BookAuthors ba ON a.AuthorID = ba.AuthorID " +
                            "LEFT JOIN Books b ON ba.BookID = b.BookID " +
                            "GROUP BY a.Name " +
                            "ORDER BY BookCount DESC;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(result);
                }
            }

            return result;
        }

        internal DataTable GetbooksBorrowedLastYear()
        {
            DataTable result = new DataTable();

            string query = "SELECT b.Title, m.FullName AS MemberName, bh.BorrowDate " +
                            "FROM BorrowingHistory bh " +
                            "JOIN Books b ON b.BookID = bh.BookID " +
                            "JOIN Members m ON m.MemberID = bh.MemberID " +
                            "WHERE bh.BorrowDate >= DATEADD(year, -1, GETDATE()) " +
                            "ORDER BY bh.BorrowDate DESC;";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(result);
                }
            }

            return result;
        }

        public void AddMember(string name)
        {
            string query = "INSERT INTO Members (FullName, MembershipDate) VALUES (@name, GETDATE());";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteRecordsOlderThan5Years()
        {
            string query = "DELETE FROM BorrowingHistory WHERE ReturnDate < DATEADD(year, -5, GETDATE())";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public DataTable GetAllJKRowlingBooks()
        {
            DataTable result = new DataTable();

            string query = "SELECT * FROM BooksWithAuthors WHERE AuthorName = 'J.K. Rowling';";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(result);
                }
            }

            return result;
        }

        public DataTable GetBooksWithAuthorsStatus()
        {
            DataTable result = new DataTable();

            string query = "SELECT b.Title, CASE WHEN ba.AuthorID IS NOT NULL THEN 'Yes' ELSE 'No' END AS Has_Author " +
                            "FROM Books b " +
                            "LEFT JOIN BookAuthors ba ON b.BookID = ba.BookID";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(result);
                }
            }

            return result;
        }
        public DataTable GetLastBorrowedBooks()
        {
            DataTable result = new DataTable();

            string query = "SELECT m.FullName, bh.LastBorrowedDate FROM Members m " +
                            "LEFT JOIN (SELECT bh.MemberID, MAX(bh.BorrowDate) AS LastBorrowedDate " +
                            "FROM BorrowingHistory bh " +
                            "GROUP BY bh.MemberID) bh ON m.MemberID = bh.MemberID;";


            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    connection.Open();
                    adapter.Fill(result);
                }
            }

            return result;
        }
    }
}
