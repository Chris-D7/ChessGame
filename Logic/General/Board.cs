using ChessGame.Logic.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ChessGame.Logic.General
{
    public class Board : Panel
    {
        public static readonly Color CONTRAST_COLOR = Color.SaddleBrown;
        public static readonly Color BACKGROUND_COLOR = Color.LightYellow;
        public static readonly Color ATTACK_COLOR = Color.Red;
        public static readonly Color MOVE_CONTRAST_COLOR = Color.FromArgb(255, 150, 205, 100);
        public static readonly Color MOVE_BACKGROUND_COLOR = Color.FromArgb(255, 80, 155, 30);
        public static readonly Color PASSANT_COLOR = Color.Red;
        public static readonly Color SELECTED_COLOR = Color.Green;
        public List<Piece> pieces = new List<Piece>();
        public List<Piece> removedPieces = new List<Piece>();
        public List<Square> squares { get; set; }
        private Position activePosition;
        public Player player = Player.White;
        private PictureBox pictureBoxTurn;
        private Label labelTurn;
        public Board()
        {
            squares = new List<Square>();
            this.Size = new Size(730, 640);
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
            int col = 0;
            pieces.Add(new Rook(color, new Position(0, col), this));
            pieces.Add(new Knight(color, new Position(1, col), this));
            pieces.Add(new Bishop(color, new Position(2, col), this));
            pieces.Add(new Queen(color, new Position(3, col), this));
            pieces.Add(new King(color, new Position(4, col), this));
            pieces.Add(new Bishop(color, new Position(5, col), this));
            pieces.Add(new Knight(color, new Position(6, col), this));
            pieces.Add(new Rook(color, new Position(7, col), this));
            col++;
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Pawn(color, new Position(i, col), this));
            }
            color = Player.White;
            col = 7;
            pieces.Add(new Rook(color, new Position(0, col), this));
            pieces.Add(new Knight(color, new Position(1, col), this));
            pieces.Add(new Bishop(color, new Position(2, col), this));
            pieces.Add(new Queen(color, new Position(3, col), this));
            pieces.Add(new King(color, new Position(4, col), this));
            pieces.Add(new Bishop(color, new Position(5, col), this));
            pieces.Add(new Knight(color, new Position(6, col), this));
            pieces.Add(new Rook(color, new Position(7, col), this));
            col--;
            for (int i = 0; i < 8; i++)
            {
                pieces.Add(new Pawn(color, new Position(i, col), this));
            }
        }

        private void Initialize()
        {
            squares.ForEach(sq => { sq.Controls.Clear(); });
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
            this.labelTurn = new System.Windows.Forms.Label();
            this.pictureBoxTurn = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxTurn)).BeginInit();
            this.labelTurn.BackColor = System.Drawing.SystemColors.Control;
            this.labelTurn.Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelTurn.Location = new System.Drawing.Point(650, 100);
            this.labelTurn.Name = "labelTurn";
            this.labelTurn.Size = new System.Drawing.Size(80, 24);
            this.labelTurn.TabIndex = 0;
            this.labelTurn.Text = "White";
            this.labelTurn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.pictureBoxTurn.Image = global::ChessGame.Properties.Resources.WhitePawn;
            this.pictureBoxTurn.Location = new System.Drawing.Point(650, 15);
            this.pictureBoxTurn.Name = "pictureBoxTurn";
            this.pictureBoxTurn.Size = new System.Drawing.Size(80, 80);
            this.pictureBoxTurn.TabIndex = 1;
            this.pictureBoxTurn.TabStop = false;
            this.Controls.Add(this.pictureBoxTurn);
            this.Controls.Add(this.labelTurn);
        }

        public void BoardDrawing()
        {
            foreach (Piece piece in pieces)
            {
                if (piece.Attack)
                {
                    piece.Attack = false;
                    piece.Click -= piece.AttackClick;
                    piece.Click += piece.ClickOn;
                }
            }
            foreach (Square square in squares)
            {
                if (square.BackColor == MOVE_BACKGROUND_COLOR || square.BackColor == MOVE_CONTRAST_COLOR)
                {
                    RemoveSquareClick(square, GreenSquareClick);
                }
                if (square.BackColor == PASSANT_COLOR)
                {
                    RemoveSquareClick(square, PassantSquareClick);
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
            activePosition = null;
        }

        private void GreenSquareClick(object sender, System.EventArgs e)
        {
            Piece selectedPiece = GetPiece(activePosition);
            if (selectedPiece != null)
            {
                Square originalSquare = GetSquare(selectedPiece.Position);
                originalSquare.Controls.Remove(selectedPiece);
                Square targetSquare = (Square)sender;
                selectedPiece.Position = targetSquare.Position;
                if (Math.Abs(targetSquare.Position.Column - originalSquare.Position.Column) == 2 && selectedPiece is Pawn)
                {
                    ((Pawn)selectedPiece).SetFirstBurst(true);
                }
                targetSquare.Controls.Add(selectedPiece);
                activePosition = null;
                selectedPiece.Moved = true;
            }
            BoardDrawing();
            ChangeTurns();
        }

        private void PassantSquareClick(object sender, System.EventArgs e)
        {
            Square square = (Square)sender;
            Position moveTo = square.Position;
            Position attackPosition = moveTo;
            if (moveTo.Column == 2)
            {
                attackPosition += Direction.Down;
            }
            else if (moveTo.Column == 5)
            {
                attackPosition += Direction.Up;
            }
            Position activePosition = GetActivePosition();
            Square attackSquare = GetSquare(attackPosition);
            Square activeSquare = GetSquare(activePosition);
            Piece attackPiece = GetPiece(attackPosition);
            Piece activePiece = GetPiece(activePosition);
            attackSquare.Controls.Remove(attackPiece);
            pieces.Remove(attackPiece);
            removedPieces.Add(attackPiece);
            activeSquare.Controls.Remove(activePiece);
            activePiece.Position = moveTo;
            square.Controls.Add(activePiece);
            this.activePosition = null;
            BoardDrawing();
            ChangeTurns();
        }

        public void SetGreenSquareClick(Square square)
        {
            square.Click -= SquareClick;
            square.Click += GreenSquareClick;
        }

        public void SetPassantSquareClick(Square square)
        {
            square.Click -= SquareClick;
            square.Click += PassantSquareClick;
        }

        private void RemoveSquareClick(Square square, EventHandler function)
        {
            square.Click -= function;
            square.Click += SquareClick;
        }

        public Square GetSquare(Position position)
        {
            return squares.FirstOrDefault(square => square.Position == position);
        }

        public Piece GetPiece(Position position)
        {
            return pieces.FirstOrDefault(piece => piece.Position == position);
        }

        public List<Piece> GetAttackedPieces()
        {
            List<Piece> attacked = new List<Piece>();
            foreach (Piece piece in pieces)
            {
                if (piece.Attack)
                {
                    attacked.Add(piece);
                }
            }
            return attacked;
        }

        public void Attack(Position attackPosition)
        {
            Piece activePiece = GetPiece(activePosition);
            Piece attackedPiece = GetPiece(attackPosition);
            if (activePiece != null && attackedPiece != null)
            {
                Square activeSquare = GetSquare(activePiece.Position);
                Square attackedSquare = GetSquare(attackPosition);
                attackedPiece.Click -= attackedPiece.AttackClick;
                attackedPiece.Click += attackedPiece.ClickOn;
                activePiece.Position = attackedPiece.Position;
                pieces.Remove(attackedPiece);
                removedPieces.Add(attackedPiece);
                attackedSquare.Controls.Remove(attackedPiece);
                activeSquare.Controls.Remove(activePiece);
                attackedSquare.Controls.Add(activePiece);
                activePiece.Moved = true;
                activePosition = null;
            }
            BoardDrawing();
            ChangeTurns();
        }

        public void SetActivePosition(Position position)
        {
            activePosition = position;
        }

        public Position GetActivePosition()
        {
            return activePosition;
        }

        private void ChangeTurns()
        {
            switch (player = player.Opponent())
            {
                case Player.White:
                    labelTurn.Text = "White";
                    pictureBoxTurn.Image = Properties.Resources.WhitePawn;

                    break;
                case Player.Black:
                    labelTurn.Text = "Black";
                    pictureBoxTurn.Image = Properties.Resources.BlackPawn;
                    break;
                default:
                    break;
            }
            foreach (Piece piece in pieces)
            {
                if (piece is Pawn pawn && piece.Color == player)
                {
                    pawn.SetFirstBurst(false);
                }
            }
        }
    }
}
