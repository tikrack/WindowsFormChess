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
    public partial class Board : Form
    {
        Panel panel;
        PictureBox selectedChessman;

        int[,] map = new int[8,8];

        Dictionary<int, Bitmap> ChessmansWhite = new Dictionary<int, Bitmap>()
        {
            { -1, Properties.Resources.pawn_b },
            { -2, Properties.Resources.rook_b },
            { -3, Properties.Resources.knight_b },
            { -4, Properties.Resources.bishop_b },
            { -5, Properties.Resources.queen_b },
            { -6, Properties.Resources.king_b },

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
                case -1:
                    return "pawn@b";
                case -2:
                    return "rook@b";
                case -3:
                    return "knight@b";
                case -4:
                    return "bishop@b";
                case -5:
                    return "queen@b";
                case -6:
                    return "king@b";
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

        int getChessmanType(string name)
        {
            switch (name)
            {
                case "pawn@b":
                    return -1;
                case "rook@b":
                    return -2;
                case "knight@b":
                    return -3;
                case "bishop@b":
                    return -4;
                case "queen@b":
                    return -5;
                case "king@b":
                    return -6;
                case "pawn@w":
                    return 1;
                case "rook@w":
                    return 2;
                case "knight@w":
                    return 3;
                case "bishop@w":
                    return 4;
                case "queen@w":
                    return 5;
                case "king@w":
                    return 6;
            }

            return 0;
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
            string[] details = tag.Split('@');
            string pieceType = details[0];
            string pieceColor = details[1];

            int fromRow = from[0];
            int fromCol = from[1];
            int toRow = to[0];
            int toCol = to[1];

            int targetValue = map[toRow, toCol];

            if (pieceColor == "w" && targetValue > 0) return false;
            if (pieceColor == "b" && targetValue < 0) return false;

            if (pieceColor == "w")
            {
                if (pieceType == "pawn")
                {
                    if (fromCol == toCol)
                    {
                        if (targetValue != 0) return false;
                        if (fromRow == 6 && (toRow == 5 || toRow == 4))
                        {
                            if (toRow == 4 && map[5, fromCol] != 0) return false;
                            return true;
                        }
                        if (toRow == fromRow - 1) return true;
                    }
                    else if (Math.Abs(fromCol - toCol) == 1 && toRow == fromRow - 1)
                    {
                        if (targetValue < 0) return true;
                    }
                    return false;
                }

                if (pieceType == "rook")
                {
                    if (fromRow == toRow || fromCol == toCol)
                    {
                        if (!IsPathClear(fromRow, fromCol, toRow, toCol)) return false;
                        return true;
                    }
                    return false;
                }

                if (pieceType == "bishop")
                {
                    if (Math.Abs(fromRow - toRow) == Math.Abs(fromCol - toCol))
                    {
                        if (!IsPathClear(fromRow, fromCol, toRow, toCol)) return false;
                        return true;
                    }
                    return false;
                }

                if (pieceType == "queen")
                {
                    if (fromRow == toRow || fromCol == toCol || Math.Abs(fromRow - toRow) == Math.Abs(fromCol - toCol))
                    {
                        if (!IsPathClear(fromRow, fromCol, toRow, toCol)) return false;
                        return true;
                    }
                    return false;
                }

                if (pieceType == "king")
                {
                    if (Math.Abs(fromRow - toRow) <= 1 && Math.Abs(fromCol - toCol) <= 1) return true;
                    return false;
                }

                if (pieceType == "knight")
                {
                    int dr = Math.Abs(fromRow - toRow);
                    int dc = Math.Abs(fromCol - toCol);
                    if ((dr == 2 && dc == 1) || (dr == 1 && dc == 2)) return true;
                    return false;
                }
            }
            else if (pieceColor == "b")
            {
                if (pieceType == "pawn")
                {
                    if (fromCol == toCol)
                    {
                        if (targetValue != 0) return false;
                        if (fromRow == 1 && (toRow == 2 || toRow == 3))
                        {
                            if (toRow == 3 && map[2, fromCol] != 0) return false;
                            return true;
                        }
                        if (toRow == fromRow + 1) return true;
                    }
                    else if (Math.Abs(fromCol - toCol) == 1 && toRow == fromRow + 1)
                    {
                        if (targetValue > 0) return true;
                    }
                    return false;
                }

                if (pieceType == "rook")
                {
                    if (fromRow == toRow || fromCol == toCol)
                    {
                        if (!IsPathClear(fromRow, fromCol, toRow, toCol)) return false;
                        return true;
                    }
                    return false;
                }

                if (pieceType == "bishop")
                {
                    if (Math.Abs(fromRow - toRow) == Math.Abs(fromCol - toCol))
                    {
                        if (!IsPathClear(fromRow, fromCol, toRow, toCol)) return false;
                        return true;
                    }
                    return false;
                }

                if (pieceType == "queen")
                {
                    if (fromRow == toRow || fromCol == toCol || Math.Abs(fromRow - toRow) == Math.Abs(fromCol - toCol))
                    {
                        if (!IsPathClear(fromRow, fromCol, toRow, toCol)) return false;
                        return true;
                    }
                    return false;
                }

                if (pieceType == "king")
                {
                    if (Math.Abs(fromRow - toRow) <= 1 && Math.Abs(fromCol - toCol) <= 1) return true;
                    return false;
                }

                if (pieceType == "knight")
                {
                    int dr = Math.Abs(fromRow - toRow);
                    int dc = Math.Abs(fromCol - toCol);
                    if ((dr == 2 && dc == 1) || (dr == 1 && dc == 2)) return true;
                    return false;
                }
            }

            return false;
        }

        bool IsPathClear(int fromRow, int fromCol, int toRow, int toCol)
        {
            if (fromRow == toRow)
            {
                int step = (toCol > fromCol) ? 1 : -1;
                for (int col = fromCol + step; col != toCol; col += step)
                {
                    if (map[fromRow, col] != 0) return false;
                }
            }

            else if (fromCol == toCol)
            {
                int step = (toRow > fromRow) ? 1 : -1;
                for (int row = fromRow + step; row != toRow; row += step)
                {
                    if (map[row, fromCol] != 0) return false;
                }
            }

            else if (Math.Abs(fromRow - toRow) == Math.Abs(fromCol - toCol))
            {
                int rowStep = (toRow > fromRow) ? 1 : -1;
                int colStep = (toCol > fromCol) ? 1 : -1;
                for (int r = fromRow + rowStep, c = fromCol + colStep; r != toRow; r += rowStep, c += colStep)
                {
                    if (map[r, c] != 0) return false;
                }
            }

            return true;
        }

        void SyncUi()
        {
            int[,] oldMap = new int[8,8];

            for (int i = 0; i < panel.Controls.Count; i++)
            {
                Panel p = panel.Controls[i] as Panel;
                PictureBox pic = null;
                
                if (p.Controls.Count == 0){
                    oldMap[i / 8, i % 8] = 0;
                    continue;
                }

                pic = p.Controls[0] as PictureBox;

                oldMap[i / 8, i % 8] = getChessmanType(pic.Tag.ToString());
            }
            
            for (int i = 0; i < panel.Controls.Count; i++)
            {
                Panel p = panel.Controls[i] as Panel;

                if (map[i / 8, i % 8] == 0 && oldMap[i / 8, i % 8] == 0) continue;

                if(map[i / 8, i % 8] != oldMap[i / 8, i % 8]) {
                    foreach(object item in panel.Controls) {
                        Panel p2 = item as Panel;

                        if (p2.Name == ConvertToChessNotation(i / 8, i % 8))
                        {
                            p2.Controls.Clear();

                            if (map[i / 8, i % 8] != 0)
                            {
                                PictureBox k = chessman(ChessmansWhite[map[i / 8, i % 8]], getChessmanName(map[i / 8, i % 8]));
                                p2.Controls.Add(k);
                            }
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

            //Pawn
            map[1, 0] = -1;
            map[1, 1] = -1;
            map[1, 2] = -1;
            map[1, 3] = -1;
            map[1, 4] = -1;
            map[1, 5] = -1;
            map[1, 6] = -1;
            map[1, 7] = -1;
            //Ot1her
            map[0, 0] = -2;
            map[0, 1] = -3;
            map[0, 2] = -4;
            map[0, 3] = -5;
            map[0, 4] = -6;
            map[0, 5] = -4;
            map[0, 6] = -3;
            map[0, 7] = -2;


            //Initialize
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

        public Board()
        {
            this.BackColor = Color.FromArgb(38, 35, 34);
            InitializeComponent();
            InitializeBoard();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            panel.Location = new Point((this.ClientSize.Width / 2) - (panel.Width / 2), 50);
            InitializeKeys();
        }

        void panel_click(object sender, EventArgs e)
        {
            Panel p = sender as Panel;

            if (selectedChessman == null)
                return;

            int[] chessmanPos = ConvertFromChessNotation((selectedChessman.Parent as Panel).Name);
            int[] currentChessmanPos = ConvertFromChessNotation(p.Name);

            if (!ValidateMove(chessmanPos, currentChessmanPos, selectedChessman.Tag.ToString()))
            {
                MessageBox.Show("Wrong Move!", "Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                selectedChessman = null;
                foreach (object item in panel.Controls)
                    (item as Panel).Enabled = true;
                return;
            }

            map[currentChessmanPos[0], currentChessmanPos[1]] = map[chessmanPos[0], chessmanPos[1]];
            map[chessmanPos[0], chessmanPos[1]] = 0;

            selectedChessman = null;
            SyncUi();
            
            foreach (object item in panel.Controls)
                (item as Panel).Enabled = true;
        }

        void chessman_click(object sender, EventArgs e)
        {
            if (selectedChessman != null)
            {
                int[] chessmanPos = ConvertFromChessNotation((selectedChessman.Parent as Panel).Name);
                int[] currentChessmanPos = ConvertFromChessNotation(((sender as PictureBox).Parent as Panel).Name);

                if (selectedChessman == (sender as PictureBox)) return;

                if (!ValidateMove(chessmanPos, currentChessmanPos, selectedChessman.Tag.ToString()))
                {
                    MessageBox.Show("Wrong Move!", "Incorrect", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    selectedChessman = null;
                    foreach (object item in panel.Controls)
                        (item as Panel).Enabled = true;
                    return;
                }

                map[currentChessmanPos[0], currentChessmanPos[1]] = map[chessmanPos[0], chessmanPos[1]];
                map[chessmanPos[0], chessmanPos[1]] = 0;

                selectedChessman = null;
                SyncUi();

                return;
            }

            PictureBox c = sender as PictureBox;
            selectedChessman = c;

            if (c.Tag.ToString().Split('@')[1] == "w")
            {
                if (c.Tag.ToString().Split('@')[0] == "pawn")
                {
                    int[] position = ConvertFromChessNotation((c.Parent as Panel).Name);
                }
            }
        }

        private void Board_FormClosing(object sender, FormClosingEventArgs e)
        {
            Start start = this.Tag as Start;
            start.Visible = true;
        }
    }
}