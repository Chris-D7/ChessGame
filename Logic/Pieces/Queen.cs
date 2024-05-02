using ChessGame.Logic.General;

namespace ChessGame.Logic.Pieces
{
    public class Queen : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        private readonly Direction[] directions = {
            Direction.Up, Direction.Down, Direction.Left, Direction.Right,
            Direction.UpRight, Direction.UpLeft, Direction.DownRight, Direction.DownLeft
        };

        public Queen(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            if (Color == Player.White)
            {
                this.Image = Properties.Resources.WhiteQueen;
            }
            else
            {
                this.Image = Properties.Resources.BlackQueen;
            }
            this.Click += ClickOn;
        }

        public override void PrintMove(bool changeHandles)
        {
            int scalar = 0;
            int index = 0;
            while (index < 8)
            {
                scalar++;
                Position position = this.Position + (scalar * directions[index]);
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
                        index++;
                        scalar = 0;
                    }
                    else
                    {
                        square.BackColor = ((position.Row + position.Column) % 2 == 0) ? Board.MOVE_CONTRAST_COLOR : Board.MOVE_BACKGROUND_COLOR;
                        if (changeHandles)
                        {
                            board.SetGreenSquareClick(square);
                        }
                    }

                }
                else
                {
                    index++;
                    scalar = 0;
                }
            }
        }
    }
}
