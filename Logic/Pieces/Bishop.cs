using ChessGame.Logic.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Logic.Pieces
{
    public class Bishop : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }

        public Bishop(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            if (Color == Player.White)
            {
                this.Image = Properties.Resources.WhiteBishop;
            }
            else
            {
                this.Image = Properties.Resources.BlackBishop;
            }
        }
    }
}
