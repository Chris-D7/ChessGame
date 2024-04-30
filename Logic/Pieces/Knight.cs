﻿using ChessGame.Logic.General;
using System;

namespace ChessGame.Logic.Pieces
{
    public class Knight : Piece
    {
        public override Position Position { get; set; }
        public override Player Color { get; }

        public Knight(Player color, Position position, Board board) : base(board)
        {
            Color = color;
            Position = position;
            if (Color == Player.White)
            {
                this.Image = Properties.Resources.WhiteKnight;
            }
            else
            {
                this.Image = Properties.Resources.BlackKnight;
            }
        }

        public override void AttackClick(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void ClickOn(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        public override void PrintMove()
        {
            throw new NotImplementedException();
        }

        public override void PrintAttack()
        {
            throw new NotImplementedException();
        }
    }
}
