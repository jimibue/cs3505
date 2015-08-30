Authors: James Yeates and Tyler Down

-------------------------------- PS10 -----------------------------------

We separated our Web Server and our Boggle Server into two separate projects.
However, when BoggleServer.exe is run, a Web Server is started as well as a Boggle Server.

Once a game ends, all of the game information is added to a database. This is done in the BoggleGame class.


Database Description:

We closely followed the database organization guidelines given in the PS10 specifications to build our 
tables, with one exception: we added player names to the table that contains the outcome of each game.

We have ta table that keeps track of words played but never use it.  This was meant to be used to rebuild
the summary string for the the game page but we did not finish this part.

Query for "game?players=JOHN1" - gets information to create the individaul player webpage:
	(SELECT gID, DateTime, p1Score, p2Score, p2Name
	FROM cs3500_down.Game 
	WHERE p1Name = 'JOHN1')
	union
	(SELECT gID, DateTime, p2Score, p1Score, p1Name
	FROM cs3500_down.Game 
	WHERE p2Name = 'JOHN1')
	order by gID;

Query for  "game?id=16" - gets information to create a webpage about a specific game:
	select p1Name, p2Name, p1Score, p2Score, DateTime, TimeLimit, Board
	From cs3500_down.Game WHERE gID =16;

Queries for"players" - gets information to create page for player stats:

	SELECT * FROM cs3500_down.Player;

	(SELECT p1Score, p2Score From cs3500_down.Game Where pID1 = 11)
	UNION
	(SELECT p2Score, p1Score From cs3500_down.Game Where pID2 = 11);



-------------------------------- PS9 ------------------------------------

GUI Features:

- The User can hit the Enter key and/or use a button to connect to a game.

- The user can also hit the Enter key and/or use a button to submit a word during a game.

- The user can hit the exit button or the red X to exit out of game or any other window. A pop up will confirm that the user wants to exit.

- If the user exits during a game, a message is sent to the other player notifying him/her and the player is disconnected.

- The Boggle board letters spin, and the user's score determines how fast the other player's letters will spin.

- If the server encounters an error, a pop-up message will alert the players, and they will be disconnected.

- The beginning/connection window gives basic rules for the game.

- The final window displays a detailed result of the game, and a win/lose/tie message depending on how the player did.
