using ChessGame.Logic.General;
using System;
using System.Collections.Generic;

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
            this.Click += this.ClickOn;
        }

        public override void PrintMove(bool changeHandles)
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
                            if (changeHandles)
                            {
                                attack.Attack = true;
                                attack.Click -= attack.ClickOn;
                                attack.Click += attack.AttackClick;
                            }
                        }
                    }
                    else
                    {
                        square.BackColor = ((position.Row + position.Column) % 2 == 0) ? Board.MOVE_CONTRAST_COLOR : Board.MOVE_BACKGROUND_COLOR;
                        if (changeHandles)
                        {
                            board.SetSquareHandleClick(square, SquareHandle.Move);
                        }
                    }

                }
                index++;
            }
        }

        public void PrintCastling()
        {
            if (!this.Moved)
            {
                List<Rook> rooks = board.CastlingGetRooks();
                List<Square> freeSquares = board.Check();
                if (freeSquares.Contains(board.GetSquare(this.Position)))
                {
                    if (rooks.Count == 1)
                    {
                        if (rooks[0].Position.Row == 0)
                        {
                            DetermineCastlingSquares(freeSquares, Direction.Left);
                        }
                        else
                        {
                            DetermineCastlingSquares(freeSquares, Direction.Right);
                        }
                    }
                    else if (rooks.Count == 2)
                    {
                        DetermineCastlingSquares(freeSquares, Direction.Left);
                        DetermineCastlingSquares(freeSquares, Direction.Right);
                    }
                }
            }
        }

        private void DetermineCastlingSquares(List<Square> freeSquares, Direction direction)
        {
            Square square1 = board.GetSquare(this.Position + (2 * direction));
            Square square2 = board.GetSquare(this.Position + direction);
            if(direction == Direction.Left)
            {
                Piece square3Piece = board.GetPiece(this.Position + 3 * direction);
                if (freeSquares.Contains(square1) && freeSquares.Contains(square2) && square3Piece == null)
                {
                    square1.BackColor = Board.CASTLING_COLOR;
                    board.SetSquareHandleClick(square1, SquareHandle.Castling);
                }
            }
            else
            {
                if (freeSquares.Contains(square1) && freeSquares.Contains(square2))
                {
                    square1.BackColor = Board.CASTLING_COLOR;
                    board.SetSquareHandleClick(square1, SquareHandle.Castling);
                }
            }

        }

        public override void ClickOn(object sender, EventArgs e)
        {
            if (board.player == Color)
            {
                board.BoardDrawing();
                PrintCastling();
                Piece piece = (Piece)sender;
                Position position = piece.Position;
                piece.board.SetActivePosition(position);
                Square square = piece.board.GetSquare(position);
                square.BackColor = Board.SELECTED_COLOR;
                piece.PrintMove();
            }
        }
    }
}
