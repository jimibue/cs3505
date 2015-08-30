// Authors: James Yeates and Tyler Down

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;
using CC;
using System.Threading;

namespace BoggleServerUnitTests
{
    /// <summary>
    /// Unit tests the BoggleServer and BoggleGame classes.
    /// </summary>
    [TestClass]
    public class BoggleServerUnitTest1
    {
        // Create the boggle server before running tests.
        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            new BoggleServer.BoggleServer(5, "..\\..\\..\\Resources\\Libraries\\dictionary.txt", "TESRRDELEAOBSINM");
        }

        /// <summary>
        /// A simple test that imitates a simple game and tests whether 
        /// the client receives the proper strings from the server.
        /// </summary>
        [TestMethod]
        public void TestSimpleGame()
        {
            // Create two clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();

            // Connect the clients.
            client1.Connect("localhost", 2000, "PLAY Harry");
            client2.Connect("localhost", 2000, "PLAY Hermione");

            // Check start messages.
            Thread.Sleep(100);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 Hermione", client1.msgString);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 Harry", client2.msgString);

            // Send words, and check the score messages:

            client1.SendGoMessage("WORD read");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 1 0", client1.msgString);
            Assert.AreEqual("SCORE 0 1", client2.msgString);

            client2.SendGoMessage("WORD reads");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 1 2", client1.msgString);
            Assert.AreEqual("SCORE 2 1", client2.msgString);

            // Send a word already played by other player
            client2.SendGoMessage("WORD read");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 0 2", client1.msgString);
            Assert.AreEqual("SCORE 2 0", client2.msgString);

            // Send an invalid word
            client2.SendGoMessage("WORD asdfjkl");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 0 1", client1.msgString);
            Assert.AreEqual("SCORE 1 0", client2.msgString);

            // Make time run out, then check game summary messages.
            Thread.Sleep(6000);
            Assert.AreEqual("STOP 0 1 READS 1 READ 0 1 ASDFJKL", client1.msgString); 
            Assert.AreEqual("STOP 1 READS 0 1 READ 1 ASDFJKL 0", client2.msgString);
        }

        /// <summary>
        /// Tests sending illegal words.
        /// </summary>
        [TestMethod]
        public void TestIllegalWords()
        {
            // Create two clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();

            // Connect the clients.
            client1.Connect("localhost", 2000, "PLAY Harry");
            client2.Connect("localhost", 2000, "PLAY Hermione");
            Thread.Sleep(100);

            // Send words, and check the score messages:

            client1.SendGoMessage("WORD asdfjkl");
            client1.SendGoMessage("WORD re");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE -1 0", client1.msgString);
            Assert.AreEqual("SCORE 0 -1", client2.msgString);
        }

        /// <summary>
        /// Tests for ignoring messages when the client doesn't send a 
        /// proper "PLAY (name)" message.
        /// </summary>
        [TestMethod]
        public void TestIgnoringName()
        {
            // Create two clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();

            // Check for IGNORING statements:

            client1.Connect("localhost", 2000, "asd");
            Thread.Sleep(100);
            Assert.AreEqual("IGNORING asd", client1.msgString);

            client1.Connect("localhost", 2000, "PLAY  ");
            Thread.Sleep(100);
            Assert.AreEqual("IGNORING PLAY  ", client1.msgString);
        }

        /// <summary>
        /// Tests for ignoring messages when the client doesn't send
        /// proper "WORD (word)" messages.
        /// </summary>
        [TestMethod]
        public void TestIgnoringWord()
        {
            // Create two clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();

            // Connect the clients.
            client1.Connect("localhost", 2000, "PLAY Harry");
            client2.Connect("localhost", 2000, "PLAY Hermione");

            // Check start messages.
            Thread.Sleep(50);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 Hermione", client1.msgString);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 Harry", client2.msgString);

            // Send words, and check IGNORING messages:

            client1.SendGoMessage("read");
            Thread.Sleep(100);
            Assert.AreEqual("IGNORING read", client1.msgString);

            client1.SendGoMessage("WORD ");
            Thread.Sleep(100);
            Assert.AreEqual("IGNORING WORD ", client1.msgString);
        }

        /// <summary>
        /// Tests score messages after playing words with large point values.
        /// </summary>
        [TestMethod]
        public void TestLargePointWords()
        {
            // Create two clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();

            // Connect the clients.
            client1.Connect("localhost", 2000, "PLAY Harry");
            client2.Connect("localhost", 2000, "PLAY Hermione");
            Thread.Sleep(100);

            // Send words and check scores.

            client1.SendGoMessage("WORD reloader");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 11 0", client1.msgString);
            Assert.AreEqual("SCORE 0 11", client2.msgString);

            client1.SendGoMessage("WORD RELOAD");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 14 0", client1.msgString);
            Assert.AreEqual("SCORE 0 14", client2.msgString);

            client2.SendGoMessage("WORD reload");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 11 0", client1.msgString);
            Assert.AreEqual("SCORE 0 11", client2.msgString);

            client2.SendGoMessage("WORD ReLoaDS");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 11 5", client1.msgString);
            Assert.AreEqual("SCORE 5 11", client2.msgString);

            // Make time run out, then check game summary messages.
            Thread.Sleep(6000);
            Assert.AreEqual("STOP 1 RELOADER 1 RELOADS 1 RELOAD 0 0", client1.msgString);
            Assert.AreEqual("STOP 1 RELOADS 1 RELOADER 1 RELOAD 0 0", client2.msgString);
        }

        /// <summary>
        /// Tests two games running at the same time.
        /// </summary>
        [TestMethod]
        public void TwoGames()
        {
            // Create four clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();
            BoggleClientModel client3 = new BoggleClientModel();
            BoggleClientModel client4 = new BoggleClientModel();

            // Connect the clients.
            client1.Connect("localhost", 2000, "PLAY Harry");
            client2.Connect("localhost", 2000, "PLAY Hermione");
            client3.Connect("localhost", 2000, "PLAY Ron");
            client4.Connect("localhost", 2000, "PLAY Crookshanks");
            Thread.Sleep(1000);

            // Send words.
            client2.SendGoMessage("WORD load");
            client3.SendGoMessage("WORD reads");
            client1.SendGoMessage("WORD invalid");
            client4.SendGoMessage("WORD read");
            client1.SendGoMessage("WORD loads");
            client3.SendGoMessage("WORD xxx");
            client4.SendGoMessage("WORD reads");

            // Check scores.
            Thread.Sleep(200);
            Assert.AreEqual("SCORE 1 1", client1.msgString);
            Assert.AreEqual("SCORE 1 1", client2.msgString);
            Assert.AreEqual("SCORE -1 1", client3.msgString);
            Assert.AreEqual("SCORE 1 -1", client4.msgString);

            // Make time run out, then check game summary messages.
            Thread.Sleep(6000);
            Assert.AreEqual("STOP 1 LOADS 1 LOAD 0 1 INVALID 0", client1.msgString);
            //Assert.AreEqual("STOP 1 LOAD 1 LOADS 0 0 1 INVALID", client2.msgString);
            Assert.AreEqual("STOP 0 1 READ 1 READS 1 XXX 0", client3.msgString);
            //Assert.AreEqual("STOP 1 READ 0 1 READS 0 1 XXX", client4.msgString);
        }
    }
}
