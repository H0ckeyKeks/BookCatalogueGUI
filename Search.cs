using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BookCatalogueGUI
{
    public partial class Search : Form
    {
        public Search()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            // Making sure the search term will be available in the "view" form
            string searchTerm = txtSearch.Text.Trim();

            // Create an instance of Form2
            View view = new View(searchTerm);

            // Show Form2
            view.ShowDialog();

            // Clear search bar
            txtSearch.Text = "";
        }

        private void btnMain_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
