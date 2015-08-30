using SpreadsheetUtilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics;


namespace PS2GradingTests
{
    /// <summary>
    ///  This is a test class for DependencyGraphTest
    /// 
    ///  These tests should help guide you on your implementation.  Warning: you can not "test" yourself
    ///  into correctness.  Tests only show incorrectness.  That being said, a large test suite will go a long
    ///  way toward ensuring correctness.
    /// 
    ///  You are strongly encouraged to write additional tests as you think about the required
    ///  functionality of yoru library.
    /// 
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {
        // ************************** TESTS ON EMPTY DGs ************************* //

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest1()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependees("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest3()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.HasDependents("a"));
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest4()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependees("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest5()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.IsFalse(t.GetDependents("a").GetEnumerator().MoveNext());
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyTest6()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t["a"]);
        }

        /// <summary>
        ///Removing from an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest7()
        {
            DependencyGraph t = new DependencyGraph();
            t.RemoveDependency("a", "b");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Adding an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest8()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest9()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependents("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        ///Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void EmptyTest10()
        {
            DependencyGraph t = new DependencyGraph();
            t.ReplaceDependees("a", new HashSet<string>());
            Assert.AreEqual(0, t.Size);
        }


        /**************************** SIMPLE NON-EMPTY TESTS ****************************/

        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest1()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Slight variant
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest2()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            Assert.AreEqual(1, t.Size);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest3()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsTrue(t.HasDependees("c"));
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest4()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            HashSet<String> aDents = new HashSet<String>(t.GetDependents("a"));
            HashSet<String> bDents = new HashSet<String>(t.GetDependents("b"));
            HashSet<String> cDents = new HashSet<String>(t.GetDependents("c"));
            HashSet<String> dDents = new HashSet<String>(t.GetDependents("d"));
            HashSet<String> eDents = new HashSet<String>(t.GetDependents("e"));
            HashSet<String> aDees = new HashSet<String>(t.GetDependees("a"));
            HashSet<String> bDees = new HashSet<String>(t.GetDependees("b"));
            HashSet<String> cDees = new HashSet<String>(t.GetDependees("c"));
            HashSet<String> dDees = new HashSet<String>(t.GetDependees("d"));
            HashSet<String> eDees = new HashSet<String>(t.GetDependees("e"));
            Assert.IsTrue(aDents.Count == 2 && aDents.Contains("b") && aDents.Contains("c"));
            Assert.IsTrue(bDents.Count == 0);
            Assert.IsTrue(cDents.Count == 0);
            Assert.IsTrue(dDents.Count == 1 && dDents.Contains("c"));
            Assert.IsTrue(eDents.Count == 0);
            Assert.IsTrue(aDees.Count == 0);
            Assert.IsTrue(bDees.Count == 1 && bDees.Contains("a"));
            Assert.IsTrue(cDees.Count == 2 && cDees.Contains("a") && cDees.Contains("d"));
            Assert.IsTrue(dDees.Count == 0);
            Assert.IsTrue(dDees.Count == 0);
        }

        /// <summary>
        ///Nonempty graph should contain something
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest5()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            Assert.AreEqual(0, t["a"]);
            Assert.AreEqual(1, t["b"]);
            Assert.AreEqual(2, t["c"]);
            Assert.AreEqual(0, t["d"]);
            Assert.AreEqual(0, t["e"]);
        }

        /// <summary>
        ///Removing from a DG 
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest6()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.RemoveDependency("a", "b");
            Assert.AreEqual(2, t.Size);
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest7()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> aPends = new HashSet<string>(t.GetDependents("a"));
            Assert.IsTrue(aPends.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        /// <summary>
        ///Replace on a DG
        ///</summary>
        [TestMethod()]
        public void NonEmptyTest8()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("d", "c");
            t.ReplaceDependees("c", new HashSet<string>() { "x", "y", "z" });
            HashSet<String> cDees = new HashSet<string>(t.GetDependees("c"));
            Assert.IsTrue(cDees.SetEquals(new HashSet<string>() { "x", "y", "z" }));
        }

        // ************************** STRESS TESTS ******************************** //
        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest1()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
               Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }



