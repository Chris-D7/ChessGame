using ChessGame.Logic.General;
using System;

namespace ChessGame.Logic.Pieces
{
    public class King : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }

        public King(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            if (Color == Player.White)
            {
                this.Image = Properties.Resources.WhiteKing;
            }
            else
            {
                this.Image = Properties.Resources.BlackKing;
            }
        }

        //public override void AttackClick(object sender, EventArgs e)
        //{
        //    throw new NotImplementedException();
        //}

        public override void ClickOn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void PrintMove()
        {
            throw new NotImplementedException();
        }

        public override void PrintAttack()
        {
            throw new NotImplementedException();
        }
    }
}
