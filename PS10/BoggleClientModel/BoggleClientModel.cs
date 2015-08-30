// Authors: James Yeates and Tyler Down

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using CustomNetworking;
using System.Threading;

namespace BoggleClient
{
    /// <summary>
    /// The model for the client side of the Boggle game.
    /// </summary>
    public class BoggleClientModel
    {
        // The socket used to communicate with the server.  If no connection has been
        // made yet, this is null.
        private StringSocket socket;

        /// <summary>
        /// The current player's name.
        /// </summary>
        public string name { get; private set; }

        /// <summary>
        /// The other player's name.
        /// </summary>
        public string otherPlayerName { get; set; }

        /// <summary>
        /// The IP address string that the player used to connect.
        /// </summary>
        public string hostname { get; private set; }

        /// <summary>
        /// The current player's score.
        /// </summary>
        public int currentPlayerScore { get; set; }

        /// <summary>
        /// The other player's score.
        /// </summary>
        public int otherPlayerScore { get; set; }

        /// <summary>
        /// Holds the last message that the boggle server sent to the client.
        /// (Used for unit testing.)
        /// </summary>
        public string msgString;

        // Register for these events to be modified when a specific line of text arrives.
        public event Action<String> IncomingStartEvent;
        public event Action<String> IncomingStopEvent;
        public event Action<String> IncomingScoreEvent;
        public event Action<String> IncomingTerminatedEvent;
        public event Action<String> IncomingTimeEvent;
        public event Action IncomingTurnEvent;
        public event Action IncomingErrorEvent;

        /// <summary>
        /// Creates a not yet connected client model.
        /// </summary>
        public BoggleClientModel()
        {
            socket = null;
        }

        /// <summary>
        /// Connect to the server at the given hostname and port, with the given name.
        /// </summary>
        public void Connect(String hostname, int port, String name)
        {
            if (socket == null)
            {
                TcpClient client = new TcpClient(hostname, port);
                socket = new StringSocket(client.Client, UTF8Encoding.Default);

                this.name = name;
                this.hostname = hostname;

                socket.BeginSend("PLAY " + name + "\n", (e, p) => { }, null);
                socket.BeginReceive(LineReceived, null);
            }
        }

        /// <summary>
        /// Deal with an arriving line of text, and call an the appropriate action.
        /// </summary>
        private void LineReceived(String s, Exception e, object p)
        {
            if ((s == null && e == null))
            {
                return;
            }
            else if (e !=null)
            {
                if (IncomingErrorEvent != null)
                {
                    IncomingErrorEvent();
                }
                return;
            }

            s = s.ToUpper();

            if (s.StartsWith("TIME "))
            {
                if (IncomingTimeEvent != null)
                {
                    IncomingTimeEvent(s);
                }
            }
            else if (s.StartsWith("START "))
            {
                msgString = s;
                if (IncomingStartEvent != null)
                {
                    IncomingStartEvent(s);
                }

                // Start the letter turning mechanism.
                Thread t = new Thread(turnLetter);
                t.Start();
            }
            else if (s.StartsWith("SCORE "))
            {
                msgString = s;
                if (IncomingScoreEvent != null)
                {
                    IncomingScoreEvent(s);
                }
            }
            else if (s.StartsWith("TERMINATED"))
            {
                msgString = s;
                if (IncomingTerminatedEvent != null)
                {
                    IncomingTerminatedEvent(s);
                }
            }
            else if (s.StartsWith("IGNORING "))
            {
                msgString = s;
            }
            else if (s.StartsWith("STOP "))
            {
                msgString = s;
                if (IncomingStopEvent != null)
                {
                    IncomingStopEvent(s);
                }
            }

            // Listen for another message.
            socket.BeginReceive(LineReceived, null);
        }

        /// <summary>
        /// sends the action every 20 milliseconds to turn the letters
        /// </summary>
        private void turnLetter()
        {
            while(socket.Connected)
            {
                if (IncomingTurnEvent != null)
                {
                    IncomingTurnEvent();
                }
                Thread.Sleep(20);
            }
        }

        /// <summary>
        /// Sends a line of text to the server.
        /// </summary>
        /// <param name="line"></param>
        public void SendGoMessage(String line)
        {
            if (socket != null)
            {
                socket.BeginSend("WORD " + line + "\n", (e, p) => { }, null);
            }
        }

        /// <summary>
        /// Disconnects the current client model from the server.
        /// </summary>
        public void disconnect()
        {
            socket.Close();
        }
    }
}

