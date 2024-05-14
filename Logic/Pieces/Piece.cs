using ChessGame.Logic.General;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ChessGame.Logic.Pieces
{

    public enum PieceType
    {
        Pawn,
        Rook,
        Knight,
        Bishop,
        King,
        Queen
    }

    public abstract class Piece : PictureBox
    {
        public abstract Position Position { get; set; }
        public abstract Player Color { get; }
        public abstract PieceType Type { get; }
        public bool Moved { get; set; } = false;
        public bool Attack { get; set; } = false;
        protected List<Square> pathToKing = new List<Square>();
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

        public abstract void PrintMove(bool changeHandles = true);

        //public virtual void PrintAttack(bool changeHandles)
        //{
        //    Console.WriteLine("########## Piece PrintAttack ##########");
        //}

        public virtual void ClickOn(object sender, EventArgs e)
        {
            if (board.player == Color && !board.GetPawnPromotionHappening())
            {
                Piece piece = (Piece)sender;
                piece.board.BoardDrawing();
                board.DetermineLegalMoves(piece);
                Position position = piece.Position;
                piece.board.SetActivePosition(position);
                Square square = piece.board.GetSquare(position);
                square.BackColor = Board.SELECTED_COLOR;
                piece.PrintMove();
            }
        }

        public List<Square> GetPathToKing()
        {
            return pathToKing;
        }
    }
}
