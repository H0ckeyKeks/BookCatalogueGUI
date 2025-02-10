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

        private void btnView_Click(object sender, EventArgs e)
        {
            Search search = new Search();

            search.ShowDialog();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            Delete delete = new Delete();

            delete.ShowDialog();
        }
    }
}
