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
    public partial class Start : Form
    {
        bool isDragging = false;
        Point dragStartPoint;

        public Start()
        {
            InitializeComponent();
        }

        private void Start_Load(object sender, EventArgs e)
        {
            foreach (object item in this.Controls)
            {
                if (item is Panel) {
                    Panel btn = item as Panel;
                    Label lbl = new Label();

                    if (btn.Tag == null) continue;

                    lbl.BackColor = Color.Transparent;
                    lbl.ForeColor = Color.White;
                    lbl.AutoSize = true;
                    lbl.Text = btn.Tag.ToString().Split('_')[1];
                    lbl.Font = new System.Drawing.Font("Arial Rounded MT Bold", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));

                    if (btn.Tag.ToString().StartsWith("btn_"))
                    {
                        btn.Controls.Add(lbl);
                        lbl.Top = btn.Height / 2 - lbl.Height / 2;
                        lbl.Left = btn.Width / 2 - lbl.Width / 2;
                    }
                    else if (btn.Tag.ToString().StartsWith("select_"))
                    {
                        btn.Controls.Add(lbl);
                        lbl.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        lbl.Top = btn.Height / 2 - lbl.Height / 2;
                        lbl.Left = 15;

                        Panel down_icon = new Panel();
                        down_icon.BackgroundImage = Properties.Resources.arrow_down;
                        down_icon.BackColor = Color.Transparent;
                        down_icon.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
                        down_icon.Size = new Size(15, 10);
                        btn.Controls.Add(down_icon);

                        down_icon.Top = btn.Height / 2 - down_icon.Height / 2;
                        down_icon.Left = btn.Width - down_icon.Width - 15;
                    }
                    else continue;
                }
            }
        }

        private void play_btn_Click(object sender, EventArgs e)
        {
            Board board = new Board();
            board.Tag = this;
            this.Visible = false;
            board.Show();
        }

        private void panel2_Click(object sender, EventArgs e)
        {
            Application.Exit();
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

        private void panel3_Click(object sender, EventArgs e)
        {
            SelectTime s = new SelectTime();
            Label lbl = time.Controls[0] as Label;

            s.selected = lbl.Text;

            if (DialogResult.OK == s.ShowDialog())
                lbl.Text = s.selected;
        }

        private void Start_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
