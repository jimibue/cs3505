// Authors: James Yeates and Tyler Down

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using BB;
using System.Net;
using System.Net.Sockets;
using CustomNetworking;
using System.Threading;

namespace BoggleServer
{
    /// <summary>
    /// Represents a boggle server.
    /// Once two cleints connect and send their names, a new boggle game is created.
    /// </summary>
    public class BoggleServer
    {
        /// <summary>
        /// The number of seconds that each boggle game should last.
        /// This should be a positive integer.
        /// </summary>
        private int time;

        /// <summary>
        /// The pathname of a file that contains all the legal words. 
        /// The file should contain one word per line.
        /// </summary>
        private string dictionaryPath;

        /// <summary>
        /// An optional string consisting of exactly 16 letters. 
        /// If provided, this will be used to initialize each Boggle board.
        /// </summary>
        private string letters;

        /// <summary>
        /// A set that will hold the legal words from the file at the
        /// given pathname dictionaryPath.
        /// A reference to this object is passed to each new boggle game.
        /// </summary>
        private HashSet<string> dictionary;

        /// <summary>
        /// Holds the players that are waiting to play.
        /// Once two players are in the queue, they are both dequeued, 
        /// and a new boggle game is created and started.
        /// </summary>

        private Queue<BoggleGame> games;

        /// <summary>
        /// Listens for incoming connections.
        /// </summary>
        private TcpListener server;

        /// <summary>
        /// Takes two required and one optional command line parameter.
        /// 
        /// 1st command line parameter: (required)
        ///   The number of seconds that each Boggle game should last. This should be a positive integer.
        /// 2nd command line parameter: (required)
        ///   The pathname of a file that contains all the legal words. The file should contain one word per line.
        /// 3rd command line parameter: (optional)
        ///   An optional string consisting of exactly 16 letters.
        /// </summary>
        public static void Main(string[] args)
        {
            // Store the time.
            int time;
            Int32.TryParse(args[0], out time);

            // Store the dictionary path.
            string dictionaryPath = args[1];

            // Store the letters (if provided).
            string letters = "";
            if (args.Length == 3)
                letters = args[2];

            // Start a new boggle server.
            new BoggleServer(time, dictionaryPath, letters);
            Console.Read();
     }

        /// <summary>
        /// Constructs a new boggle server.
        /// </summary>
        /// <param name="time">The number of seconds each game will last.</param>
        /// <param name="dictionaryPath">The path to the file that contains all legal words.</param>
        /// <param name="letters">Either the empty string (for a random boggle board) or a string of exactly 16 letters.</param>
        public BoggleServer(int time, string dictionaryPath, string letters)
        {
            // Store the time, dictionary path, and string of letters.
            this.time = time;
            this.dictionaryPath = dictionaryPath;
            this.letters = letters;

            // Put words from dictionary into the hash set.
            dictionary = new HashSet<string>();
            loadDictionary(dictionaryPath);

            games = new Queue<BoggleGame>();

            // Start the server, and listen for incoming connections.
            server = new TcpListener(IPAddress.Any, 2000);
            server.Start();
            server.BeginAcceptSocket(ConnectionReceived, null);
        }

        /// <summary>
        /// This callback method is called when a connection has been received.
        /// </summary>
        private void ConnectionReceived(IAsyncResult ar)
        {
            Socket socket = server.EndAcceptSocket(ar);
            StringSocket ss = new StringSocket(socket, UTF8Encoding.Default);
            
            // Set the StringSocket to listen for a name from the client.
            ss.BeginReceive(NameReceived, ss);
            
            // Set the server to listen for another connection.
            server.BeginAcceptSocket(ConnectionReceived, null);
        }

        /// <summary>
        /// This callback method is called when a name has been received from the connected client.
        /// </summary>
        private void NameReceived(string name, Exception e, object payload)
        {
            StringSocket ss = (StringSocket)payload;
            
            // Check that the received string starts with "PLAY " followed by at least one non-whitespace character.
            if (name.StartsWith("PLAY ") && name.Substring(5).Trim().Length > 0)
            {
                name = name.Substring(5);

                lock (games)
                {
                    // If there is no game in the queue, create one and add it.
                    if (games.Count == 0 )
                    {
                        games.Enqueue(new BoggleGame(new Player(ss, name)));
                    }
                    // Otherwise, there is a game in the queue. If the player is still connected, start the game.
                    else if (games.Count == 1)
                    {
                        // Check if the player in game is connected, and if so, start the game.
                        if (games.Peek().playersConnected)
                            games.Dequeue().startGame(new Player(ss, name), time, letters, ref dictionary);
                        
                        // Otherwise, the player in the game has disconnected. Dequeue it, and enqueue a new game.   
                        else
                        {
                            games.Dequeue();
                            games.Enqueue(new BoggleGame(new Player(ss, name)));
                        }
                    }
                }
            }
            // Otherwise, the client deviated from protocol. Send an IGNORING message.
            else
            {
                ss.BeginSend("IGNORING " + name + "\n", (ee, pp) => { }, ss);
            }
        }

        /// <summary>
        /// Reads the file of legal words from the given pathname and enters each line 
        /// into the dictionary hashset field.
        /// </summary>
        /// <param name="dictionaryPath">The path to the file of legal words.</param>
        private void loadDictionary(string dictionaryPath)
        {
            using (StreamReader reader = new StreamReader(dictionaryPath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                    dictionary.Add(line);
            }
        }
    }

    /// <summary>
    /// This class represents a player, which is constructed with a StringSocket
    /// and a name.
    /// </summary>
    public class Player
    {
        /// <summary>
        /// The StringSocket of this player.
        /// </summary>
        public StringSocket ss;

        /// <summary>
        /// The name of the player.
        /// </summary>
        public string name;

        /// <summary>
        /// The player's current score.
        /// </summary>
        public int score;

        /// <summary>
        /// The set of legal words played by the player.
        /// </summary>
        public HashSet<string> legalWords;

        /// <summary>
        /// The set of illegal words played by the player.
        /// </summary>
        public HashSet<string> illegalWords;

        /// <summary>
        /// Creates a player with the given StringSocket and name.
        /// </summary>
        public Player(StringSocket ss, string name)
        {
            this.ss = ss;
            this.name = name;
            score = 0;
            legalWords = new HashSet<string>();
            illegalWords = new HashSet<string>();
        }
    }
}
