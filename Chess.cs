using ChessGame.Logic.General;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class Chess : Form
    {
        public Chess()
        {
            InitializeComponent();
            Board board = new Board();
            this.Controls.Add(board);
        }
    }
}
