using ChessGame.Logic.General;

namespace ChessGame.Logic.Pieces
{
    public class Pawn : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        private readonly Direction face;

        public Pawn(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
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

        public override void ClickOn(object sender, System.EventArgs e)
        {
            Piece piece = (Piece)sender;
            piece.board.BoardDrawing();
            Position position = piece.Position;
            piece.board.SetActivePosition(position);
            Square square = piece.board.GetSquare(position);
            square.BackColor = Board.SELECTED_COLOR;
            piece.PrintMove();
            piece.PrintAttack();
        }

        public override void PrintMove()
        {
            int i = Moved ? 2 : 3;
            for (int k = 1; k < i; k++)
            {
                Position position = this.Position + (k * face);
                Square square = board.GetSquare(position);
                if (square != null)
                {
                    if (square.Controls.Count > 0)
                    {
                        break;
                    }
                    square.BackColor = ((position.Row + position.Column) % 2 == 0) ? Board.MOVE_CONTRAST_COLOR : Board.MOVE_BACKGROUND_COLOR;
                    board.SetGreenSquareClick(square);
                }
            }
        }

        public override void PrintAttack()
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

            if (squareLeft != null && squareLeft.Controls.Count > 0)
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
            if (squareRight != null && squareRight.Controls.Count > 0)
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
    }
}