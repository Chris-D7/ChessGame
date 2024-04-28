using ChessGame.Logic.General;
using System.Windows.Forms;

namespace ChessGame.Logic.Pieces
{

    public abstract class Piece : PictureBox
    {
        public abstract Position Position { get; set; }
        public abstract Player Color { get; }
        public bool Moved { get; set; } = false;
        public bool Active { get; set; } = false;
        public Board board;

        public Piece(Board board)
        {
            this.Size = new System.Drawing.Size(80, 80);
            this.Location = new System.Drawing.Point(0, 0);
            this.board = board;
        }
    }
}
