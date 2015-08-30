// Authors: James Yeates and Tyler Down

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Sockets;
using System.Net;
using BoggleClient;
using System.Threading;

namespace BoggleClientTest
{
    /// <summary>
    /// Unit tests for the BoggleClientModel class.
    /// </summary>
    [TestClass]
    public class BoggleClientTest1
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
            client1.Connect("localhost", 2000, "Harry");
            client2.Connect("localhost", 2000, "Hermione");

            // Check start messages.
            Thread.Sleep(100);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 HERMIONE", client1.msgString);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 HARRY", client2.msgString);

            // Send words, and check the score messages:

            client1.SendGoMessage("read");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 1 0", client1.msgString);
            Assert.AreEqual("SCORE 0 1", client2.msgString);

            client2.SendGoMessage("reads");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 1 2", client1.msgString);
            Assert.AreEqual("SCORE 2 1", client2.msgString);

            // Send a word already played by other player
            client2.SendGoMessage("read");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 0 2", client1.msgString);
            Assert.AreEqual("SCORE 2 0", client2.msgString);

            // Send an invalid word
            client2.SendGoMessage("asdfjkl");
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
            client1.Connect("localhost", 2000, "Harry");
            client2.Connect("localhost", 2000, "Hermione");
            Thread.Sleep(100);

            // Send words, and check the score messages:

            client1.SendGoMessage("asdfjkl");
            client1.SendGoMessage("re");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE -1 0", client1.msgString);
            Assert.AreEqual("SCORE 0 -1", client2.msgString);
        }

        /// <summary>
        /// Tests for ignoring messages when the client sends only whitespace.
        /// </summary>
        [TestMethod]
        public void TestIgnoringWord()
        {
            // Create two clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();

            // Connect the clients.
            client1.Connect("localhost", 2000, "Harry");
            client2.Connect("localhost", 2000, "Hermione");

            // Check start messages.
            Thread.Sleep(50);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 HERMIONE", client1.msgString);
            Assert.AreEqual("START TESRRDELEAOBSINM 5 HARRY", client2.msgString);

            // Send words, and check IGNORING messages:

            client1.SendGoMessage("");
            Thread.Sleep(100);
            Assert.AreEqual("IGNORING WORD ", client1.msgString);

            client1.SendGoMessage(" ");
            Thread.Sleep(100);
            Assert.AreEqual("IGNORING WORD  ", client1.msgString);
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
            client1.Connect("localhost", 2000, "Harry");
            client2.Connect("localhost", 2000, "Hermione");
            Thread.Sleep(100);

            // Send words and check scores.

            client1.SendGoMessage("reloader");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 11 0", client1.msgString);
            Assert.AreEqual("SCORE 0 11", client2.msgString);

            client1.SendGoMessage("RELOAD");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 14 0", client1.msgString);
            Assert.AreEqual("SCORE 0 14", client2.msgString);

            client2.SendGoMessage("reload");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 11 0", client1.msgString);
            Assert.AreEqual("SCORE 0 11", client2.msgString);

            client2.SendGoMessage("ReLoaDS");
            Thread.Sleep(100);
            Assert.AreEqual("SCORE 11 5", client1.msgString);
            Assert.AreEqual("SCORE 5 11", client2.msgString);

            // Make time run out, then check game summary messages.
            Thread.Sleep(6000);
            Assert.AreEqual("STOP 1 RELOADER 1 RELOADS 1 RELOAD 0 0", client1.msgString);
            Assert.AreEqual("STOP 1 RELOADS 1 RELOADER 1 RELOAD 0 0", client2.msgString);
        }

        /// <summary>
        /// Tests whether a terminated message is received after a player disconnects.
        /// </summary>
        [TestMethod]
        public void TestTerminated()
        {
            // Create two clients.
            BoggleClientModel client1 = new BoggleClientModel();
            BoggleClientModel client2 = new BoggleClientModel();

            // Connect the clients.
            client1.Connect("localhost", 2000, "Harry");
            client2.Connect("localhost", 2000, "Hermione");
            Thread.Sleep(100);

            client1.disconnect();
            Thread.Sleep(100);

            Assert.AreEqual("TERMINATED", client2.msgString);
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
            client1.Connect("localhost", 2000, "Harry");
            client2.Connect("localhost", 2000, "Hermione");
            client3.Connect("localhost", 2000, "Ron");
            client4.Connect("localhost", 2000, "Crookshanks");
            Thread.Sleep(1000);

            // Send words.
            client2.SendGoMessage("load");
            client3.SendGoMessage("reads");
            client1.SendGoMessage("invalid");
            client4.SendGoMessage("read");
            client1.SendGoMessage("loads");
            client3.SendGoMessage("xxx");
            client4.SendGoMessage("reads");

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
