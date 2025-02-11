using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;

namespace BookCatalogueGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            lblWelcome.Text = "Welcome to the Book Catalogue!";
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Create an instance of Form2
            Add add = new Add();

            // Show Form2
            add.ShowDialog();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            // Making sure the search term will be available in the "view" form
            string searchTerm = txtSearch.Text.Trim();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                ViewBooks(searchTerm);
            }
            else
            {
                MessageBox.Show("Please enter a search term.", "Search Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            // Clear search bar
            txtSearch.Text = "";
        }

        public void ViewBooks(string searchTerm)
        {
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            if (string.IsNullOrEmpty(dbPassword))
            {
                MessageBox.Show("Database password is not set in environment variables.");
                return;
            }

            string connString = $"Host=localhost;Username=postgres;Password={dbPassword}; Database=book_archive";

            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string sqlQuery = "SELECT \"Book\".\"Id\", \"Book\".\"Title\", \"Author\".\"FirstName\", \"Author\".\"LastName\", \"Book\".\"Pages\", \"Book\".\"Publisher\", \"Book\".\"ISBN\", \"Book\".\"Rating\", \"Book\".\"Summary\" " +
                                        "FROM \"Book\" " +
                                        "JOIN \"AuthorBook\" ON \"Book\".\"Id\" = \"AuthorBook\".\"BookId\" " +
                                        "JOIN \"Author\" ON \"AuthorBook\".\"AuthorId\" = \"Author\".\"Id\" " +
                                        "WHERE lower(\"Book\".\"Title\") LIKE @searchTerm " +
                                        "OR lower(\"Author\".\"FirstName\") LIKE @searchTerm " +
                                        "OR lower(\"Author\".\"LastName\") LIKE @searchTerm " +
                                        "OR lower(\"Book\".\"Publisher\") LIKE @searchTerm " +
                                        "OR lower(\"Book\".\"Summary\") LIKE @searchTerm";

                    using (var cmd = new NpgsqlCommand(sqlQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("searchTerm", $"%{searchTerm.ToLower()}%");

                        using (var adapter = new NpgsqlDataAdapter(cmd))
                        {
                            DataTable bookData = new DataTable();
                            adapter.Fill(bookData);

                            // Showing results in DataGridView
                            dataGridViewBooks.DataSource = bookData;



                            // Hiding the summary as this is supposed to be shown under the DataGridView
                            if (dataGridViewBooks.Columns["Summary"] != null)
                            {
                                dataGridViewBooks.Columns["Summary"].Visible = false;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An error occurred: {ex.Message}");
                }
            }
        }


        private void dataGridViewBooks_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Show data that was found by search
            if (e.RowIndex >= 0)
            {
                // Retrieve the Summary from the hidden column
                var summaryValue = dataGridViewBooks.Rows[e.RowIndex].Cells["Summary"].Value;

                if (summaryValue != null)
                {
                    lblSummary.Text = "Summary:\n" + summaryValue.ToString();
                }
                else
                {
                    lblSummary.Text = "Summary:\nNo summary available.";
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridViewBooks.SelectedRows.Count > 0)
            {
                // Getting the selected row in dataGridView
                DataGridViewRow selectedRow = dataGridViewBooks.SelectedRows[0];

                // Getting the book Id for the next step
                var selectedBookId = selectedRow.Cells["Id"].Value;

                if (selectedBookId != null)
                {
                    int bookId = Convert.ToInt32(selectedBookId);

                    // Popup to confirm deletion
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this book?",
                        "Confirm deletion", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    // Delete book if deletion is confirmed
                    if (result == DialogResult.Yes)
                    {
                        DeleteBook(bookId);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.", "No Book Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void DeleteBook(int bookId)
        {
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");
            if (string.IsNullOrEmpty(dbPassword))
            {
                MessageBox.Show("Database password is not set in environment variables.");
                return;
            }

            string connString = $"Host=localhost;Username=postgres;Password={dbPassword}; Database=book_archive";

            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();
                    string deleteSqlQuery1 = "DELETE FROM \"AuthorBook\" WHERE \"BookId\" = @bookId";
                    string deleteSqlQuery2 = "DELETE FROM \"Book\" WHERE \"Id\" = @bookId";

                    using (var cmd = new NpgsqlCommand(deleteSqlQuery1, conn))
                    {
                        cmd.Parameters.AddWithValue("bookId", bookId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd2 = new NpgsqlCommand(deleteSqlQuery2, conn))
                    {
                        cmd2.Parameters.AddWithValue("bookId", bookId);
                        int rowsAffected = cmd2.ExecuteNonQuery();

                        // If there is a book to delete
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Book deleted successfully!");

                            // Updating DataGridView after deletion by reloading all books
                            ViewBooks("");
                        }
                        else
                        {
                            MessageBox.Show("No book found with the given ID.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private void btnRate_Click(object sender, EventArgs e)
        {
            if (dataGridViewBooks.SelectedRows.Count > 0)
            {
                // Getting the selected row in dataGridView
                DataGridViewRow selectedRow = dataGridViewBooks.SelectedRows[0];

                // Getting the book Id for the next step
                var selectedBookId = selectedRow.Cells["Id"].Value;

                if (selectedBookId != null)
                {
                    int bookId = Convert.ToInt32(selectedBookId);

                    // Accessing Form "Rate" to pass the Id of the selected book
                    Rate rateForm = new Rate();
                    rateForm.selectedBookId = bookId;
                    rateForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Please select a book to delete.", "No Book Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
