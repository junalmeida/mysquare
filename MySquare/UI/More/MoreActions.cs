using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MySquare.UI.More
{
    partial class MoreActions : UserControl
    {
        public MoreActions()
        {
            InitializeComponent();

            listBox.AddItem("Shout\r\nBroadcast a message to your friends.", null);
            listBox.AddItem("The Leaderboard\r\nSee the weekely ranking.", null);
        }

        private void listBox_DrawItem(object sender, Tenor.Mobile.UI.DrawItemEventArgs e)
        {

        }

        private void listBox_SelectedItemClicked(object sender, EventArgs e)
        {
        }

        private void listBox_SelectedItemChanged(object sender, EventArgs e)
        {
        }

    }
}
