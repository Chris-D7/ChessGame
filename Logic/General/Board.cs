using ChessGame.Logic.Pieces;
using System;
using System.Collections.Generic;
using System.Data;
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
        private int movesSincePawnMoveOrCapture;

        #endregion

        public Board()
        {
            pawnPromotionHappening = false;
            squaresList = new List<Square>();
            removedPiecesList = new List<Piece>();
            piecesList = new List<Piece>();
            movesSincePawnMoveOrCapture = 0;
            this.Size = new Size(750, 640);
            AddSquares();
            AddPieces();
            Initialize();
        }

        #region Maintenance

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
                square.BackColor = (square.Position.Row + square.Position.Column) % 2 == 0 ? BACKGROUND_COLOR : CONTRAST_COLOR;
            }
            activePosition = null;
            SetSquaresLegal(true);
        }

        public Square GetSquare(Position position)
        {
            return squaresList.FirstOrDefault(square => square.Position == position);
        }

        public Piece GetPiece(Position position)
        {
            return piecesList.FirstOrDefault(piece => piece.Position == position);
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
            player = player.Opponent();
            labelTurn.Text = player == Player.White ? "White" : "Black";
            pictureBoxTurn.Image = player == Player.White ? Properties.Resources.WhitePawn : Properties.Resources.BlackPawn;

            foreach (Piece piece in piecesList)
            {
                if (piece is Pawn pawn && pawn.Color == player)
                {
                    pawn.SetFirstBurst(false);
                }
            }

            CheckForEndgame();
        }

        private void ClearSquares()
        {
            foreach (Square square in squaresList)
            {
                square.Controls.Clear();
            }
        }

        private void SetSquaresLegal(bool squareLegal)
        {
            foreach (Square square in squaresList)
            {
                square.Legal = squareLegal;
            }
        }

        #endregion

        #region Initializing

        private void PutPiecesOnBoard()
        {
            Dictionary<Position, Piece> piecePositions = new Dictionary<Position, Piece>();
            foreach (Piece piece in piecesList)
            {
                piecePositions[piece.Position] = piece;
            }
            foreach (Square square in squaresList)
            {
                if (piecePositions.ContainsKey(square.Position))
                {
                    Piece piece = piecePositions[square.Position];
                    square.Controls.Add(piece);
                }
            }
        }

        private void AddSquares()
        {
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
                    square.BackColor = (i + j) % 2 == 0 ? BACKGROUND_COLOR : CONTRAST_COLOR;
                    this.Controls.Add(square);
                    squaresList.Add(square);
                }
            }
        }

        private void AddPieces()
        {
            AddMajorPieces(Player.Black, 0);
            AddPawns(Player.Black, 1);
            AddMajorPieces(Player.White, 7);
            AddPawns(Player.White, 6);
        }

        private void AddMajorPieces(Player color, int col)
        {
            piecesList.Add(new Rook(color, new Position(0, col), this));
            piecesList.Add(new Knight(color, new Position(1, col), this));
            piecesList.Add(new Bishop(color, new Position(2, col), this));
            piecesList.Add(new Queen(color, new Position(3, col), this));
            piecesList.Add(new King(color, new Position(4, col), this));
            piecesList.Add(new Bishop(color, new Position(5, col), this));
            piecesList.Add(new Knight(color, new Position(6, col), this));
            piecesList.Add(new Rook(color, new Position(7, col), this));
        }

        private void AddPawns(Player color, int col)
        {
            for (int i = 0; i < 8; i++)
            {
                piecesList.Add(new Pawn(color, new Position(i, col), this));
            }
        }

        private void Initialize()
        {
            ClearSquares();
            PutPiecesOnBoard();
            InitializeControls();
        }

        private void InitializeControls()
        {
            InitializeLabelTurn();
            InitializePictureBoxTurn();
            InitializeRemovedPiecesPanel();
        }

        private void InitializeLabelTurn()
        {
            labelTurn = new Label
            {
                BackColor = System.Drawing.SystemColors.Control,
                Font = new System.Drawing.Font("Arial Rounded MT Bold", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0))),
                Location = new System.Drawing.Point(650, 100),
                Name = "labelTurn",
                Size = new System.Drawing.Size(80, 24),
                TabIndex = 0,
                Text = "White",
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            };
            Controls.Add(labelTurn);
        }

        private void InitializePictureBoxTurn()
        {
            pictureBoxTurn = new PictureBox
            {
                Image = global::ChessGame.Properties.Resources.WhitePawn,
                Location = new System.Drawing.Point(650, 15),
                Name = "pictureBoxTurn",
                Size = new System.Drawing.Size(80, 80),
                TabIndex = 1,
                TabStop = false
            };
            Controls.Add(pictureBoxTurn);
        }

        private void InitializeRemovedPiecesPanel()
        {
            removedPiecesPanel = new Panel
            {
                Size = new System.Drawing.Size(100, 490),
                Location = new System.Drawing.Point(650, 150),
                Visible = true,
                AutoScroll = true
            };
            Controls.Add(removedPiecesPanel);
        }

        #endregion

        #region Square Handles

        private void SquareClick(object sender, System.EventArgs e)
        {
            BoardDrawing();
        }

        private void MoveSquareClick(object sender, System.EventArgs e)
        {
            MovePiece((Square)sender);
        }

        private void PassantSquareClick(object sender, System.EventArgs e)
        {
            Passant((Square)sender);
        }

        private void CastlingSquareClick(object sender, EventArgs e)
        {
            Castle((Square)sender);
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
                case SquareHandle.Passant: square.Click += PassantSquareClick; break;
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
                case SquareHandle.Castling: square.Click -= CastlingSquareClick; break;
                default: square.Click -= SquareClick; break;
            }
            square.Click += SquareClick;
            square.SquareHandle = SquareHandle.None;
        }

        #endregion

        #region Piece Actions

        private void PawnPromotion(Pawn pawn)
        {
            pawnPromotionHappening = true;

            IEnumerable<Image> pieceImages = removedPiecesList
                .Where(piece => piece.Color == player && !(piece is Pawn))
                .Select(piece => piece.Image)
                .Distinct();

            Dictionary<Image, Piece> imageToPieceMap = removedPiecesList
                .Where(piece => piece.Color == player && !(piece is Pawn))
                .ToDictionary(piece => piece.Image);

            int locationHeight = 0;
            foreach (Image image in pieceImages)
            {
                PictureBox pieceCameo = new PictureBox
                {
                    Size = new Size(80, 80),
                    Image = image,
                    Location = new Point(0, locationHeight)
                };

                if (imageToPieceMap.TryGetValue(image, out Piece pieceToReturn))
                {
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
                        CheckForEndgame();
                    };
                }

                removedPiecesPanel.Controls.Add(pieceCameo);
                locationHeight += 80;
            }

            if (!pieceImages.Any())
            {
                pawnPromotionHappening = false;
            }
        }

        public bool GetPawnPromotionHappening()
        {
            return pawnPromotionHappening;
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

                attackedSquare.Controls.Remove(attackedPiece);
                piecesList.Remove(attackedPiece);
                removedPiecesList.Add(attackedPiece);

                MovePieceToPosition(activePiece, attackedSquare);

                if (activePiece is Pawn pawn && (removedPiecesList.Count > 0 && (pawn.Position.Column == 0 || pawn.Position.Column == 7)))
                {
                    PawnPromotion(pawn);
                }

                movesSincePawnMoveOrCapture = 0;
            }

            BoardDrawing();
            ChangeTurns();
        }

        public List<Rook> CastlingGetRooks()
        {
            return piecesList.OfType<Rook>()
                             .Where(rook => rook.Color == player && !rook.Moved)
                             .ToList();
        }

        private void Castle(Square castlingSquare)
        {
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
            MovePieceToPosition(rook, moveRookSquare);
            MovePieceToPosition(king, castlingSquare);
            BoardDrawing();
            ChangeTurns();
        }

        private void MovePieceToPosition(Piece piece, Square destinationSquare)
        {
            if (piece == null) return;
            Square sourceSquare = GetSquare(piece.Position);
            if (sourceSquare == null) return;
            if (destinationSquare == null) return;

            sourceSquare.Controls.Remove(piece);
            destinationSquare.Controls.Add(piece);
            piece.Position = destinationSquare.Position;
            piece.Moved = true;
        }

        private void MovePiece(Square targetSquare)
        {
            Piece selectedPiece = GetPiece(activePosition);
            if (selectedPiece != null)
            {
                movesSincePawnMoveOrCapture++;
                Square originalSquare = GetSquare(selectedPiece.Position);
                MovePieceToPosition(selectedPiece, targetSquare);

                if (selectedPiece is Pawn pawn)
                {
                    movesSincePawnMoveOrCapture = 0;
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

        private void Passant(Square targetSquare)
        {
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
            Piece activePiece = GetPiece(GetActivePosition());
            Square attackSquare = GetSquare(attackPosition);
            Piece attackPiece = GetPiece(attackPosition);

            attackSquare.Controls.Remove(attackPiece);
            piecesList.Remove(attackPiece);
            removedPiecesList.Add(attackPiece);

            MovePieceToPosition(activePiece, targetSquare);

            BoardDrawing();
            ChangeTurns();
        }

        #endregion

        #region Endgame Conditions

        private bool IsKingVsKing()
        {
            return piecesList.Count == 2;
        }

        private bool IsKingVsKingMinor()
        {
            int bishopCount = piecesList.Count(piece => piece is Bishop);
            int knightCount = piecesList.Count(piece => piece is Knight);

            return (bishopCount == 1 || knightCount == 1) && piecesList.Count == 3;
        }

        private bool IsKingBishopVsKingBishop()
        {
            var blackBishop = piecesList.OfType<Bishop>().FirstOrDefault(b => b.Color == Player.Black);
            var whiteBishop = piecesList.OfType<Bishop>().FirstOrDefault(b => b.Color == Player.White);

            return blackBishop != null && whiteBishop != null
                && ((blackBishop.Position.Row + blackBishop.Position.Column) % 2
                != (whiteBishop.Position.Row + whiteBishop.Position.Column) % 2)
                && piecesList.Count == 4;
        }

        private bool IsLoneKing()
        {
            int countKing = piecesList.Count(piece => piece is King && piece.Color == player);
            int countAll = piecesList.Count;

            return countKing == 1 && countAll == 17;
        }

        private bool FiftyMoveRule()
        {
            return movesSincePawnMoveOrCapture >= 50;
        }

        private void CheckForEndgame()
        {
            bool legalMoves = CheckAllForLegalMoves();
            bool isInCheck = IsInCheck(player);

            if (IsKingVsKing())
                GenerateEndScreen(Result.KingVsKing);
            else if (IsKingVsKingMinor())
                GenerateEndScreen(Result.KingVsKingMinor);
            else if (IsKingBishopVsKingBishop())
                GenerateEndScreen(Result.KingBishopVsKingBishop);
            else if (IsLoneKing())
                GenerateEndScreen(Result.LoneKing);
            else if (!legalMoves && !isInCheck)
                GenerateEndScreen(Result.StaleMate);
            else if (FiftyMoveRule())
                GenerateEndScreen(Result.FiftyMoveRule);
            else if (!legalMoves && isInCheck)
                GenerateEndScreen(player == Player.White ? Result.Black : Result.White);
        }

        private void GenerateEndScreen(Result result)
        {
            EndScreen endScreen = new EndScreen(result);
            endScreen.ShowDialog();
        }

        #endregion

        #region Move Legality

        private List<Piece> PiecesAttackingKing(Player kingColor)
        {
            List<Piece> attackers = new List<Piece>();
            Square kingSquare = GetSquare(piecesList.FirstOrDefault(piece => piece.Color == kingColor && piece is King).Position);

            foreach (Piece piece in piecesList)
            {
                if (piece.Color != kingColor)
                {
                    piece.PrintMove(false);
                    if (kingSquare.BackColor == ATTACK_COLOR)
                    {
                        attackers.Add(piece);
                    }
                    BoardDrawing(false);
                }
            }
            return attackers;
        }

        private bool IsInCheck(Player color)
        {
            return PiecesAttackingKing(color).Count > 0;
        }

        private bool MovesForInCheck(Piece piece)
        {
            List<Piece> attackers = PiecesAttackingKing(piece.Color);
            HashSet<Square> legalSquares = new HashSet<Square>();
            if (attackers.Count > 0)
            {
                foreach (Piece attacker in attackers)
                {
                    attacker.PrintMove(false);
                    List<Square> pathToKing = attacker.GetPathToKing();
                    foreach (Square square in pathToKing)
                    {
                        legalSquares.Add(square);
                    }
                    legalSquares.Add(GetSquare(attacker.Position));
                    BoardDrawing(false);
                }
                foreach (Square square in squaresList)
                {
                    square.Legal = legalSquares.Contains(square);
                }
                return true;
            }
            else
            {
                SetSquaresLegal(true);
            }
            return false;
        }

        private void MovesResultingInCheck(Piece piece)
        {
            if (!IsInCheck(piece.Color))
            {
                Square squarePiece = GetSquare(piece.Position);
                squarePiece.Controls.Remove(piece);
                List<Piece> attackers = PiecesAttackingKing(piece.Color);
                if (attackers.Count == 0)
                {
                    squarePiece.Controls.Add(piece);
                    SetSquaresLegal(true);
                    return;
                }
                HashSet<Square> legalSquares = new HashSet<Square>();
                foreach (Piece attacker in attackers)
                {
                    attacker.PrintMove(false);
                    List<Square> pathToKing = attacker.GetPathToKing();
                    foreach (Square square in pathToKing)
                    {
                        legalSquares.Add(square);
                    }
                    legalSquares.Add(GetSquare(attacker.Position));
                    BoardDrawing(false);
                }
                squarePiece.Controls.Add(piece);
                foreach (Square square in squaresList)
                {
                    square.Legal = legalSquares.Contains(square);
                }
            }
            else
            {
                SetSquaresLegal(true);
            }
        }

        public void DetermineLegalMovesForKing(Piece king)
        {
            HashSet<Square> legalSquares = new HashSet<Square>();
            Square kingSquare = GetSquare(king.Position);
            kingSquare.Controls.Remove(king);
            foreach (Piece piece in piecesList)
            {
                if (piece.Color != king.Color)
                {
                    if (piece is Pawn pawn)
                    {
                        pawn.PrintAttack(false);
                    }
                    else
                    {
                        piece.PrintMove(false);
                    }
                    legalSquares.Add(GetSquare(piece.Position));
                }
            }
            foreach (Square square in squaresList)
            {
                if (square.BackColor == BACKGROUND_COLOR || square.BackColor == CONTRAST_COLOR)
                {
                    legalSquares.Add(square);
                }
            }
            kingSquare.Controls.Add(king);
            BoardDrawing(false);
            List<Square> attackSquares = new List<Square>();
            king.PrintMove(false);
            foreach (Square square in squaresList)
            {
                if (square.BackColor == ATTACK_COLOR)
                {
                    attackSquares.Add(square);
                }
            }
            BoardDrawing(false);
            foreach (Square square in attackSquares)
            {
                Piece attacker = GetPiece(square.Position);
                square.Controls.Clear();
                piecesList.Remove(attacker);
                foreach (Piece piece in piecesList)
                {
                    if (piece.Color != king.Color)
                    {
                        if (piece is Pawn pawn)
                        {
                            pawn.PrintAttack(false);
                        }
                        else
                        {
                            piece.PrintMove(false);
                        }
                    }
                }
                if (square.BackColor == MOVE_BACKGROUND_COLOR
                    || square.BackColor == MOVE_CONTRAST_COLOR
                    || square.BackColor == ATTACK_COLOR
                    || square.BackColor == PASSANT_COLOR)
                {
                    legalSquares.Remove(square);
                }
                piecesList.Add(attacker);
                square.Controls.Add(attacker);
                BoardDrawing(false);
            }
            foreach (Square square in squaresList)
            {
                square.Legal = legalSquares.Contains(square);
            }
        }

        public void DetermineLegalMoves(Piece piece)
        {
            if (!MovesForInCheck(piece))
            {
                MovesResultingInCheck(piece);
            }
        }

        private bool CheckAllForLegalMoves()
        {
            List<Piece> auxList = new List<Piece>(piecesList);
            bool legalMoves = false;
            foreach (Piece piece in auxList)
            {
                if (piece.Color == player)
                {
                    if (piece is King king)
                    {
                        DetermineLegalMovesForKing(king);
                        king.PrintCastling(false);
                        king.PrintMove(false);
                    }
                    else
                    {
                        DetermineLegalMoves(piece);
                        piece.PrintMove(false);
                    }
                    legalMoves = CheckPieceForLegalMoves();
                    BoardDrawing(false);
                    if (legalMoves) { break; }
                }
            }
            return legalMoves;
        }

        private bool CheckPieceForLegalMoves()
        {
            foreach (Square square in squaresList)
            {
                if (square.Controls.Count > 0)
                {
                    if (GetPiece(square.Position).Color == player)
                    {
                        square.Legal = false;
                    }
                }
                Color color = square.BackColor;
                if ((color == MOVE_BACKGROUND_COLOR
                    || color == MOVE_CONTRAST_COLOR
                    || color == ATTACK_COLOR
                    || color == PASSANT_COLOR
                    || color == CASTLING_COLOR)
                    && square.Legal)
                {
                    return true;
                }
            }
            return false;
        }

        #endregion
    }
}