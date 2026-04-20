using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Chess
{
    public enum AlertType
    {
        White,
        Black
    }

    public partial class Alert : Form
    {
        AlertType type;
        string title;
        string description;

        public Alert(AlertType ty, string t, string d)
        {
            type = ty;
            title = t;
            description = d;
            InitializeComponent();
        }

        private void Alert_Load(object sender, EventArgs e)
        {
            if (type == AlertType.Black)
                pictureBox1.Image = Properties.Resources.queen_b;

            label1.Text = title;
            label2.Text = title;
            label3.Text = description;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
