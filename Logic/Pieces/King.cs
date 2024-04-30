using ChessGame.Logic.General;
using System;

namespace ChessGame.Logic.Pieces
{
    public class King : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        private readonly Direction[] directions = {
            Direction.Up, Direction.Down, Direction.Left, Direction.Right,
            Direction.UpRight, Direction.UpLeft, Direction.DownRight, Direction.DownLeft
        };

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
            this.Click += ClickOn;
        }

        public override void PrintMove()
        {
            int index = 0;
            while (index < 8)
            {
                Position position = this.Position + (directions[index]);
                Square square = board.GetSquare(position);
                if (square != null)
                {
                    if (square.Controls.Count > 0)
                    {
                        Piece attack = board.GetPiece(square.Position);
                        if (attack.Color != this.Color)
                        {
                            square.BackColor = Board.ATTACK_COLOR;
                            attack.Attack = true;
                            attack.Click -= attack.ClickOn;
                            attack.Click += attack.AttackClick;
                        }
                    }
                    else
                    {
                        square.BackColor = ((position.Row + position.Column) % 2 == 0) ? Board.MOVE_CONTRAST_COLOR : Board.MOVE_BACKGROUND_COLOR;
                        board.SetGreenSquareClick(square);
                    }

                }
                index++;
            }
        }

    }
}
