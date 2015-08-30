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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BoggleClient;
using System.Net;

namespace BoggleClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// The model for this client.
        /// </summary>
        private BoggleClientModel model;

        /// <summary>
        /// This is the beginning window for the client view.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            IPAddress_Text_Box.Focus();
            
            model = new BoggleClientModel();
        }

        /// <summary>
        /// Opens a new MainWindow with the last hostname and name that the client used.
        /// </summary>
        public MainWindow(string hostname, string name)
        {
            InitializeComponent();
            Play_Button.Focus();

            IPAddress_Text_Box.Text = hostname;
            Name_Text_Box.Text = name;

            model = new BoggleClientModel();
        }

        /// <summary>
        /// Connects the client to the server once the play button is clicked.
        /// </summary>
        private void Play_Button_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        /// <summary>
        /// Connects the client to the server if the Enter key is pushed.
        /// </summary>
        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                Connect();
            }
        }

        /// <summary>
        /// Tries to connect the client to the server.
        /// If an error occurs, a message box appears.
        /// </summary>
        private void Connect()
        {
            if (Name_Text_Box.Text.Trim().Length == 0)
            {
                MessageBox.Show("Invalid Name. Please enter one or more characters.", "Invalid Input");
                return;
            }

            try
            {
                model.Connect(IPAddress_Text_Box.Text, 2000, Name_Text_Box.Text);
            }
            catch
            {
                MessageBox.Show("Invalid IP Address. Please try again.", "Invalid Input");
                return;
            }

            // Close this window and open the game window.

            new GameWindow(model).Show();
            this.Close();
        }
    }
}
