﻿using ChessGame.Logic.General;

namespace ChessGame.Logic.Pieces
{
    public class Rook : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        public override PieceType Type { get; }
        private readonly Direction[] directions = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };

        public Rook(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            Type = PieceType.Rook;
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

        public override void PrintMove(bool changeHandles)
        {
            pathToKing.Clear();
            int scalar = 0;
            int index = 0;
            bool kingFound = false;
            while (index < 4)
            {
                scalar++;
                Position position = this.Position + (scalar * directions[index]);
                Square square = board.GetSquare(position);
                if (square != null)
                {
                    if (!kingFound)
                    {
                        pathToKing.Add(square);
                    }
                    if (square.Controls.Count > 0)
                    {
                        Piece attack = board.GetPiece(square.Position);
                        if (attack.Color != this.Color && square.Legal)
                        {
                            if (attack is King)
                            {
                                kingFound = true;
                            }
                            square.BackColor = Board.ATTACK_COLOR;
                            if (changeHandles)
                            {
                                attack.Attack = true;
                                attack.Click -= attack.ClickOn;
                                attack.Click += attack.AttackClick;
                            }
                        }
                        index++;
                        scalar = 0;
                        if (!kingFound)
                        {
                            pathToKing.Clear();
                        }
                    }
                    else if (square.Legal)
                    {
                        square.BackColor = ((position.Row + position.Column) % 2 == 0) ? Board.MOVE_CONTRAST_COLOR : Board.MOVE_BACKGROUND_COLOR;
                        if (changeHandles)
                        {
                            board.SetSquareHandleClick(square, SquareHandle.Move);
                        }
                    }
                }
                else
                {
                    index++;
                    scalar = 0;
                    if (!kingFound)
                    {
                        pathToKing.Clear();
                    }
                }
            }
        }
    }
}
