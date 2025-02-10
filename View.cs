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
    public partial class View : Form
    {

        public View(string searchTerm)
        {
            InitializeComponent();

            ViewBooks(searchTerm);
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



        private void dataGridViewBooks_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Making sure a row is selected
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

        private void btnMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
