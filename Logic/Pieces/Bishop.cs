using ChessGame.Logic.General;
using System;

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

        public override void AttackClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ClickOn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void PrintMove()
        {
            throw new NotImplementedException();
        }
    }
}
