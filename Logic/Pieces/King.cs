using ChessGame.Logic.General;
using System;
using System.Collections.Generic;

namespace ChessGame.Logic.Pieces
{
    public class King : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        public override PieceType Type { get; }
        private readonly Direction[] directions = {
            Direction.Up, Direction.Down, Direction.Left, Direction.Right,
            Direction.UpRight, Direction.UpLeft, Direction.DownRight, Direction.DownLeft
        };

        public King(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            Type = PieceType.King;
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
                    if (square.Legal)
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
                }
                index++;
            }
        }

        public void PrintCastling(bool changeHandles = true)
        {
            if (!this.Moved)
            {
                List<Rook> rooks = board.CastlingGetRooks();
                if (board.GetSquare(this.Position).Legal)
                {
                    if (rooks.Count == 1)
                    {
                        if (rooks[0].Position.Row == 0)
                        {
                            DetermineCastlingSquares(Direction.Left, changeHandles);
                        }
                        else
                        {
                            DetermineCastlingSquares(Direction.Right, changeHandles);
                        }
                    }
                    else if (rooks.Count == 2)
                    {
                        DetermineCastlingSquares(Direction.Left, changeHandles);
                        DetermineCastlingSquares(Direction.Right, changeHandles);
                    }
                }
            }
        }

        private void DetermineCastlingSquares(Direction direction, bool changeHandles)
        {
            Square square1 = board.GetSquare(this.Position + (2 * direction));
            Square square2 = board.GetSquare(this.Position + direction);
            if (direction == Direction.Left)
            {
                Piece square3Piece = board.GetPiece(this.Position + 3 * direction);
                if (square1.Legal && square2.Legal && square3Piece == null
                    && square1.Controls.Count == 0 && square2.Controls.Count == 0)
                {
                    square1.BackColor = Board.CASTLING_COLOR;
                    if (changeHandles)
                    {
                        board.SetSquareHandleClick(square1, SquareHandle.Castling);
                    }
                }
            }
            else
            {
                if (square1.Legal && square2.Legal
                    && square1.Controls.Count == 0 && square2.Controls.Count == 0)
                {
                    square1.BackColor = Board.CASTLING_COLOR;
                    if (changeHandles)
                    {
                        board.SetSquareHandleClick(square1, SquareHandle.Castling);
                    }
                }
            }
        }

        public override void ClickOn(object sender, EventArgs e)
        {
            if (board.player == Color && !board.GetPawnPromotionHappening())
            {
                Piece piece = (Piece)sender;
                board.BoardDrawing();
                board.DetermineLegalMovesForKing(piece);
                PrintCastling();
                Position position = piece.Position;
                piece.board.SetActivePosition(position);
                Square square = piece.board.GetSquare(position);
                square.BackColor = Board.SELECTED_COLOR;
                piece.PrintMove();
            }
        }
    }
}
