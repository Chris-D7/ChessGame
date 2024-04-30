using ChessGame.Logic.General;
using System;

namespace ChessGame.Logic.Pieces
{
    public class Rook : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }

        public Rook(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            if (Color == Player.White)
            {
                this.Image = Properties.Resources.WhiteRook;
            }
            else
            {
                this.Image = Properties.Resources.BlackRook;
            }
        }

        public override void PrintMove()
        {
            throw new NotImplementedException();
        }

        public override void PrintAttack()
        {
            throw new NotImplementedException();
        }

        public override void ClickOn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
