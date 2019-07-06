using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework02
{
    class Program
    {
        private static string _connectionString = @"Server=DESKTOP-8FCTU48\SQLEXPRESS;Database=AdoNetDB;Trusted_Connection=True;";

        static void Main(string[] args)
        {
            //AddAuthor();
            //AddBook();
            //EditAuthor();
            //EditBook();
            GetAllAuthorsWithBooks();

            Console.ReadLine();
        }
        private static void AddAuthor()
        {
            Console.WriteLine("Please enter the name of the Author: ");
            var firstName = Console.ReadLine();
            Console.WriteLine("Please enter the surname of the Author: ");
            var lastName = Console.ReadLine();

            using (var connection = new SqlConnection(_connectionString))
            {

                var newAuthor = connection.Execute("AddAuthor", new { FirstName = firstName, LastName = lastName }, commandType: CommandType.StoredProcedure);

                Console.WriteLine("Successfully added an Author");

            }
        }
        private static void AddBook()
        {
            Console.WriteLine("Please enter the title of the Book: ");
            var title = Console.ReadLine();
            Console.WriteLine("Genre: ");
            var genre = Console.ReadLine();
            Console.WriteLine("Author's ID: ");
            var authorId = Console.ReadLine();

            string sql = $"INSERT INTO Book (Title, Genre, AuthorID) VALUES('{title}', '{genre}', '{authorId}');";

            using (var connection = new SqlConnection(_connectionString))
            {
                var newBook = connection.Query(sql);

                Console.WriteLine("Successfully added a book");
            }
        }
        private static void EditAuthor()
        {
            Console.WriteLine("Please enter the name of the Author: ");
            var authorsName = Console.ReadLine();
            Console.WriteLine("Please enter the first name of the Author: ");
            var firstName = Console.ReadLine();
            Console.WriteLine("Last Name: ");
            var lastName = Console.ReadLine();

            string sql = $"UPDATE Author SET FirstName = '{firstName}', LastName = '{lastName}' WHERE FirstName LIKE '%{authorsName}%';";

            using (var connection = new SqlConnection(_connectionString))
            {
                var editedAuthor = connection.Query(sql);

                Console.WriteLine("Successfully edited an Author");
            }
        }
        private static void EditBook()
        {
            Console.WriteLine("Please enter a Title: ");
            var title = Console.ReadLine();
            Console.WriteLine("Please enter a Genre: ");
            var genre = Console.ReadLine();
            Console.WriteLine("Please enter an id for Author: ");
            var authorId = Console.ReadLine();

            string sql = $"UPDATE Book SET Title = '{title}', Genre = '{genre}' WHERE AuthorID = {authorId};";

            using (var connection = new SqlConnection(_connectionString))
            {
                var editedBook = connection.Query(sql);

                Console.WriteLine("Successfully edited a Book");
            }
        }
        private static void GetAllAuthorsWithBooks()
        {
            string sql = "SELECT * FROM Book AS B INNER JOIN Author AS A ON A.ID = B.AuthorID";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                var booksDictionary = new Dictionary<int, Book>();

                var books = connection.Query<Book, Author, Book>(sql,
                    (book, author) =>
                    {
                        book.Author = author;
                        return book;
                    },
                    splitOn: "AuthorID")
                    .Distinct().ToList();

                foreach (var book in books)
                {
                    Console.WriteLine($"{book.Author.FirstName} {book.Author.LastName} - {book.Title}");
                }
            }
        }
    }
}
