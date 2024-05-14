using System;
using System.Windows.Forms;

namespace ChessGame.Logic.General
{
    public enum Result
    {
        White,
        Black,
        KingVsKing,
        KingVsKingMinor,
        KingBishopVsKingBishop,
        LoneKing,
        StaleMate,
        FiftyMoveRule
    }

    public partial class EndScreen : Form
    {
        public EndScreen(Result result)
        {
            InitializeComponent();
            switch (result)
            {
                case Result.White: reasonLabel.Text = "White Wins"; break;
                case Result.Black: reasonLabel.Text = "Black Wins"; break;
                case Result.KingVsKing: reasonLabel.Text = ": Draw: King Vs King"; break;
                case Result.KingVsKingMinor: reasonLabel.Text = "Draw: King Vs King + Minor"; break;
                case Result.KingBishopVsKingBishop: reasonLabel.Text = "Draw: King + Bishop Vs King + Bishop"; break;
                case Result.LoneKing: reasonLabel.Text = "Draw: Lone King"; break;
                case Result.StaleMate: reasonLabel.Text = "Draw: Stalemate"; break;
                case Result.FiftyMoveRule: reasonLabel.Text = "Draw: Fifty Move Rule"; break;
            }
        }

        private void restartButton_Click(object sender, EventArgs e)
        {
            Program.Restart();
        }

        private void exitButton_Click(object sender, EventArgs e)
        {
            Program.Exit();
        }
    }
}
