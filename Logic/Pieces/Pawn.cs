using ChessGame.Logic.General;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

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

        public void ClickOn(object sender, System.EventArgs e)
        {
            board.BoardDrawing();
            int i;
            if (Moved) { i = 2; }
            else { i = 3; }
            foreach (Square square in board.squares)
            {
                if (square.Position == Position)
                {
                    square.BackColor = Board.SELECTED_COLOR;
                }
            }
            if (face == Direction.Up)
            {
                Square square;
                for (int k = 1; k < i; k++)
                {
                    square = board.getSquare(this.Position + new Direction(0, -k));
                    if (square.Controls.Count > 0)
                    {
                        break;
                    }
                    if (square != null)
                    {
                        square.BackColor = Board.MOVE_COLOR;
                        board.SetGreenSquareClick(square);
                    }
                }
                square = board.getSquare(this.Position + new Direction(1, -1));
                if (square != null)
                {
                    if (square.Controls.Count > 0)
                    {
                        if (((Piece)square.Controls[0]).Color == Player.Black)
                        {
                            square.BackColor = Board.ATTACK_COLOR;
                        }
                    }
                }
                square = board.getSquare(this.Position + new Direction(-1, -1));
                if(square != null)
                {
                    if (square.Controls.Count > 0)
                    {
                        if (((Piece)square.Controls[0]).Color == Player.Black)
                        {
                            square.BackColor = Board.ATTACK_COLOR;
                        }
                    }
                }
            }
            if (face == Direction.Down)
            {
                Square square;
                for (int k = 1; k < i; k++)
                {
                    square = board.getSquare(this.Position + new Direction(0, k));
                    if (square.Controls.Count > 0)
                    {
                        break;
                    }
                    if (square != null)
                    {
                        square.BackColor = Board.MOVE_COLOR;
                        board.SetGreenSquareClick(square);
                    }
                }
                square = board.getSquare(this.Position + new Direction(1, 1));
                if(square != null)
                {
                    if (square.Controls.Count > 0)
                    {
                        if (((Piece)square.Controls[0]).Color == Player.White)
                        {
                            square.BackColor = Board.ATTACK_COLOR;
                        }
                    }
                }
                square = board.getSquare(this.Position + new Direction(-1, 1));
                if (square != null)
                {
                    if (square.Controls.Count > 0)
                    {
                        if (((Piece)square.Controls[0]).Color == Player.White)
                        {
                            square.BackColor = Board.ATTACK_COLOR;
                        }
                    }
                }
            }
            Active = true;
        }
    }
}
