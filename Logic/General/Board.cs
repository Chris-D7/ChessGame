using ChessGame.Logic.Pieces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ChessGame.Logic.General
{
    public enum SquareHandle
    {
        Move,
        Passant,
        Castling,
        None
    }

    public class Board : Panel
    {
        #region Colors

        public static readonly Color CONTRAST_COLOR = Color.SaddleBrown;

        public static readonly Color BACKGROUND_COLOR = Color.LightYellow;

        public static readonly Color ATTACK_COLOR = Color.Red;

        public static readonly Color MOVE_CONTRAST_COLOR = Color.FromArgb(255, 150, 205, 100);

        public static readonly Color MOVE_BACKGROUND_COLOR = Color.FromArgb(255, 80, 155, 30);

        public static readonly Color PASSANT_COLOR = Color.HotPink;

        public static readonly Color CASTLING_COLOR = Color.SkyBlue;

        public static readonly Color SELECTED_COLOR = Color.Green;

        #endregion

        #region Variables

        private List<Piece> piecesList;

        private List<Piece> removedPiecesList;

        private List<Square> squaresList;

        private Position activePosition;

        public Player player = Player.White;

        private PictureBox pictureBoxTurn;

        private Label labelTurn;

        private Panel removedPiecesPanel;

        private bool pawnPromotionHappening;

        #endregion

        public Board()
        {
            pawnPromotionHappening = false;

            squaresList = new List<Square>();

            removedPiecesList = new List<Piece>();

            piecesList = new List<Piece>();

            this.Size = new Size(750, 640);

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Square square = new Square(new Position(i, j))
                    {
                        Size = new Size(80, 80),

                        Location = new Point(80 * i, 80 * j)
                    };
                    SetSquareHandleClick(square, SquareHandle.None, true);

                    if ((i + j) % 2 == 0)
                    {
                        square.BackColor = BACKGROUND_COLOR;
                    }
                    else
                    {
                        square.BackColor = CONTRAST_COLOR;
                    }
                    this.Controls.Add(square);

                    squaresList.Add(square);
                }
            }
            AddPieces();

            Initialize();
        }

        private void AddPieces()
        {
            Player color = Player.Black;

            int col = 0;

            piecesList.Add(new Rook(color, new Position(0, col), this));
            piecesList.Add(new Knight(color, new Position(1, col), this));
            piecesList.Add(new Bishop(color, new Position(2, col), this));
            piecesList.Add(new Queen(color, new Position(3, col), this));
            piecesList.Add(new King(color, new Position(4, col), this));
            piecesList.Add(new Bishop(color, new Position(5, col), this));
            piecesList.Add(new Knight(color, new Position(6, col), this));
            piecesList.Add(new Rook(color, new Position(7, col), this));

            col++;

            for (int i = 0; i < 8; i++)
            {
                piecesList.Add(new Pawn(color, new Position(i, col), this));
            }
            color = Player.White;

            col = 7;

            piecesList.Add(new Rook(color, new Position(0, col), this));
            piecesList.Add(new Knight(color, new Position(1, col), this));
            piecesList.Add(new Bishop(color, new Position(2, col), this));
            piecesList.Add(new Queen(color, new Position(3, col), this));
            piecesList.Add(new King(color, new Position(4, col), this));
            piecesList.Add(new Bishop(color, new Position(5, col), this));
            piecesList.Add(new Knight(color, new Position(6, col), this));
            piecesList.Add(new Rook(color, new Position(7, col), this));

            col--;

            for (int i = 0; i < 8; i++)
            {
                piecesList.Add(new Pawn(color, new Position(i, col), this));
            }
        }

        private void Initialize()
        {
            squaresList.ForEach(sq => { sq.Controls.Clear(); });
            foreach (Piece piece in piecesList)
            {
                foreach (Square square in squaresList)
                {
                    if (square.Position == piece.Position)
                    {
                        square.Controls.Add(piece);
                    }
                }
            }

            #region Controls
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
            removedPiecesPanel = new Panel
            {
                Size = new Size(100, 490),
                Location = new Point(650, 150),
                Visible = true,
                AutoScroll = true
            };
            this.Controls.Add(removedPiecesPanel);
            #endregion

        }

        public void BoardDrawing(bool changeHandles = true)
        {
            foreach (Piece piece in piecesList)
            {
                if (piece.Attack)
                {
                    piece.Attack = false;
                    piece.Click -= piece.AttackClick;
                    piece.Click += piece.ClickOn;
                }
            }
            foreach (Square square in squaresList)
            {
                if (changeHandles)
                {
                    RemoveSquareHandleClick(square);
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
            activePosition = null;
        }

        private void SquareClick(object sender, System.EventArgs e)
        {
            BoardDrawing();
        }

        private void MoveSquareClick(object sender, System.EventArgs e)
        {
            Piece selectedPiece = GetPiece(activePosition);

            if (selectedPiece != null)
            {
                Square originalSquare = GetSquare(selectedPiece.Position);

                originalSquare.Controls.Remove(selectedPiece);

                Square targetSquare = (Square)sender;

                selectedPiece.Position = targetSquare.Position;

                targetSquare.Controls.Add(selectedPiece);

                selectedPiece.Moved = true;

                if (selectedPiece is Pawn pawn)
                {
                    if (Math.Abs(targetSquare.Position.Column - originalSquare.Position.Column) == 2)
                    {
                        pawn.SetFirstBurst(true);
                    }

                    if (removedPiecesList.Count() > 0 && (pawn.Position.Column == 0 || pawn.Position.Column == 7))
                    {
                        PawnPromotion(pawn);
                    }
                }

                BoardDrawing();

                if (!pawnPromotionHappening)
                {
                    ChangeTurns();
                }
            }
        }

        private void PawnPromotion(Pawn pawn)
        {
            pawnPromotionHappening = true;

            List<Image> uniquePieceImages = removedPiecesList
                .Where(piece => piece.Color == player && !(piece is Pawn))
                .Select(piece => piece.Image)
                .Distinct()
                .ToList();

            int locationHeight = 0;

            foreach (Image image in uniquePieceImages)
            {
                PictureBox pieceCameo = new PictureBox
                {
                    Size = new Size(80, 80),
                    Image = image,
                    Location = new Point(0, locationHeight)
                };

                Piece pieceToReturn = removedPiecesList
                    .FirstOrDefault(piece => piece.Color == player && piece.Image == image);

                pieceCameo.Click += (sender, e) =>
                {
                    Position position = pawn.Position;
                    Square square = GetSquare(position);

                    square.Controls.Remove(pawn);
                    piecesList.Remove(pawn);
                    removedPiecesList.Add(pawn);

                    square.Controls.Add(pieceToReturn);
                    piecesList.Add(pieceToReturn);
                    removedPiecesList.Remove(pieceToReturn);

                    pieceToReturn.Position = position;
                    removedPiecesPanel.Controls.Clear();

                    pawnPromotionHappening = false;
                    ChangeTurns();
                };

                removedPiecesPanel.Controls.Add(pieceCameo);
                locationHeight += 80;
            }
        }

        public bool getPawnPromotionHappening()
        {
            return pawnPromotionHappening;
        }

        private void PassantSquareClick(object sender, System.EventArgs e)
        {
            Square targetSquare = (Square)sender;

            Position moveTo = targetSquare.Position;

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

            piecesList.Remove(attackPiece);

            removedPiecesList.Add(attackPiece);

            activeSquare.Controls.Remove(activePiece);

            activePiece.Position = moveTo;

            targetSquare.Controls.Add(activePiece);

            BoardDrawing();

            ChangeTurns();
        }

        public void SetSquareHandleClick(Square square, SquareHandle handle, bool firstAssignment = false)
        {
            if (!firstAssignment)
            {
                square.Click -= SquareClick;
            }

            switch (handle)
            {
                case SquareHandle.Move: square.Click += MoveSquareClick; break;

                case SquareHandle.Passant : square.Click += PassantSquareClick; break;

                case SquareHandle.Castling: square.Click += CastlingSquareClick; break;

                default: square.Click += SquareClick; break;
            }

            square.SquareHandle = handle;
        }

        private void RemoveSquareHandleClick(Square square)
        {
            switch (square.SquareHandle)
            {
                case SquareHandle.Move: square.Click -= MoveSquareClick; break;

                case SquareHandle.Passant: square.Click -= PassantSquareClick; break;

                case SquareHandle.Castling : square.Click -= CastlingSquareClick; break;

                default: square.Click -= SquareClick; break;
            }

            square.Click += SquareClick;

            square.SquareHandle = SquareHandle.None;
        }

        public Square GetSquare(Position position)
        {
            return squaresList.FirstOrDefault(square => square.Position == position);
        }

        public Piece GetPiece(Position position)
        {
            return piecesList.FirstOrDefault(piece => piece.Position == position);
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

                piecesList.Remove(attackedPiece);

                removedPiecesList.Add(attackedPiece);

                attackedSquare.Controls.Remove(attackedPiece);

                activeSquare.Controls.Remove(activePiece);

                attackedSquare.Controls.Add(activePiece);

                activePiece.Moved = true;
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
            foreach (Piece piece in piecesList)
            {
                if (piece is Pawn pawn && piece.Color == player)
                {
                    pawn.SetFirstBurst(false);
                }
            }
        }

        public List<Square> CastlingCheck()
        {
            List<Square> freeSquares = new List<Square>();

            foreach (Piece piece in piecesList)
            {
                if (piece.Color != player)
                {
                    piece.PrintMove(false);
                }
            }

            foreach(Square square in squaresList)
            {
                if ((square.BackColor == BACKGROUND_COLOR || square.BackColor == CONTRAST_COLOR) &&
                    (GetPiece(square.Position) == null || GetPiece(square.Position) is King))
                {
                    freeSquares.Add(square);
                }
            }

            BoardDrawing(false);

            return freeSquares;
        }

        public List<Rook> CastlingGetRooks()
        {
            List<Rook> rooks = new List<Rook>();

            foreach (Piece piece in piecesList)
            {
                if (piece is Rook rook && piece.Color == player && !piece.Moved)
                {
                    rooks.Add(rook);
                }
            }

            return rooks;
        }

        private void CastlingSquareClick(object sender, EventArgs e)
        {
            Square castlingSquare = (Square)sender;

            Square rookSquare = null, kingSquare = null, moveRookSquare = null;

            if (castlingSquare.Position.Row == 2)
            {
                rookSquare = GetSquare(castlingSquare.Position + (2 * Direction.Left));

                kingSquare = GetSquare(castlingSquare.Position + (2 * Direction.Right));

                moveRookSquare = GetSquare(castlingSquare.Position + Direction.Right);
            }

            else if (castlingSquare.Position.Row == 6)
            {
                rookSquare = GetSquare(castlingSquare.Position + Direction.Right);

                kingSquare = GetSquare(castlingSquare.Position + (2 * Direction.Left));

                moveRookSquare = GetSquare(castlingSquare.Position + Direction.Left);
            }

            Piece rook = GetPiece(rookSquare.Position);

            Piece king = GetPiece(kingSquare.Position);

            rookSquare.Controls.Remove(rook);

            moveRookSquare.Controls.Add(rook);

            kingSquare.Controls.Remove(king);

            castlingSquare.Controls.Add(king);

            king.Position = castlingSquare.Position;

            rook.Position = moveRookSquare.Position;

            king.Moved = true;

            rook.Moved = true;

            BoardDrawing();

            ChangeTurns();
        }
    }
}