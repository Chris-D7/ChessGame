using System.Windows.Forms;

namespace ChessGame.Logic.General
{
    public class Square : Panel
    {
        public Position Position { get; set; }
        public SquareHandle SquareHandle { get; set; }
        public bool Legal { get; set; }

        public Square(Position position)
        {
            Position = position;
            Legal = true;
        }

        public Square Clone()
        {
            Square newSquare = new Square(new Position(Position.Row, Position.Column))
            {
                SquareHandle = this.SquareHandle,
                Size = this.Size,
                Location = this.Location,
                BackColor = this.BackColor
            };
            return newSquare;
        }
    }
}