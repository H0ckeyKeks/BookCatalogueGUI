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
    public partial class PopupAdd : Form
    {
        private Timer timer;

        public PopupAdd(string message)
        {
            this.Text = "Notification";
            this.Size = new Size(300, 150);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterScreen;

            Label label = new Label();
            label.Text = message;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.Dock = DockStyle.Fill;
            label.BackColor = Color.Transparent;
            this.Controls.Add(label);

            timer = new Timer();
            // Close after 20 seconds
            timer.Interval = 20000;
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            timer.Stop();
            this.Close();
        }
    }
}
