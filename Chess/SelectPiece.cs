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
    public partial class SelectPiece : Form
    {
        public string selected = "";

        public SelectPiece()
        {
            InitializeComponent();
        }

        private void Start_Load(object sender, EventArgs e) {}

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
