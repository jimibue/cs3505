// Authors: James Yeates and Tyler Down

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
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
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        /// <summary>
        /// The model for this client.
        /// </summary>
        private BoggleClientModel model;

        /// <summary>
        /// Keeps track of whether a player disconnects during a game.
        /// </summary>
        private bool disconnected = false;

        /// <summary>
        /// True once the game has ended.
        /// </summary>
        private bool gameOver = false;

        // Allow access to Angle property of the images that allows to rotate images
        private RotateTransform rt = new RotateTransform();
        private RotateTransform rt1 = new RotateTransform();
        private RotateTransform rt2 = new RotateTransform();
        private RotateTransform rt3 = new RotateTransform();

        /// <summary>
        /// used to control how much letter images rotate
        /// </summary>
        private int rotateAmount = 0;
            
        /// <summary>
        /// Opens a game window and adds events to the model
        /// </summary>
        public GameWindow(BoggleClientModel model)
        {
           
            this.model = model;
            this.model.IncomingStartEvent += StartReceived;
            this.model.IncomingStopEvent += StopReceived;
            this.model.IncomingTimeEvent += TimeReceived;
            this.model.IncomingScoreEvent += ScoreReceived;
            this.model.IncomingTerminatedEvent += TerminatedReceived;
            this.model.IncomingTurnEvent += TurnReceived;
            this.model.IncomingErrorEvent += ErrorReceived;
            
            InitializeComponent();
            
            word_text_box.IsEnabled = false;
            send_word_button.IsEnabled = false;

            // Assign RenderTransform property for each letter image.
            letter_image1.RenderTransform = rt;
            letter_image2.RenderTransform = rt1;
            letter_image3.RenderTransform = rt2;
            letter_image4.RenderTransform = rt3;
            letter_image5.RenderTransform = rt;
            letter_image6.RenderTransform = rt1;
            letter_image7.RenderTransform = rt2;
            letter_image8.RenderTransform = rt3;
            letter_image9.RenderTransform = rt;
            letter_image10.RenderTransform = rt1;
            letter_image11.RenderTransform = rt2;
            letter_image12.RenderTransform = rt3;
            letter_image13.RenderTransform = rt;
            letter_image14.RenderTransform = rt1;
            letter_image15.RenderTransform = rt2;
            letter_image16.RenderTransform = rt3;
        }

        /// <summary>
        /// Display a message and closes window if an error occured.
        /// </summary>
        private void ErrorReceived()
        {
            if (!disconnected)
                MessageBox.Show("The server has disconnected. :(", "Disconnected", MessageBoxButton.OK);

            disconnected = true;
            Dispatcher.Invoke(new Action(() => { Close(); }));
        }

        /// <summary>
        /// Event handler that rotates letter images.
        /// </summary>
        private void TurnReceived()
        {
            if (model.otherPlayerScore < 0)
                return;
            Dispatcher.Invoke(new Action(() => { rt.Angle = rotateAmount  % 360; }));
            Dispatcher.Invoke(new Action(() => { rt1.Angle = -.5*rotateAmount % 360; }));
            Dispatcher.Invoke(new Action(() => { rt2.Angle = .75*rotateAmount % 360; }));
            Dispatcher.Invoke(new Action(() => { rt3.Angle = -rotateAmount % 360; }));
            rotateAmount += model.otherPlayerScore;
        }

        /// <summary>
        /// Event handler that handles when terminated is received.
        /// </summary>
        private void TerminatedReceived(String line)
        {
            disconnected = true;
            MessageBox.Show("The other player has disconnected. :(", "Disconnected", MessageBoxButton.OK);
            Dispatcher.Invoke(new Action(() => { Close(); }));
        }

        /// <summary>
        /// Event handler that updates players' scores when changed.
        /// </summary>
        private void ScoreReceived(String line)
        {
            string[] split = line.Split(new Char[] { ' ' });
            Dispatcher.Invoke(new Action(() => { current_player_score.Content = split[1]; }));
            Dispatcher.Invoke(new Action(() => { other_player_score.Content = split[2]; }));
            model.currentPlayerScore = Int32.Parse(split[1]);
            model.otherPlayerScore = Int32.Parse(split[2]);
        }

        /// <summary>
        /// Event handler that is called at the start of the game and sets up the board.
        /// </summary>
        private void StartReceived(String line)
        {
            string[] split = line.Split(new Char[] { ' ' });
            FillBoogleBoard(split[1]);

            model.otherPlayerName = split[3];
            Dispatcher.Invoke(new Action(() => { other_player_label1.Content = split[3] + "'s score"; }));
            Dispatcher.Invoke(new Action(() => { send_word_button.IsEnabled = true; }));
            Dispatcher.Invoke(new Action(() => { word_text_box.IsEnabled = true; }));
            Dispatcher.Invoke(new Action(() => { word_text_box.Focus(); }));
        }

        /// <summary>
        /// Takes in the game letters and sets up the appropriate image letters on the board.
        /// </summary>
        private void FillBoogleBoard(string p)
        {
            char[] chars = p.ToCharArray();
            string [] fileNames = new string [16];
            string fileName = "Resource\\boggleAlphabet\\";
            
            for (int i = 0; i < 16; i++ )
            {
                fileNames[i] = fileName+chars[i]+".png";
            }

            Dispatcher.Invoke(new Action(() => { letter_image1.Source = new BitmapImage(new Uri(fileNames[0], UriKind.Relative));  }));
            Dispatcher.Invoke(new Action(() => { letter_image2.Source = new BitmapImage(new Uri(fileNames[1], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image3.Source = new BitmapImage(new Uri(fileNames[2], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image4.Source = new BitmapImage(new Uri(fileNames[3], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image5.Source = new BitmapImage(new Uri(fileNames[4], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image6.Source = new BitmapImage(new Uri(fileNames[5], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image7.Source = new BitmapImage(new Uri(fileNames[6], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image8.Source = new BitmapImage(new Uri(fileNames[7], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image9.Source = new BitmapImage(new Uri(fileNames[8], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image10.Source = new BitmapImage(new Uri(fileNames[9], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image11.Source = new BitmapImage(new Uri(fileNames[10], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image12.Source = new BitmapImage(new Uri(fileNames[11], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image13.Source = new BitmapImage(new Uri(fileNames[12], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image14.Source = new BitmapImage(new Uri(fileNames[13], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image15.Source = new BitmapImage(new Uri(fileNames[14], UriKind.Relative)); }));
            Dispatcher.Invoke(new Action(() => { letter_image16.Source = new BitmapImage(new Uri(fileNames[15], UriKind.Relative)); }));
        }

        /// <summary>
        /// Event handler that handles when the game ends, closes this widow and opens the EndGameWindow
        /// </summary>
        private void StopReceived(String line)
        {
            disconnected = true;
            gameOver = true;
            Dispatcher.Invoke(new Action(() => { new EndGameWindow(model, line).Show(); }));
            Dispatcher.Invoke(new Action(() => { this.Close(); }));
        }

        /// <summary>
        /// Event handler that handles updating the timer in the GUI.
        /// </summary>
        private void TimeReceived(String line)
        {
            Dispatcher.Invoke(new Action(() => { time_label.Content = line.Substring(5); }));
        }

        /// <summary>
        /// Button used to send the word entered by the player.
        /// </summary>
        private void send_word_button_Click(object sender, RoutedEventArgs e)
        {
            model.SendGoMessage(word_text_box.Text);
            word_text_box.Text = "";
        }

        /// <summary>
        /// Ask the user if they want to exit before exiting, and if so disconnects
        /// the user and opens a new MainWindow if the user wants to play again.
        /// 
        /// If the other player disconnected, or if the game is over, the 
        /// "Are you sure you want to exit?" message is skipped.
        /// </summary>
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!disconnected)
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to exit? ", "Exiting", MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes)
                {
                    e.Cancel = true;
                    return;
                }
            }

            if (!gameOver)
            {
                model.disconnect();
                disconnected = true;
                new MainWindow(model.hostname, model.name).Show();
            }
        }

        /// <summary>
        /// Another way the user can exit the game, using a button.
        /// </summary>
        private void exit_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Event handler that allows the user to hit enter to send the word.
        /// </summary>
        private void word_text_box_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                model.SendGoMessage(word_text_box.Text);
                word_text_box.Text = "";
            }
        }
    }
}
