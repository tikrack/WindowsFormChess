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
    public partial class Welcome : Form
    {
        int c = 0;

        public Welcome()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            c++;

            if (c == 1)
            {
                Start start = new Start();

                start.Show();
                this.Visible = false;
            }
        }

        private void Welcome_Load(object sender, EventArgs e)
        {

        }
    }
}
