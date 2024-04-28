using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChessGame.Logic.General
{
    public class Direction
    {
        public readonly static Direction Up = new Direction(-1, 0);
        public readonly static Direction Down = new Direction(1, 0);
        public readonly static Direction Left = new Direction(0, -1);
        public readonly static Direction Right = new Direction(0, 1);
        public readonly static Direction UpLeft = Up + Left;
        public readonly static Direction UpRight = Up + Right;
        public readonly static Direction DownLeft = Down + Left;
        public readonly static Direction DownRight = Down + Right;

        public int RowOffset { get; }
        public int ColumnOffset { get; }

        public Direction(int rowOffset, int columnOffset)
        {
            RowOffset = rowOffset;
            ColumnOffset = columnOffset;
        }

        public static Direction operator +(Direction a, Direction b)
        {
            return new Direction(a.RowOffset + b.RowOffset, a.ColumnOffset + b.ColumnOffset);
        }

        public static Direction operator *(int scalar, Direction dir)
        {
            return new Direction(scalar * dir.RowOffset, scalar * dir.ColumnOffset);
        }
    }
}
