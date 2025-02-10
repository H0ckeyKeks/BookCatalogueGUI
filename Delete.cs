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
    public partial class Delete : Form
    {
        // Creating an instance of View to be able to access the method "ViewBooks"
        private View viewInstance;
        
        public Delete()
        {
            InitializeComponent();

            viewInstance = new View("");
        }

        private void txtDelete_TextChanged(object sender, EventArgs e)
        {
            string searchTerm = txtDelete.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                viewInstance.ViewBooks(searchTerm);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string searchTerm = txtDelete.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                viewInstance.ViewBooks(searchTerm);
            }
            else
            {
                MessageBox.Show("Please enter a search term.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtDeleteId.Text))
            {
                MessageBox.Show("Please enter the book ID to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Asking for confirmation of deletion
            DialogResult result = MessageBox.Show("Are you sure you want to delete this book?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            // If deletion is confirmed
            if (result == DialogResult.Yes)
            {
                int bookId;
                if (int.TryParse(txtDeleteId.Text, out bookId))
                {
                    // Call the method that deletes the book
                    if (DeleteBook(bookId))
                    {
                        MessageBox.Show("Book deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Book deletion failed. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Invalid book ID. Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public bool DeleteBook(int bookId)
        {
            string dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD");

            if (string.IsNullOrEmpty(dbPassword))
            {
                Console.WriteLine("Password is not set in environment variables.");
                return false;
            }

            string connString = $"Host=localhost;Username=postgres;Password={dbPassword}; Database=book_archive";

            // Opening the connection to the database
            using (var conn = new NpgsqlConnection(connString))
            {
                try
                {
                    conn.Open();

                    string sqlQuery1 = "DELETE FROM \"AuthorBook\" WHERE \"BookId\" = @bookId";
                    string sqlQuery2 = "DELETE FROM \"Boook\" WHERE \"Id\" = @bookId";

                    using (var cmd = new NpgsqlCommand(sqlQuery1, conn))
                    {
                        cmd.Parameters.AddWithValue("bookId", bookId);
                        cmd.ExecuteNonQuery();
                    }

                    using (var cmd2 = new NpgsqlCommand(sqlQuery2, conn))
                    {
                        cmd2.Parameters.AddWithValue("bookId", bookId);
                        int deletions = cmd2.ExecuteNonQuery();
                        // if there was a deletion, return true
                        return deletions > 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occured: {ex.Message}");
                    return false;
                }
            }
        }

        // Making DataGridView available outside of delete.cs
        public DataGridView DeleteDataGridView
        {
            get { return dataGridViewDelete; }
        }

        private void dataGridViewDelete_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Getting the Id of the selected Book
                var selectedBookId = dataGridViewDelete.Rows[e.RowIndex].Cells["Id"].Value;
                txtDeleteId.Text = selectedBookId.ToString();
            }
        }

        private void btnMenu_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}
