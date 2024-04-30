using ChessGame.Logic.General;
using System;

namespace ChessGame.Logic.Pieces
{
    public class Knight : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        private readonly Direction[] directions = {
        new Direction(2, 1), //new Direction(2, -1),
        new Direction(-2, 1), //new Direction(-2, -1),
        new Direction(1, 2), //new Direction(1, -2),
        new Direction(-1, 2), //new Direction(-1, -2)
        };

        public Knight(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            if (Color == Player.White)
            {
                this.Image = Properties.Resources.WhiteHorse;
            }
            else
            {
                this.Image = Properties.Resources.BlackHorse;
            }
            this.Click += ClickOn;
        }

        public override void PrintMove()
        {
            int scalar = 1;
            int index = 0;
            while (index < 8)
            {
                if(index == 4) { scalar = -scalar; }
                Square square = board.GetSquare(this.Position + (scalar * directions[index%4]));
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
                        square.BackColor = Board.MOVE_COLOR;
                        board.SetGreenSquareClick(square);
                    }

                }
                index++;
            }
        }

        public override void ClickOn(object sender, EventArgs e)
        {
            Piece piece = (Piece)sender;
            piece.board.BoardDrawing();
            Position position = piece.Position;
            piece.board.SetActivePosition(position);
            Square square = piece.board.GetSquare(position);
            square.BackColor = Board.SELECTED_COLOR;
            piece.PrintMove();
        }
    }
}
