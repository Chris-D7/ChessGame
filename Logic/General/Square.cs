using System.Windows.Forms;

namespace ChessGame.Logic.General
{
    public class Square : Panel
    {
        public Position Position { get; set; }

        public Square(Position position)
        {
            Position = position;
        }
    }
}
