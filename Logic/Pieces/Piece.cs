using ChessGame.Logic.General;
using System;
using System.Windows.Forms;

namespace ChessGame.Logic.Pieces
{

    public abstract class Piece : PictureBox
    {
        public abstract Position Position { get; set; }
        public abstract Player Color { get; }
        public bool Moved { get; set; } = false;
        public bool Attack { get; set; } = false;
        public Board board;

        public Piece(Board board)
        {
            Size = new System.Drawing.Size(80, 80);
            Location = new System.Drawing.Point(0, 0);
            this.board = board;
        }

        public virtual void AttackClick(object sender, System.EventArgs e)
        {
            board.Attack(((Piece)sender).Position);
        }

        public abstract void PrintMove();

        public virtual void PrintAttack()
        {
            Console.WriteLine("########## Piece PrintAttack ##########");
        }

        public virtual void ClickOn(object sender, EventArgs e)
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
