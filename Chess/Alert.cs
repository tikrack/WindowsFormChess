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
        Black,
        Info
    }

    public partial class Alert : Form
    {
        bool isDragging = false;
        Point dragStartPoint;

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

            if (type == AlertType.Info)
                pictureBox1.Image = Properties.Resources.info;

            label1.Text = title;
            label2.Text = title;
            label3.Text = description;
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = true;
                dragStartPoint = new Point(e.X, e.Y);
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point newLocation = this.Location;
                newLocation.X += e.X - dragStartPoint.X;
                newLocation.Y += e.Y - dragStartPoint.Y;

                this.Location = newLocation;
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
            }
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
