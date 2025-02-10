using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using Npgsql.TypeHandlers;

namespace BookCatalogueGUI
{
    public partial class Add : Form
    {
        public Add()
        {
            InitializeComponent();
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn1Add_Click(object sender, EventArgs e)
        {
            var book = new Book
            {
                Title = txtTitle.Text,
                Pages = int.Parse(txtPages.Text),
                Publisher = txtPublisher.Text,
                ReleaseYear = int.Parse(txtReleaseYear.Text),
                Summary = txtSummary.Text,
                ISBN = txtIsbn.Text
            };

            var author = new Author
            {
                FirstName = txtFirstName.Text,
                LastName = txtLastName.Text
            };

            AddBook(book, author);

            // Showing a message that the book was added successfully
            ShowPopup();

            // Clearing the textboxes after adding the book
            txtTitle.Text = "";
            txtPages.Text = "";
            txtPublisher.Text = "";
            txtReleaseYear.Text = "";
            txtSummary.Text = "";
            txtIsbn.Text = "";
            txtFirstName.Text = "";
            txtLastName.Text = "";


            // Setting focus back to the first TextBox
            txtTitle.Focus();
        }

        public static void AddBook(Book book, Author author)
        {
            // Getting the environmental variable (database password)
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrEmpty(dbPassword))
            {
                Console.WriteLine("Password is not set in environment variables.");
                return;
            }

            string connString = $"Host=localhost;Username=postgres;Password={dbPassword}; Database=book_archive";

            // Opening the connection to the database
            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    Console.WriteLine("Connection Established!");

                    // Insert a book into the "Book" table
                    string bookSql = "INSERT INTO \"Book\" (\"Title\", \"Pages\", \"Publisher\", \"ReleaseYear\", \"Summary\", \"ISBN\") " +
                                     "VALUES (@title, @pages, @publisher, @releaseYear, @summary, @isbn) RETURNING \"Id\"";

                    int bookId;
                    using (var cmd = new NpgsqlCommand(bookSql, conn))
                    {
                        cmd.Parameters.AddWithValue("title", book.Title);
                        cmd.Parameters.AddWithValue("pages", book.Pages);
                        cmd.Parameters.AddWithValue("publisher", book.Publisher);
                        cmd.Parameters.AddWithValue("releaseYear", book.ReleaseYear);
                        cmd.Parameters.AddWithValue("summary", book.Summary);
                        cmd.Parameters.AddWithValue("isbn", book.ISBN);

                        bookId = (int)cmd.ExecuteScalar();
                        Console.WriteLine($"Book added with ID: {bookId}");
                    }

                    // Check if the Author already exists - if not, insert a new author
                    int authorId;
                    string authorSql = "SELECT \"Id\" FROM \"Author\" WHERE \"FirstName\" = @firstName AND \"LastName\" = @lastName";

                    using (var cmd = new NpgsqlCommand(authorSql, conn))
                    {
                        cmd.Parameters.AddWithValue("firstName", author.FirstName);
                        cmd.Parameters.AddWithValue("lastName", author.LastName);

                        var result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            // Author exists and gets the existing ID
                            authorId = (int)result;
                        }
                        else
                        {
                            // Author doesn't exist: enter new author
                            string insertAuthorSql = "INSERT INTO \"Author\" (\"FirstName\", \"LastName\") " + "VALUES (@firstName, @lastName) RETURNING \"Id\"";

                            using (var insertCmd = new NpgsqlCommand(insertAuthorSql, conn))
                            {
                                insertCmd.Parameters.AddWithValue("firstName", author.FirstName);
                                insertCmd.Parameters.AddWithValue("lastName", author.LastName);

                                authorId = (int)insertCmd.ExecuteScalar();
                                // ExecuteScalar is a method from the NpgsqlCommand
                                // It executes the query and returns the first column of the first row in the result set
                                // If there are no rows returned, it returns null

                                Console.WriteLine($"New author added with ID: {authorId}");
                            }

                        }
                    }

                    // Connect Author and Book inside the "AuthorBook" table
                    string authorBooksSql = "INSERT INTO \"AuthorBook\" (\"AuthorId\", \"BookId\") " + "VALUES (@authorId, @bookId)";

                    using (var cmd = new NpgsqlCommand(authorBooksSql, conn))
                    {
                        cmd.Parameters.AddWithValue("authorId", authorId);
                        cmd.Parameters.AddWithValue("bookId", bookId);

                        cmd.ExecuteNonQuery();
                        Console.WriteLine("Author and Book are now connected.");
                    }
                }

                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }

            }

        }

        private void ShowPopup()
        {
            PopupAdd popup = new PopupAdd("Book successfully added");
            popup.ShowDialog();
        }
    }
}
