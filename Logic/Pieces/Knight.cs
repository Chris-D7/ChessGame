using ChessGame.Logic.General;

namespace ChessGame.Logic.Pieces
{
    public class Knight : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }
        public override PieceType Type { get; }
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
            Type = PieceType.Knight;
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
            pathToKing.Clear();
            int scalar = 1;
            int index = 0;
            bool kingFound = false;
            while (index < 8)
            {
                if (index == 4) { scalar = -scalar; }
                Position position = this.Position + (scalar * directions[index % 4]);
                Square square = board.GetSquare(position);
                if (square != null)
                {
                    if (square.Legal)
                    {
                        if (!kingFound)
                        {
                            pathToKing.Add(square);
                        }
                        if (square.Controls.Count > 0)
                        {
                            Piece attack = board.GetPiece(square.Position);
                            if (attack.Color != this.Color)
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
                        }
                        else
                        {
                            square.BackColor = ((position.Row + position.Column) % 2 == 0) ? Board.MOVE_CONTRAST_COLOR : Board.MOVE_BACKGROUND_COLOR;
                            if (changeHandles)
                            {
                                board.SetSquareHandleClick(square, SquareHandle.Move);
                            }
                            if (!kingFound)
                            {
                                pathToKing.Add(square);
                            }
                        }
                    }
                }
                index++;
                if (!kingFound)
                {
                    pathToKing.Clear();
                }
            }
        }
    }
}
