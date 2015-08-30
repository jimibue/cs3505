//Authors: James Yeates and Tyler Down

using CustomNetworking;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace WebServer
{
    public class WebServer
    {
        /// <summary>
        /// Listens for incoming connections.
        /// </summary>
        private TcpListener server;
        
        /// <summary>
        /// beggining of string sent to web page
        /// </summary>
        private StringBuilder sendString = new StringBuilder("HTTP/1.1 200 OK\r\nConnection: close\r\nContent-Type: text/html; charset=UTF-8\r\n\r\n");
        
        /// <summary>
        /// string used to connect to database
        /// </summary>
        private const string connectionString = "server=atr.eng.utah.edu;database=cs3500_down;uid=cs3500_down;password=085389825";

        /// <summary>
        /// Starts a single web server and keeps it running.
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            new WebServer();
            Console.Read();
        }

        /// <summary>
        /// Constructs a new web server.
        /// </summary>
        public WebServer()
        {
            Console.WriteLine("Web Server started.");

            // Start the server, and listen for incoming connections.
            server = new TcpListener(IPAddress.Any, 2500);
            server.Start();
            server.BeginAcceptSocket(ConnectionReceived, null);
            sendString.Append("<html>");
        }

        /// <summary>
        /// This callback method is called when a connection has been received.
        /// </summary>
        private void ConnectionReceived(IAsyncResult ar)
        {
            Socket socket = server.EndAcceptSocket(ar);
            StringSocket ss = new StringSocket(socket, UTF8Encoding.Default);
            
            // Set the StringSocket to listen for a name from the client.
            ss.BeginReceive(GetReceived, ss);
            
            // Set the server to listen for another connection.
            server.BeginAcceptSocket(ConnectionReceived, null);
        }

        private void GetReceived(string s, Exception e, object payload)
        {
            StringSocket ss = (StringSocket)payload;
         

            //check for errors / lost connection
            if(payload == null)
            {
                return;
            }
            if(e != null)
            {
                return;
            }
            if( s.StartsWith("GET /players HTTP/1.1") )
            {
                sendString.Append("<h1>Players Page</h1><table border =\"1\">");
                sendString.Append(getTableHeader("players"));
                
                sendString.Append(GetPlayersPage());
                ss.BeginSend(sendString.ToString(), (ee, oo) => { }, ss);
                sendString.Clear();
            }
            else if (s.StartsWith( "GET /games?player="))
            {
                //get name
                s = s.Substring(18);
                s = s.Substring(0, s.IndexOf(" "));

               sendString.Append("<h1>Player Page</h1><table border =\"1\">");
               sendString.Append(getTableHeader("player"));
               
               sendString.Append(GetPlayerInfo(s));
               sendString.Append("</table>");
               sendString.Append("</html>");
               ss.BeginSend(sendString.ToString(), (ee, oo) => { }, ss);
               sendString.Clear();
            }
            else if (s.StartsWith("GET /game?id="))
            {
                s = s.Substring(13);
                s = s.Substring(0,s.IndexOf(" "));
                int id = 0;
                if(int.TryParse(s, out id))
                {
                    sendString.Append("<h1>Game Page</h1><table border =\"1\">");
                    sendString.Append(getTableHeader("game"));

                    sendString.Append( GetGameInfo(id));
                    sendString.Append("</table>");
                    sendString.Append("</html>");

                    ss.BeginSend(sendString.ToString(), (ee, oo) => { }, ss);
                    sendString.Clear();
                    
                }
                else
                {
                    sendString.Append("<h1>The URL that you entered was invalid</h1>"+
                    "<H3>Please try one of the following options:</h3>"+
                    "<p>for players win, loss and tie information enter <bold>http://localhost:2500/players</bold>"+
                    "<p>for Individual player information enter <bold>http://localhost:2500/games?player=name</bold> where name is the name of the player" +
                    "<p>for Individual game information enter <bold>http://localhost:2500/game?id=number</bold> where number is an int that identifies that game" +
                    "</html>");
                    ss.BeginSend(sendString.ToString(), (ee, oo) => { }, ss);
                    sendString.Clear();
                
                }
            }
            else
            {
                sendString.Append("<h1>The URL that you entered was invalid</h1>" +
                   "<H3>Please try one of the following options:</h3>" +
                   "<p>for players win, loss and tie information enter <b>http://localhost:2500/players</b>" +
                   "<p>for Individual player information enter <b>http://localhost:2500/games?player=name</b> where name is the name of the player" +
                   "<p>for Individual game information enter <b>http://localhost:2500/game?id=number</b> where number is an int that identifies that game" +
                   "</html>");
                ss.BeginSend(sendString.ToString(), (ee, oo) => { }, ss);
                sendString.Clear();
            }

            ss.Close();
        }

        /// <summary>
        /// gets the player info thml string
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private string GetPlayerInfo(string name)
        {
            StringBuilder htmlString = new StringBuilder();
           
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "(SELECT gID, DateTime, p1Score, p2Score, p2Name" +
                    " FROM cs3500_down.Game" +
                    " WHERE p1Name = '" + name + "')" +
                    " union" +
                    " (SELECT gID, DateTime, p2Score, p1Score, p1Name" +
                    " FROM cs3500_down.Game" +
                    " WHERE p2Name = '" +name+ "')" +
                    "order by gID;";

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        //htmlString.Append("<table>");
                        //htmlString.Append(getTableHeader("player"));

                        while (reader.Read())
                        {
                            string [] row = new string[]{reader[0].ToString(), reader[1].ToString() , reader[2].ToString() , reader[3].ToString(), reader[4].ToString()};
                            htmlString.Append(getRow(row,"<td>","</td>"));
                        }
                        
                        return htmlString.ToString();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return "";
            }
        }

        private List<string> getPlayerList()
        {
            List<string> playerList = new List<string>();
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "SELECT Name FROM cs3500_down.Player";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            playerList.Add(reader[0].ToString());
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return playerList;
            }

        }

        private string GetPlayersPage()
        {
            List<string> playerList = getPlayerList();
            StringBuilder htmlString = new StringBuilder();
            foreach (string name in playerList)
            {
                
                
                // Connect to the DB
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    try
                    {
                        // Open a connection
                        conn.Open();

                        // Create a command
                        MySqlCommand command = conn.CreateCommand();
                        command.CommandText = "(SELECT p1Score, p2Score From cs3500_down.Game Where p1Name ='"+name+"')" +
                         " UNION " +
                         "(SELECT p2Score, p1Score From cs3500_down.Game Where p2Name ='"+name+"');";

                        // Execute the command and cycle through the DataReader object
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            int wins = 0;
                            int ties = 0;
                            int loses = 0;

                            while (reader.Read())
                            {
                                if ((int)reader[0] > (int)reader[1])
                                    wins++;
                                else if ((int)reader[0] == (int)reader[1])
                                    ties++;
                                else
                                    loses++;
                            }
                            htmlString.Append("<tr><td>" + name + "</td><td>" + wins + "</td><td>"
                                + loses + "</td><td>" + ties + "</td></tr>");
                            
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            return htmlString.ToString();
        }

        private string GetGameInfo(int gID)
        {
            StringBuilder htmlString = new StringBuilder();
           
            // Connect to the DB
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    // Open a connection
                    conn.Open();

                    // Create a command
                    MySqlCommand command = conn.CreateCommand();
                    command.CommandText = "select p1Name, p2Name, p1Score, p2Score, DateTime, TimeLimit, Board" +
                    " From cs3500_down.Game WHERE gID=" + gID + ";";

                    // Execute the command and cycle through the DataReader object
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if(reader.Read())
                        {
                            htmlString.Append("<tr><td>" + reader[0] + "</td><td>" + reader[1] + "</td><td>"
                               + reader[2] + "</td><td>" + reader[3] + "</td><td>" + reader[4] + "</td><td>" +
                               reader[5] + "</td><td>");
                            
                            Char[] board = reader[6].ToString().ToCharArray();
                            htmlString.Append("<table border = \"1\"><tr><td>" + board[0] + "</td>" +
                                "<td>" + board[1] + "</td>" +
                                "<td>" + board[2] + "</td>" +
                                "<td>" + board[3] + "</td></tr><tr>" +
                                "<td>" + board[4] + "</td>" +
                                "<td>" + board[5] + "</td>" +
                                "<td>" + board[6] + "</td>" +
                                "<td>" + board[7] + "</td></tr><tr>" +
                                "<td>" + board[8] + "</td>" +
                                "<td>" + board[9] + "</td>" +
                                "<td>" + board[10] + "</td>" +
                                "<td>" + board[11] + "</td></tr><tr>" +
                                "<td>" + board[12] + "</td>" +
                                "<td>" + board[13] + "</td>" +
                                "<td>" + board[14] + "</td>" +
                                "<td>" + board[15] + "</td></tr></table>");

                           htmlString.Append("</td></tr>");
                        }
                        
                    }
                    return htmlString.ToString();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                return "";
            }
        }

    private string getTableHeader( string webpage)
    {
        if (webpage == "game")
            return getRow(new string []{"P1 Name", "P2 Name","P1 Score", "P2 Score",
                "Date and Time","Time", "Board"}, "<th>", "</th>");
        else if (webpage == "players")
            return getRow(new string[]{"Name", "Games Won","Games Lost", "Games Tied"}, "<th>", "</th>");
        else if (webpage == "player")
            return getRow(new string[] { "Game ID", "DateTime", "Your score", "Opponents score", "Opponents Name" }, "<th>", "</th>");
        return "";
    }

    private string getRow(string[] p, string opentag, string closetag)
    {
        string row = "<tr>";
        foreach(string s in p)
        {
            row +=opentag+ s + closetag;
        }
        return row += "</tr>";
    }




    }
}
