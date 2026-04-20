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
    public partial class SelectTime : Form
    {
        public string selected = "";

        public SelectTime()
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

                    if (btn.Tag.ToString().StartsWith("select_"))
                    {
                        if (lbl.Text == selected) {
                            btn.BorderStyle = BorderStyle.FixedSingle;
                        }

                        btn.Controls.Add(lbl);
                        lbl.Font = new System.Drawing.Font("Arial Rounded MT Bold", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                        lbl.Top = btn.Height / 2 - lbl.Height / 2;
                        lbl.Left = 15;
                    }
                    else continue;
                }
            }
        }

        private void panel3_Click(object sender, EventArgs e)
        {
            Panel btn = sender as Panel;
            Label text = btn.Controls[0] as Label;

            selected = text.Text;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
