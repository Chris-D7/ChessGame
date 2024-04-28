using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame.Logic.General
{
    public class Board : Panel
    {
        Panel[,] squares;
        public Board()
        {
            squares = new Panel[8,8];
            this.Size = new Size(600, 600);
            for(int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    squares[i,j] = new Panel();
                    squares[i,j].Size = new Size(75, 75);
                    squares[i,j].Location = new Point(75*i, 75*j);
                    if ((i + j) % 2 == 0)
                    {
                        squares[i, j].BackColor = Color.LightYellow;
                    }
                    else
                    {
                        squares[i, j].BackColor = Color.SaddleBrown;
                        PictureBox piece = new PictureBox();
                        piece.Image = Properties.Resources.WhitePawn;
                        piece.Size = new Size(75, 75);
                        squares[i, j].Controls.Add(piece);
                    }
                    this.Controls.Add(squares[i, j]);
                }
            }
        }

        public void Initialize()
        {

        }
    }
}
