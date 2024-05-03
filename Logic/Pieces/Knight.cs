using ChessGame.Logic.General;

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

        public override void PrintMove(bool changeHandles)
        {
            int scalar = 1;
            int index = 0;
            while (index < 8)
            {
                if (index == 4) { scalar = -scalar; }
                Position position = this.Position + (scalar * directions[index % 4]);
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
    }
}
