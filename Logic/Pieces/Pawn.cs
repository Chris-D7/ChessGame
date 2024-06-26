﻿using ChessGame.Logic.General;

namespace ChessGame.Logic.Pieces
{
    public class Pawn : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        public override PieceType Type { get; }
        private readonly Direction face;
        private bool firstBurst = false;

        public Pawn(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            Type = PieceType.Pawn;
            if (Color == Player.White)
            {
                face = Direction.Up;
                this.Image = Properties.Resources.WhitePawn;
            }
            else
            {
                face = Direction.Down;
                this.Image = Properties.Resources.BlackPawn;
            }
            this.Click += ClickOn;
        }

        public override void PrintMove(bool changeHandles)
        {
            int i = Moved ? 2 : 3;
            for (int k = 1; k < i; k++)
            {
                Position position = this.Position + (k * face);
                Square square = board.GetSquare(position);
                if (square != null && square.Legal)
                {
                    if (square.Controls.Count > 0)
                    {
                        break;
                    }
                    square.BackColor = ((position.Row + position.Column) % 2 == 0) ? Board.MOVE_CONTRAST_COLOR : Board.MOVE_BACKGROUND_COLOR;
                    if (changeHandles)
                    {
                        board.SetSquareHandleClick(square, SquareHandle.Move);
                    }
                }
            }
            PrintAttack(changeHandles);
        }

        public void PrintAttack(bool changeHandles)
        {
            Direction directionRight, directionLeft;
            if (face == Direction.Up)
            {
                directionRight = Direction.UpRight;
                directionLeft = Direction.UpLeft;
            }
            else
            {
                directionRight = Direction.DownRight;
                directionLeft = Direction.DownLeft;
            }
            Position positionRight = this.Position + directionRight;
            Position positionLeft = this.Position + directionLeft;
            Square squareRight = board.GetSquare(positionRight);
            Square squareLeft = board.GetSquare(positionLeft);

            if (changeHandles)
            {
                if (squareLeft != null && squareLeft.Controls.Count > 0 && squareLeft.Legal)
                {
                    Piece attack = board.GetPiece(positionLeft);
                    if (attack != null && attack.Color != this.Color)
                    {
                        squareLeft.BackColor = Board.ATTACK_COLOR;
                        attack.Attack = true;
                        attack.Click -= attack.ClickOn;
                        attack.Click += attack.AttackClick;
                    }
                }
                if (squareRight != null && squareRight.Controls.Count > 0 && squareRight.Legal)
                {
                    Piece attack = board.GetPiece(positionRight);
                    if (attack != null && attack.Color != this.Color)
                    {
                        squareRight.BackColor = Board.ATTACK_COLOR;
                        attack.Attack = true;
                        attack.Click -= attack.ClickOn;
                        attack.Click += attack.AttackClick;
                    }
                }
            }
            else
            {
                if (squareLeft != null && squareLeft.Legal)
                {
                    squareLeft.BackColor = Board.ATTACK_COLOR;
                }
                if (squareRight != null && squareRight.Legal)
                {
                    squareRight.BackColor = Board.ATTACK_COLOR;
                }
            }
            if (Moved)
            {
                Position right = this.Position + Direction.Right;
                Position left = this.Position + Direction.Left;
                Piece leftPiece = board.GetPiece(left);
                Piece rightPiece = board.GetPiece(right);
                if (leftPiece is Pawn leftAttackPawn && leftPiece.Color != this.Color)
                {
                    if (leftAttackPawn.GetFirstBurst() && leftAttackPawn.GetFirstBurst())
                    {
                        if (face == Direction.Up && left.Column == 3)
                        {
                            PrintPassant(left, changeHandles);
                        }
                        if (face == Direction.Down && left.Column == 4)
                        {
                            PrintPassant(left, changeHandles);
                        }
                    }
                }
                if (rightPiece is Pawn rightAttackPawn && rightPiece.Color != this.Color)
                {
                    if (rightAttackPawn.GetFirstBurst() && rightAttackPawn.GetFirstBurst())
                    {
                        if (face == Direction.Up && right.Column == 3)
                        {
                            PrintPassant(right, changeHandles);
                        }
                        if (face == Direction.Down && right.Column == 4)
                        {
                            PrintPassant(right, changeHandles);
                        }
                    }
                }
            }
        }

        private void PrintPassant(Position position, bool changeHandles)
        {
            Position moveTo = position + face;
            Square moveToSquare = board.GetSquare(moveTo);
            if (moveToSquare != null && moveToSquare.Legal)
            {
                moveToSquare.BackColor = Board.PASSANT_COLOR;
                if (changeHandles)
                {
                    board.SetSquareHandleClick(moveToSquare, SquareHandle.Passant);
                }
            }
        }

        public void SetFirstBurst(bool value)
        {
            firstBurst = value;
        }

        public bool GetFirstBurst()
        {
            return firstBurst;
        }
    }
}