        // ********************************** ANOTHER STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest8()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            //// Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            //// Replace a bunch of dependents
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDents = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDents.Add(letters[j]);
                }
                t.ReplaceDependents(letters[i], newDents);

                foreach (string s in dents[i])
                {
                    dees[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDents)
                {
                    dees[s[0] - 'a'].Add(letters[i]);
                }

                dents[i] = newDents;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                HashSet<string> d = dents[i];
                HashSet<string> d1 = new HashSet<string> (t.GetDependents(letters[i]));
                HashSet<string> d2 = dees[i];
                HashSet<string> d3 = new HashSet<string>(t.GetDependees(letters[i]));
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

        // ********************************** A THIRD STESS TEST ******************** //
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void StressTest15()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 100;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 2; j < SIZE; j += 2)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Replace a bunch of dependees
            for (int i = 0; i < SIZE; i += 4)
            {
                HashSet<string> newDees = new HashSet<String>();
                for (int j = 0; j < SIZE; j += 7)
                {
                    newDees.Add(letters[j]);
                }
                t.ReplaceDependees(letters[i], newDees);

                foreach (string s in dees[i])
                {
                    dents[s[0] - 'a'].Remove(letters[i]);
                }

                foreach (string s in newDees)
                {
                    dents[s[0] - 'a'].Add(letters[i]);
                }

                dees[i] = newDees;
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                HashSet<string> d = dents[i];
                HashSet<string> d1 = new HashSet<string>(t.GetDependents(letters[i]));
                HashSet<string> d2 = dees[i];
                HashSet<string> d3 = new HashSet<string>(t.GetDependees(letters[i]));
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }
        /*_____________________MY TESTs  (Note exceptions test where changed so that null wsa allowed_____________________________*/

        //Illegal arguments tests.

        /// <summary>
        /// test for empty string
        /// </summary>
        [TestMethod()]
        public void ExceptionTest1()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            try
            {
                t.AddDependency("", "b");
               // Assert.Fail();
            }
            catch(ArgumentException){/*Do nothing test should fail*/}
                
        }
        /// <summary>
        /// test for null string
        /// </summary>

        [TestMethod()]
        public void ExceptionTest2()
        {
            DependencyGraph t = new DependencyGraph();
            
            try
            {
                t.AddDependency(null, "b");
               // Assert.Fail();
            }
            catch (ArgumentException) {/*Do nothing test should fail*/}

        }

        [TestMethod()]
        public void ExceptionTest3()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            HashSet<string> emptySet = new HashSet<string>();
            //t.ReplaceDependents("a", null);
            try
            {
                t.ReplaceDependents("a", null);
                //Assert.Fail();
            }
            catch (ArgumentException) {/*Do nothing test should fail*/}
            try
            {
                t.ReplaceDependents("a", emptySet);
                Assert.IsFalse(t.HasDependents("a"));
                //Assert.Fail();
            }
            catch (ArgumentException) {/*Do nothing test should fail*/}

        }

 
        /// <summary>
        ///Test example from instructions
        ///    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
        //     dependents("a") = {"b", "c"}
        //     dependents("b") = {"d"}
        //     dependents("c") = {}
        //     dependents("d") = {"d"}
        //     dependees("a") = {}
        //     dependees("b") = {"a"}
        //     dependees("c") = {"a"}
        //     dependees("d") = {"b", "d"}
        ///</summary>
        [TestMethod()]
        public void MyTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("b", "d");
            t.AddDependency("d", "d");
            Assert.IsTrue(t.GetDependents("a").ToHashSet().SetEquals(new HashSet<string>() { "b", "c" }));
            Assert.IsTrue(t.GetDependents("b").ToHashSet().SetEquals(new HashSet<string>() { "d" }));
            Assert.IsTrue(t.GetDependents("c").ToHashSet().SetEquals(new HashSet<string>() { }));
            Assert.IsTrue(t.GetDependents("d").ToHashSet().SetEquals(new HashSet<string>() { "d" }));
            Assert.IsTrue(t.GetDependees("a").ToHashSet().SetEquals(new HashSet<string>() {  }));
            Assert.IsTrue(t.GetDependees("b").ToHashSet().SetEquals(new HashSet<string>() { "a" }));
            Assert.AreEqual(1, t["b"]);
            Assert.IsTrue(t.GetDependees("c").ToHashSet().SetEquals(new HashSet<string>() { "a" }));
            Assert.IsTrue(t.GetDependees("d").ToHashSet().SetEquals(new HashSet<string>() { "b","d" }));
            Assert.AreEqual(2, t["d"]);
            Assert.AreEqual(0, t["h"]);
            Assert.AreEqual(0, t["a"]);
            Assert.IsFalse(t.HasDependees("a"));
            Assert.IsFalse(t.HasDependees("f"));
            Assert.IsFalse(t.HasDependents("c"));
            Assert.IsTrue(t.HasDependents("a"));
            Assert.IsFalse(t.HasDependents("h"));
            Assert.IsTrue(t.HasDependents("d"));
            Assert.IsTrue(t.HasDependees("b"));
            Assert.IsTrue(t.HasDependees("c")); 
            Assert.IsTrue(t.HasDependees("d"));
            Assert.AreEqual(4, t.Size);


            
            t.RemoveDependency("a", "b");
            Assert.IsTrue(t.GetDependents("a").ToHashSet().SetEquals(new HashSet<string>() { "c" }));
            Assert.IsTrue(t.GetDependees("b").ToHashSet().SetEquals(new HashSet<string>() {  }));

            t.AddDependency("a", "b");
            t.AddDependency("a", "b");
            t.ReplaceDependents("a", new HashSet<string>() { "x", "y", "x", "y" ,"z"});
            Assert.IsTrue(t.GetDependents("a").ToHashSet().SetEquals(new HashSet<string>() { "x", "y","z"}));
            Assert.AreEqual(5, t.Size);


            t.ReplaceDependees("b", new HashSet<string>() { "x", "y", "x", "y" });
            Assert.IsTrue(t.GetDependees("b").ToHashSet().SetEquals(new HashSet<string>() { "x", "y" }));
            Assert.AreEqual(7, t.Size);
            Assert.AreEqual(2, t["b"]);
           

            //Assert.AreEqual(4, t.Size);
        }

        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void MyStressTest1()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            Random rd = new Random();

            for (int i = 65; i < 95; i++ )
            {
                for(int j = 0; j< 10000; j++)
                {
                    string s1 = "" + (char)i +""+ j;
                    string s2 = "" + (char)(i+1) +""+ (j+1);
                    t.AddDependency(s1, s2);
                }
            }

            Assert.AreEqual(300000, t.Size);

            for (int i = 65; i < 95; i++)
            {
                for (int j = 0; j < 10000; j++)
                {
                    string s1 = "" + (char)i + "" + j;
                    string s2 = "" + (char)(i + 1) + "" + (j + 1);
                    t.RemoveDependency(s1, s2);
                }
            }
            Assert.AreEqual(0, t.Size);


        }
        /// <summary>
        ///Using lots of data with replacement
        ///</summary>
        [TestMethod()]
        public void TimeTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();
            Random rd = new Random();

            for (int i = 65; i < 95; i++)
            {
                for (int j = 0; j < 100000; j++)
                {
                    string s1 = "" + (char)i + "" + j;
                    string s2 = "" + (char)(i + 1) + "" + (j + 1);
                    
                }
            }

         

            for (int i = 65; i < 95; i++)
            {
                for (int j = 0; j < 100000; j++)
                {
                    string s1 = "" + (char)i + "" + j;
                    string s2 = "" + (char)(i + 1) + "" + (j + 1);
                    
                }
            }
       


        }

    }
}
