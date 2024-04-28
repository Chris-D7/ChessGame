using ChessGame.Logic.General;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class Chess : Form
    {
        public Chess()
        {
            InitializeComponent();
            this.Controls.Add(new Board());
        }
    }
}
