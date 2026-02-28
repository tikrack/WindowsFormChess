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
    public partial class Form1 : Form
    {
        Panel panel;
        PictureBox selectedChessman;

        int[,] map = new int[8,8];

        Dictionary<int, Bitmap> ChessmansWhite = new Dictionary<int, Bitmap>()
        {
            { 1, Properties.Resources.pawn_w },
            { 2, Properties.Resources.rook_w },
            { 3, Properties.Resources.knight_w },
            { 4, Properties.Resources.bishop_w },
            { 5, Properties.Resources.queen_w },
            { 6, Properties.Resources.king_w },
        };

        PictureBox chessman(Bitmap image, string tag)
        {
            PictureBox k = new PictureBox();

            k.Image = image;
            k.BackColor = Color.Transparent;
            k.Size = new Size(100, 100);
            k.SizeMode = PictureBoxSizeMode.Zoom;
            k.Location = new Point(0, 0);
            k.Cursor = Cursors.Hand;
            k.Tag = tag;
            k.Click += new System.EventHandler(this.chessman_click);

            return k;
        }

        string ConvertToChessNotation(int row, int col)
        {
            char file = (char)('a' + col);
            int rank = 8 - row;

            return String.Format("{0}{1}", file, rank);
        }

        int[] ConvertFromChessNotation(string notation)
        {
            char file = notation[0];
            char rankChar = notation[1];
            int rank = int.Parse(rankChar.ToString());

            int col = file - 'a';

            int row = 8 - rank;

            return new int[2] {row, col};
        }


        string getChessmanName(int type)
        {
            switch (type)
            {
                case 1:
                    return "pawn@w";
                case 2:
                    return "rook@w";
                case 3:
                    return "knight@w";
                case 4:
                    return "bishop@w";
                case 5:
                    return "queen@w";
                case 6:
                    return "king@w";
            }

            return "";
        }

        string renderCellName(int index, bool num = true, bool lett = true)
        {
            char letter = (char)(index % 8 + 97);

            if (num && lett)
                return letter.ToString() + (8 - (index / 8)).ToString();
            else if (num)
                return (8 - (index / 8)).ToString();
            else if (lett)
                return letter.ToString();
            else
                return "";
        }

        bool ValidateMove(int[] from, int[] to, string tag)
        {
            Console.WriteLine("chessman => {0} {1}", from[0], from[1]);
            Console.WriteLine("panel => {0} {1}", to[0], to[1]);

            string[] details = tag.Split('@');

            if (details[1] == "w")
            {
                if (details[0] == "pawn") {
                    if (from[1] == to[1])
                        if (from[0] == 6)
                            if (from[0] - 2 == to[0] || from[0] - 1 == to[0])
                                return true;
                        else
                            if (from[0] - 1 == to[0])
                                return true;
                }

                if (details[0] == "rook")
                {
                    if (from[0] == to[0] || from[1] == to[1])
                        return true;
                }
            }

            return false;
        }

        void SyncUi()
        {
            foreach (object item in panel.Controls)
            {
                Panel p = item as Panel;
                p.Controls.Clear();
            }

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (map[i, j] != 0)
                    {
                        PictureBox k = chessman(ChessmansWhite[map[i, j]], getChessmanName(map[i, j]));

                        foreach (object item in panel.Controls)
                        {
                            Panel p = item as Panel;

                            if (p.Name == ConvertToChessNotation(i, j))
                                p.Controls.Add(k);
                        }
                    }
                }
            }
        }

        void InitializeBoard()
        {
            panel = new Panel();

            panel.Size = new Size(800, 800);
            panel.BorderStyle = BorderStyle.FixedSingle;
            panel.BackColor = Color.Transparent;

            this.Controls.Add(panel);

            bool state = false;

            for (int i = 0; i < 64; i++)
            {
                if (i % 8 != 0)
                    state = !state;

                Panel cell = new Panel();

                cell.Location = new Point(100 * (i % 8), 100 * (i / 8));
                cell.Size = new Size(100, 100);
                cell.Click += new System.EventHandler(this.panel_click);

                if (state)
                    cell.BackColor = Color.FromArgb(181, 136, 99);
                else
                    cell.BackColor = Color.FromArgb(240, 217, 181);

                //if (i % 8 == 0)
                //    cell.Text = renderCellName(i, true, false);

                //if ((i / 8) + 1 == 8)
                //    cell.Text = renderCellName(i, false, true);

                cell.Name = renderCellName(i);

                panel.Controls.Add(cell);
            }
        }

        void InitializeKeys()
        {
            //Pawn
            map[6, 0] = 1;
            map[6, 1] = 1;
            map[6, 2] = 1;
            map[6, 3] = 1;
            map[6, 4] = 1;
            map[6, 5] = 1;
            map[6, 6] = 1;
            map[6, 7] = 1;
            //Other
            map[7, 0] = 2;
            map[7, 1] = 3;
            map[7, 2] = 4;
            map[7, 3] = 5;
            map[7, 4] = 6;
            map[7, 5] = 4;
            map[7, 6] = 3;
            map[7, 7] = 2;
        }

        void InitializeDebugKeys()
        {
            map[4, 4] = 2;
            map[4, 6] = 1;
        }

        public Form1()
        {
            InitializeComponent();
            InitializeBoard();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel.Location = new Point((this.ClientSize.Width / 2) - (panel.Width / 2), 0);
            //InitializeKeys();
            InitializeDebugKeys();
            SyncUi();
        }

        void panel_click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;

            if (selectedChessman == null)
            {
                return;
            }

            int[] chessmanPos = ConvertFromChessNotation((selectedChessman.Parent as Panel).Name);
            int[] panelPos = ConvertFromChessNotation(p.Name);

            if (!ValidateMove(chessmanPos, panelPos, selectedChessman.Tag.ToString()))
            {
                MessageBox.Show("Wrong Move!", "Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            map[panelPos[0], panelPos[1]] = map[chessmanPos[0], chessmanPos[1]];
            map[chessmanPos[0], chessmanPos[1]] = 0;

            SyncUi();

            //selectedChessman = null;
        }

        void chessman_click(object sender, EventArgs e)
        {
            PictureBox c = sender as PictureBox;
            selectedChessman = c;

            foreach (object item in panel.Controls)
            {
                Panel p = item as Panel;

                foreach (object pic in p.Controls)
                    if (pic is PictureBox && (pic as PictureBox).Tag != c.Tag)
                        p.Enabled = false;
            }

            if (c.Tag.ToString().Split('@')[1] == "w")
            {
                if (c.Tag.ToString().Split('@')[0] == "pawn")
                {
                    int[] position = ConvertFromChessNotation((c.Parent as Panel).Name);
                }
            }

            // MessageBox.Show(c.Tag.ToString());
        }
    }
}