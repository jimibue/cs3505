// Authors: James Yeates and Tyler Down

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BoggleClient;
using System.ComponentModel;

namespace BoggleClient
{
    /// <summary>
    /// Interaction logic for EndGameWindow.xaml
    /// </summary>
    public partial class EndGameWindow : Window
    {
        /// <summary>
        /// The model for this client.
        /// </summary>
        private BoggleClientModel model;

        /// <summary>
        /// Creats the summary window for the game.
        /// </summary>
        public EndGameWindow(BoggleClientModel model, string stopMessage)
        {
            this.model = model;

            InitializeComponent();

            // Updates the player scores and the other player's information.
            current_score_label.Content = model.currentPlayerScore;
            other_score_label.Content = model.otherPlayerScore;
            other_name_label.Content = model.otherPlayerName + "'s score";
            other_player_legal_words_label.Content = model.otherPlayerName + "'s\nLegal Words";
            other_player_illegal_words_label.Content = model.otherPlayerName + "'s\nBad Words";

            // Sets the win/lose/tie message.
            if(model.currentPlayerScore>model.otherPlayerScore)
            {
                win_label.Content = "YOU WIN! :)";
            }
            else if (model.currentPlayerScore < model.otherPlayerScore)
            {
                win_label.Content = "YOU LOSE :(";
            }
            else
            {
                win_label.Content = "YOU TIE :/";
            }

            // Fills the text boxes with the played words.
            string[] split = stopMessage.Split(new Char[] { ' ' });
            int x;
            current_player_legal_words.Text += getList(split, 2, Int32.Parse(split[1]), out x);
            other_player_legal_words.Text += getList(split, x + 1, Int32.Parse(split[x]), out x);
            common_words.Text += getList(split, x + 1, Int32.Parse(split[x]), out x);
            current_player_illegal_words.Text += getList(split, x + 1, Int32.Parse(split[x]), out x);
            other_player_illegal_words.Text += getList(split, x + 1, Int32.Parse(split[x]), out x);
        }

        /// <summary>
        /// Helper method used to extract data from "STOP" message.
        /// </summary>
        private string getList(string[] array, int start, int count, out int end)
        {
            string list = "";
            for (int i = start; i < count + start; i++)
            {
                list += array[i] + "\n";
            }
            end = count + start;
            return list;
        }

        /// <summary>
        /// Disconnects the client and closes this window.  Opens a new starting window.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            
            model.disconnect();

            new MainWindow(model.hostname, model.name).Show();
        }

        /// <summary>
        /// Button that allows the user to close program.
        /// </summary>
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
