using ChessGame.Logic.General;
using System;

namespace ChessGame.Logic.Pieces
{
    public class Rook : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        private readonly Direction[] directions = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

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
            this.Click += ClickOn;
        }

        public override void PrintMove()
        {
            int scalar = 0;
            int index = 0;
            while(index<4)
            {
                scalar++;
                Square square = board.GetSquare(this.Position + (scalar * directions[index]));
                if (square != null)
                {
                    if (square.Controls.Count > 0)
                    {
                        Piece attack = board.GetPiece(square.Position);
                        if(attack.Color != this.Color)
                        {
                            square.BackColor = Board.ATTACK_COLOR;
                            attack.Attack = true;
                            attack.Click -= attack.ClickOn;
                            attack.Click += attack.AttackClick;
                        }
                        index++;
                        scalar = 0;
                    }
                    else
                    {
                        square.BackColor = Board.MOVE_COLOR;
                        board.SetGreenSquareClick(square);
                    }
                    
                }
                else
                {
                    index++;
                    scalar = 0;
                }
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
