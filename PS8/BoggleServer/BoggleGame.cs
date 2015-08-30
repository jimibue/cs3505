// Authors: James Yeates and Tyler Down

using BB;
using CustomNetworking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BoggleServer
{
    /// <summary>
    /// Represents one boggle game between two clients.
    /// </summary>
    class BoggleGame
    {
        /// <summary>
        /// One of the two players in this game.
        /// </summary>
        private Player player1, player2;

        /// <summary>
        /// The remaining time. This is initially set to the number of seconds 
        /// the game should last, and decremented as time passes.
        /// </summary>
        private int time;

        /// <summary>
        /// The boggle board used for this game.
        /// </summary>
        private BoggleBoard board;

        /// <summary>
        /// The set of legal words.
        /// </summary>
        private HashSet<string> dictionary;

        /// <summary>
        /// The set of common words played by both players.
        /// </summary>
        private HashSet<string> commonWords;

        /// <summary>
        /// Used as the lock object when updating a player's score.
        /// </summary>
        private readonly Object scoreLock = new Object();

        /// <summary>
        /// True if both players are currently connected.
        /// </summary>
        private bool playersConnected = true;

        /// <summary>
        /// Construcs a new boggle game.
        /// </summary>
        /// <param name="one">The first player.</param>
        /// <param name="two">The second player.</param>
        /// <param name="time">The number of seconds the game will last.</param>
        /// <param name="letters">A string of 16 letters used for the boggle board. If left empty, a random board is created.</param>
        /// <param name="dictionary">A reference to the set of legal words.</param>
        public BoggleGame(Player one, Player two, int time, string letters, ref HashSet<string> dictionary)
        {
            this.player1 = one;
            this.player2 = two;

            // Create the board.
            if (letters == "")
                board = new BoggleBoard();
            else
                board = new BoggleBoard(letters);
            
            this.dictionary = dictionary;

            commonWords = new HashSet<string>();

            // Send START messages to the clients, and begin listening for words.
            player1.ss.BeginSend("START " + board.ToString() + " " + time.ToString() + " " + player2.name + "\n", (ee, pp) => { }, player1.ss);
            player1.ss.BeginReceive(WordReceived, player1);
            player2.ss.BeginSend("START " + board.ToString() + " " + time.ToString() + " " + player1.name + "\n", (ee, pp) => { }, player2.ss);
            player2.ss.BeginReceive(WordReceived, player2);

            // Start the game timer.
            Thread timer = new Thread(updateClock);
            timer.Start(time);
        }

        /// <summary>
        /// Updates the game's remaining time. Once time runs out, the game summaries are sent.
        /// </summary>
        /// <param name="startTime"></param>
        private void updateClock(object startTime)
        {
            time = (int)startTime;

            // Send TIME messages every second.
            while (time >= 0 && playersConnected)
            {
                player1.ss.BeginSend("TIME " + time.ToString() + "\n", (ee, pp) => { }, this);
                player2.ss.BeginSend("TIME " + time.ToString() + "\n", (ee, pp) => { }, this);
                Thread.Sleep(1000);
                time--;
            }

            // Send game over message, ignore further communication.
            if (playersConnected)
                endGame();
        }

        /// <summary>
        /// This callback method is called whenever a player tries to send a word.
        /// </summary>
        private void WordReceived(string word, Exception e, object payload)
        {
            Player p = (Player)payload;
            bool isPlayer1 = player1.Equals(p);

            // Check if a player disconnected or an error occurred.
            if (isDisconnected(word, e, isPlayer1))
                return;

            if (time > 0)
            {
                // Check if the message begins with "WORD " followed by at least one non-whitespace character.
                if (word.StartsWith("WORD ") && word.Substring(5).Trim().Length > 0)
                {
                    word = word.Substring(5).ToUpper().Trim();

                    // Figure out which player sent the word, and process it.
                    if (isPlayer1)
                        processWord(word, player1, player2);
                    else
                        processWord(word, player2, player1);
                }
                // Otherwise, the client deviated from protocol. Send an IGNORING message.
                else
                {
                    p.ss.BeginSend("IGNORING " + word + "\n", (ee, pp) => { }, p);
                }

                // Listen for more words.
                p.ss.BeginReceive(WordReceived, p);
            }
         
        }

        /// <summary>
        /// Checks if a player disconnected or an error occurred. If so, it sends the 
        /// terminated message to the other player and closes the game.
        /// </summary>
        /// <param name="word"></param>
        /// <param name="e"></param>
        /// <param name="isPlayer1"></param>
        /// <returns>True if a player disconnected or an error occurred.</returns>
        private bool isDisconnected(string word, Exception e, bool isPlayer1)
        {
            if ( (word == null && e == null) || e != null)
            {
                playersConnected = false; 

                if (isPlayer1)
                {
                    player2.ss.BeginSend("TERMINATED\n", (ee, pp) => { }, player2);
                    player2.ss.Close();
                }
                else
                {
                    player1.ss.BeginSend("TERMINATED\n", (ee, pp) => { }, player1);
                    player1.ss.Close();
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Processes the played word. If it's legal, update the scores.
        /// Then send the SCORE messages.
        /// </summary>
        private void processWord(string word, Player currentPlayer, Player otherPlayer)
        {
            lock (scoreLock)
            {
                if (word.Length < 3)
                    return;

                // Check if the word is legal.
                if (isLegalWord(word))
                {
                    // Check if the other player has already played the word.
                    if (otherPlayer.legalWords.Contains(word))
                    {
                        // Remove the word
                        otherPlayer.legalWords.Remove(word);

                        // Add it to common words
                        commonWords.Add(word);

                        // Reduce the score
                        otherPlayer.score -= getWordScore(word);
                    }
                    // Otherwise, the other player hasn't played the word.
                    else
                    {
                        // Make sure current player hasn't already played the word, and that it's not a common word.
                        if (!currentPlayer.legalWords.Contains(word) && !commonWords.Contains(word))
                        {
                            // Add it to their list of legal words.
                            currentPlayer.legalWords.Add(word);

                            // Increment their score.
                            currentPlayer.score += getWordScore(word);
                        }
                        else
                            return;
                    }
                }
                // Otherwise, the word is illegal.
                else
                {
                    // Make sure the player hasn't already played the word.
                    if (!currentPlayer.illegalWords.Contains(word))
                    {
                        // Add it to the list of illegal words.
                        currentPlayer.illegalWords.Add(word);

                        // Decrement the score.
                        currentPlayer.score += -1;
                    }
                    else
                        return;
                }

                // Send the scores.
                currentPlayer.ss.BeginSend("SCORE " + currentPlayer.score + " " + otherPlayer.score + "\n", (ee, pp) => { }, currentPlayer);
                otherPlayer.ss.BeginSend("SCORE " + otherPlayer.score + " " + currentPlayer.score + "\n", (ee, pp) => { }, otherPlayer);
            }
        }

        /// <summary>
        /// Returns true if the given word is legal.
        /// </summary>
        private bool isLegalWord(string word)
        {
            //if it is not in the dictionary it is not valid
            if (!dictionary.Contains(word))
                return false;

            //is it a valid word for the current game.
            else return board.CanBeFormed(word);
        }

        /// <summary>
        /// Returns the point value of the given word.
        /// </summary>
        private int getWordScore(string word)
        {
            int length = word.Length;

            if ((length == 3) || (length == 4))
                return 1;
            else if (length == 5)
                return 2;
            else if (length == 6)
                return 3;
            else if (length == 7)
                return 5;
            else
                return 11;
        }

        /// <summary>
        /// Sends the final score and game summaries, then closes the StringSockets.
        /// </summary>
        private void endGame()
        {
            // Send final score.
            player1.ss.BeginSend("SCORE " + player1.score + " " + player2.score + "\n", (ee, pp) => { }, player1);
            player2.ss.BeginSend("SCORE " + player2.score + " " + player1.score + "\n", (ee, pp) => { }, player2);

            bool notSent1 = true, notSent2 = true;

            // Send the game summaries.
            player1.ss.BeginSend(getSummaryString(player1, player2) + "\n", (ee, pp) => { notSent1 = false; }, player1);
            // player1.ss.Close();
            player2.ss.BeginSend(getSummaryString(player2, player1) + "\n", (ee, pp) => { notSent2 = false; }, player2);

            while (notSent1 || notSent2)
                Thread.Sleep(100);

            // Close the sockets.
            player1.ss.Close();
            player2.ss.Close();
        }

        /// <summary>
        /// Creates and returns the game summary string.
        /// </summary>
        /// <param name="currentPlayer"></param>
        /// <param name="otherPlayer"></param>
        /// <returns></returns>
        private string getSummaryString(Player currentPlayer, Player otherPlayer)
        {
            StringBuilder summary = new StringBuilder();
            summary.Append("STOP");

            // The current player's legal words:
            summary.Append(" " + currentPlayer.legalWords.Count);

            foreach (string word in currentPlayer.legalWords)
                summary.Append(" " + word);

            // The opponent's legal words:
            summary.Append(" " + otherPlayer.legalWords.Count);

            foreach (string word in otherPlayer.legalWords)
                summary.Append(" " + word);

            // The common words:
            summary.Append(" " + commonWords.Count);

            foreach (string word in commonWords)
                summary.Append(" " + word);

            // The current player's illegal words:
            summary.Append(" " + currentPlayer.illegalWords.Count);

            foreach (string word in currentPlayer.illegalWords)
                summary.Append(" " + word);

            // The opponent's illegal words:
            summary.Append(" " + otherPlayer.illegalWords.Count);

            foreach (string word in otherPlayer.illegalWords)
                summary.Append(" " + word);

            return summary.ToString();
        }
    }
}
