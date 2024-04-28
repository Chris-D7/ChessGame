using ChessGame.Logic.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame.Logic.Pieces
{

    public abstract class Piece
    {
        
        public abstract Player Color { get; }
        public abstract PictureBox PictureBox { get; }
        public bool Moved { get; set; } = false;
    }
}
