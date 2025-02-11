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
    public partial class Rate : Form
    {
        public Rate()
        {
            InitializeComponent();
        }

        public int selectedBookId { get; set; }

        private void btnMain2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMain3_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnMain4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void AddRating()
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

                    // Getting the user input for each category
                    double plotRating = GetRating(txtPlot.Text);
                    double characterRating = GetRating(txtCharacters.Text);
                    double writingRating = GetRating(txtWriting.Text);
                    double settingRating = GetRating(txtSetting.Text);
                    double tensionRating = GetRating(txtTension.Text);
                    double emotionRating = GetRating(txtEmotion.Text);
                    double effectRating = GetRating(txtEffect.Text);


                    // Calculating the average rating
                    double averageRating = (plotRating + characterRating + writingRating + settingRating + tensionRating + emotionRating + effectRating) / 7;

                    // round rating to 2 decimal points
                    double rating = Math.Round(averageRating,2);


                    // SQL query to add the rating to the database
                    string ratingQuery = "UPDATE \"Book\" SET \"Rating\" = @rating WHERE \"Id\" = @ratingId";

                    using (var cmd = new NpgsqlCommand(ratingQuery, conn))
                    {
                        cmd.Parameters.AddWithValue("ratingId", selectedBookId);
                        cmd.Parameters.AddWithValue("rating", rating);
                        int booksRated = cmd.ExecuteNonQuery();

                        if (booksRated > 0)
                        {
                            MessageBox.Show("Rating successfully added!");
                        }
                        else
                        {
                            MessageBox.Show("No rating was updated.");
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }
            }
        }

        private double GetRating(string userInput)
        {
            double rating;
            if (double.TryParse(userInput, out rating) && rating >= 0 && rating <= 5)
            {
                return rating;
            }
            else
            {
                MessageBox.Show("Please enter a valid rating between 0 and 5.");
                return 0;
            }
        }

        private void btnRating_Click(object sender, EventArgs e)
        {
            AddRating();
        }
    }
}
