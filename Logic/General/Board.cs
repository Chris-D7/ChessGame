using ChessGame.Logic.Pieces;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Remoting.Messaging;
using System.Windows.Forms;

namespace ChessGame.Logic.General
{
    public class Board : Panel
    {
        public static readonly Color CONTRAST_COLOR = Color.SaddleBrown;
        public static readonly Color BACKGROUND_COLOR = Color.LightYellow;
        public static readonly Color ATTACK_COLOR = Color.Red;
        public static readonly Color MOVE_COLOR = Color.LightGreen;
        public static readonly Color SELECTED_COLOR = Color.Green;
        public List<Piece> pieces = new List<Piece>();
        public List<Square> squares {  get; set; }
        public Board()
        {
            squares = new List<Square>();
            this.Size = new Size(640, 640);
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square square = new Square(new Position(i, j));
                    square.Size = new Size(80, 80);
                    square.Location = new Point(80 * i, 80 * j);
                    square.Click += SquareClick;
                    if ((i + j) % 2 == 0)
                    {
                        square.BackColor = BACKGROUND_COLOR;
                    }
                    else
                    {
                        square.BackColor = CONTRAST_COLOR;
                    }
                    this.Controls.Add(square);
                    squares.Add(square);
                }
            }
            AddPieces();
            Initialize();
        }

        private void AddPieces()
        {
            Player color = Player.Black;
            int row = 0;
            pieces.Add(new Rook(color, new Position(0, row), this));
            pieces.Add(new Knight(color, new Position(1, row), this));
            pieces.Add(new Bishop(color, new Position(2, row), this));
            pieces.Add(new Queen(color, new Position(3, row), this));
            pieces.Add(new King(color, new Position(4, row), this));
            pieces.Add(new Bishop(color, new Position(5, row), this));
            pieces.Add(new Knight(color, new Position(6, row), this));
            pieces.Add(new Rook(color, new Position(7, row), this));
            row++;
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Pawn(color, new Position(i, row), this));
            }
            color = Player.White;
            row = 7;
            pieces.Add(new Rook(color, new Position(0, row), this));
            pieces.Add(new Knight(color, new Position(1, row), this));
            pieces.Add(new Bishop(color, new Position(2, row), this));
            pieces.Add(new Queen(color, new Position(3, row), this));
            pieces.Add(new King(color, new Position(4, row), this));
            pieces.Add(new Bishop(color, new Position(5, row), this));
            pieces.Add(new Knight(color, new Position(6, row), this));
            pieces.Add(new Rook(color, new Position(7, row), this));
            row--;
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Pawn(color, new Position(i, row), this));
            }
        }

        private void Initialize()
        {
            foreach (Piece piece in pieces)
            {
                foreach (Square square in squares)
                {
                    if (square.Position == piece.Position)
                    {
                        square.Controls.Add(piece);
                    }
                }
            }
        }

        public void BoardDrawing()
        {
            foreach (Piece piece in pieces)
            {
                piece.Active = false;
            }
            foreach (Square square in squares)
            {
                if (square.BackColor == Color.LightGreen)
                {
                    square.Click -= GreenSquareClick;
                    square.Click += SquareClick;
                }
                if ((square.Position.Row + square.Position.Column) % 2 == 0)
                {
                    square.BackColor = BACKGROUND_COLOR;
                }
                else
                {
                    square.BackColor = CONTRAST_COLOR;
                }
            }
        }

        private void SquareClick(object sender, System.EventArgs e)
        {
            BoardDrawing();
        }

        private void GreenSquareClick(object sender, System.EventArgs e)
        {
            Piece selectedPiece = null;
            foreach (Piece p in pieces)
            {
                if (p.Active == true)
                {
                    selectedPiece = p;
                    break;
                }
            }
            if (selectedPiece != null)
            {
                Square originalSquare = getSquare(selectedPiece.Position);
                originalSquare.Controls.Remove(selectedPiece);
                Square targetSquare = (Square)sender;
                selectedPiece.Position = targetSquare.Position;
                targetSquare.Controls.Add(selectedPiece);
                selectedPiece.Active = false;
                selectedPiece.Moved = true;
            }
            BoardDrawing();
        }

        public void SetGreenSquareClick(Square square)
        {
            square.Click -= SquareClick;
            square.Click += GreenSquareClick;
        }

        public Square getSquare(Position position)
        {
            foreach(Square square in squares)
            {
                if(square.Position == position)
                {
                    return square;
                }
            }
            return null;
        }
    }
}
