
//using System;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using SpreadsheetUtilities;
//using System.Collections.Generic;

//namespace FormulaTester
//{
//    ///<summary>
//    ///Basic test for PS3
//    ///Author James Yeates
//    ///</summary>
//    //---------------------------------------Basic Tests--------------------------------------------->
//    [TestClass]
//    public class UnitTest1
//    {
//        /// <summary>
//        /// This test see's if constructor works with legal vairables
//        /// </summary>
//        [TestMethod]
//        public void BasicConstructorTest1()
//        {
//            Formula f = new Formula("X + Y");
//            Formula f1 = new Formula("Xy + Y1");
//            Formula g = new Formula("X1a_ZzA - YAaZZ12_23A09");
//            Formula h = new Formula("_12zxj * _1_A");
//            Formula l = new Formula("X / Y");
//            Formula j = new Formula("5.00000123 / 5.1");
//            Formula k = new Formula("5e5 / 5.1");
//       }
//        /// <summary>
//        /// This test see's if constructor works with legal vairables
//        /// </summary>
//        [TestMethod]
//        public void BasicEvaluateTest1()
//        {
            
//            Formula f = new Formula("2 + 3");
//            Assert.AreEqual(5.0, f.Evaluate(s=>0.0));

          
//        }
//        /// <summary>
//        /// This test see's if constructor works with legal vairables
//        /// </summary>
//        [TestMethod]
//        public void FormaulEqualTestTest1()
//        {

//            Formula f = new Formula("2 + 3");
//            Formula g = new Formula("2.0 + 3.0");
//            Assert.AreEqual(f,g);


//        }
//        /// <summary>
//        /// This test see's  if e notatation is working
//        /// </summary>
//        [TestMethod]
//        public void BasicEvaluateTest1a()
//        {

//            Formula f = new Formula("2e5 + 3");
//            Assert.AreEqual(2e5 + 3, f.Evaluate(s => 0.0));


//        }
//        /// <summary>
//        /// This test see's if constructor works with legal vairables
//        /// </summary>
//        [TestMethod]
//        public void BasicEvaluateTest3()
//        {

//            Formula f = new Formula("(2 / 3) * 3");
//            Assert.AreEqual((2.0 / 3.0) * 3, f.Evaluate(s => 0.0));

//            Formula g = new Formula("(2.000001 / 3) * ((3*5+4-2)*(1+2*5)) *3");
//            Assert.AreEqual((2.000001 / 3) * ((3 * 5 + 4 - 2) * (1 + 2 * 5)) * 3, g.Evaluate(s => 0.0));


//        }
//        /// <summary>
//        /// test a large number
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest2()
//        {
//            //double d = 5e310;
//            Formula f = new Formula("x + 5E310");
       
//        }
//        /// <summary>
//        /// basic test that should fail
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest2b()
//        {
         
//            Formula f = new Formula("5 $ 5");

//        }
//        /// <summary>
//        /// basic test that should fail
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest2c()
//        {
//            //double d = 5e310;
//            Formula f = new Formula("$");

//        }
//        ///<summary>
//        /// basic test that should fail
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest2d()
//        {
//            //double d = 5e310;
//            Formula f = new Formula("aCbh%sd + 5");

//        }
//        /// <summary>
//        /// test a large number
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest2a()
//        {
//            //double d = 5e310;
//            Formula f = new Formula("5x310");

//        }
//        /// <summary>
//        /// Test empty formulas
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest3()
//        {

//            Formula f = new Formula("");
//        }
//        /// <summary>
//        /// Test illegal first expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest4()
//        {

//            Formula f = new Formula("+ 5 -2");
//        }
//        /// <summary>
//        /// Test illegal last expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest5()
//        {

//            Formula f = new Formula("5 -2 +");
//        }
//        /// <summary>
//        /// Test illegal expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest6()
//        {

//            Formula f = new Formula("((5+2)");
//        }

//        /// <summary>
//        /// Test illegal expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest7()
//        {

//            Formula f = new Formula("(5+2))");
//        }
//        /// <summary>
//        /// Test illegal expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest8()
//        {

//            Formula f = new Formula("(34 + -3)");
//        }
//        /// <summary>
//        /// Test illegal expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest10()
//        {

//            Formula f = new Formula("()2 +3");
//        }
//        /// <summary>
//        /// Test illegal expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest11()
//        {

//            Formula f = new Formula("3 -2(+3)");
//        }
//        /// <summary>
//        /// Test illegal expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void BasicConstructorTest9()
//        {

//            Formula f = new Formula("()34 +3)");
//        }
//        /// <summary>
//        /// Test basic legal expresions
//        /// </summary>
//        [TestMethod]
        
//        public void BasicConstructorTest12()
//        {

//            Formula f = new Formula("(34 +3)");
//            Formula g = new Formula("3*(34 +3)");
//            Formula h = new Formula("3*(34 +3)/2");
//            Formula i = new Formula("(1-3) + 3*(34 +3)/2");
//        }
//        /// <summary>
//        /// Test building many formulas
//        /// </summary>
//        [TestMethod]
//        public void BasicStressTest1()
//        {
//            for (int i = 0; i < 1000; i++)
//            {
                
//                Formula f = new Formula("(34 +3)");
//                Formula g = new Formula("3*(34 +3)");
//                Formula h = new Formula("3*(34 +3)/2");
//                Formula j = new Formula("(1-3) + 3*(34 +3)/2");
//            }
//        }
//        /// <summary>
//        /// 
//        /// 5.a is illegal expresion
//        /// </summary>
//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]
//        public void ConstructorErrorTest2()
//        {
//            //regrex is not  seeing 5.a as one token instead it splits it up
//            //when it hits a char (a) sees this as 2 + 5. a
//            Formula g = new Formula("2+ 5.a");
//        }
//        /// <summary>
//        /// should fail 2x is illegal
//        /// 
//        /// </summary>

//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]

//        public void ConstructorErrorTest3()
//        {
//            //regrex is not  seeing 2x as one token instead it splits it up
//            //to "2 x"
          
//            Formula g = new Formula("2+ 2X");
//        }
//        /// <summary>
//        /// This test tests the ToString method
//        /// 
//        /// </summary>

//        [TestMethod]
      
//        public void BasicToStringTest1()
//        {
//            string s = "x+y";
//            string s1 = "X+Y";
      
//            Formula i = new Formula(s);
//            Formula f = new Formula(s, Capitalize, ReturnTrue);
//            Formula h = new Formula(s, t => t.ToUpper(), t => true);
//            Assert.AreEqual(s1, f.ToString());
//            Assert.AreEqual(s1, h.ToString());
//            Assert.AreEqual(s, i.ToString());
//        }

//        /// <summary>
//        /// This test tests the ToString method
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void BasicToStringTest2()
//        {
//            string s = "5E-05";
    
//            Formula i = new Formula("5e-5");
//            Assert.AreEqual(s, i.ToString());
          
//        }
//        /// <summary>
//        /// This test tests the ToString method
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void BasicToStringTest3()
//        {
//            string s = "5.001";

//            Formula i = new Formula("5.001");
//            Assert.AreEqual(s, i.ToString());

//            string s1 = "5+5";

//            Formula i1 = new Formula("5.00000000 + 5.0");
//            Assert.AreEqual(s1, i1.ToString());

//            string s2 = "5";

//            Formula i2 = new Formula("5.000000000000000000001");
//            Assert.AreEqual(s2, i2.ToString());

//        }

//        /// <summary>
//        /// This test numbers with lots of decimal places
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void LongTest3()
//        {
//            Formula i2 = new Formula("5.000000000000000000001 +5.000000000000000000001 ");
//            Assert.AreEqual(10.0, i2.Evaluate(x=>0));

//        }
//        /// <summary>
//        /// Basic test to test .Equals()
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void BasicEqualsTest1()
//        {
//            string s = "x+y";
//            string t = "x+y";
//            Formula nullFormula = null;
//            Formula xplusy = new Formula(s);
//            Formula XplusY = new Formula(s, x=>x.ToUpper(), x=>true);
//            Formula xplusy1 = new Formula(t);
//            Formula XplusY1 = new Formula(t, x => x.ToUpper(), x => true);
           
            
//            Assert.IsFalse(xplusy.Equals(XplusY));
//            Assert.IsFalse(xplusy.Equals(nullFormula));

//            Assert.IsTrue(xplusy.Equals(xplusy));

//            Assert.IsTrue(xplusy.Equals(xplusy1));
//            Assert.IsTrue(XplusY.Equals(XplusY1));
      
//        }


//        /// <summary>
//        /// Basic test to test ==
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void BasicEqualsEqualsTest1()
//        {
//            string s = "x+y";
//            string t = "x+y";
//            Formula nullFormula = null;
//            Formula nullFormula1 = null;
//            Formula xplusy = new Formula(s);
//            Formula XplusY = new Formula(s, x => x.ToUpper(), x => true);
//            Formula xplusy1 = new Formula(t);
//            Formula XplusY1 = new Formula(t, x => x.ToUpper(), x => true);


//            Assert.IsFalse(xplusy == XplusY );
//            Assert.IsFalse(xplusy == nullFormula);

//            Assert.IsTrue(xplusy == xplusy);
//            Assert.IsTrue(nullFormula == nullFormula1);

//            Assert.IsTrue(xplusy == xplusy1 );
//            Assert.IsTrue(XplusY == XplusY1 );

//        }
//        /// <summary>
//        /// Basic test to test !=
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void BasicNotEqualsTest1()
//        {
//            string s = "x+y";
//            string t = "x+y";
//            Formula nullFormula = null;
//            Formula nullFormula1 = null;
//            Formula xplusy = new Formula(s);
//            Formula XplusY = new Formula(s, x => x.ToUpper(), x => true);
//            Formula xplusy1 = new Formula(t);
//            Formula XplusY1 = new Formula(t, x => x.ToUpper(), x => true);


//            Assert.IsTrue(xplusy != XplusY);
//            Assert.IsTrue(xplusy != nullFormula);

//            Assert.IsFalse(xplusy != xplusy);

//            Assert.IsFalse(nullFormula != nullFormula1);

//            Assert.IsFalse(xplusy != xplusy1);
//            Assert.IsFalse(XplusY != XplusY1);

//        }

//        /// <summary>
//        /// Divison by zero test
//        /// </summary>

//        [TestMethod]

//        public void divisionByZeroTest1()
//        {
//            //x holds 2 X holds 4
//            Formula x = new Formula("x/0");
//            FormulaError error = (FormulaError)x.Evaluate(s => 0);
           
//            Assert.AreEqual(x.ToString() + " Is not valid: division by 0",error.Reason );
//           // Assert.IsTrue(str.Equals(error.Reason));
//        }

//        /// <summary>
//        /// This test tests the ToString method
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void GetVariablesTest1()
//        {
  
//            HashSet<string> correctSet= new HashSet<string>();
//            correctSet.Add("x34");
//            correctSet.Add("_y");

//            Formula i = new Formula("x34 + _y +23 -3");
//            HashSet<string> testSet = (HashSet<string>) i.GetVariables();
//            foreach (string str in testSet)
//                System.Diagnostics.Debug.WriteLine(str);


//            Assert.IsTrue(correctSet.SetEquals(testSet));

//        }

//        /// <summary>
//        /// HashCode test, test to see if same object are equal
//        /// </summary>
//        [TestMethod]
        
//        public void HashTest1()
//        {
//            Formula f = new Formula("5-2");
//            Formula g = new Formula("5-2");

//            Assert.AreEqual(f.GetHashCode(), g.GetHashCode());
//        }

//        /// <summary>
//        /// HashCode test, test to see if same object are equal
//        /// </summary>
//        [TestMethod]

//        public void HashTest2()
//        {
//            Formula f = new Formula("5-2");
//            Formula g = new Formula("5-1");
//            Assert.AreNotEqual(f.GetHashCode(), g.GetHashCode());
//        }


//        //------------------------------delagetes test-------------------------------------------->
        
//        /// <summary>
//        /// This test seed if normalize is working
//        /// 
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void NormailizeTest1()
//        {
//            //x holds 2 X holds 4
//            Formula lowerCase = new Formula("x+2");
//            Formula upperCase = new Formula("x+2", s =>s.ToUpper(),s=>true);
//            Assert.AreEqual(4.0, lowerCase.Evaluate(VarTable));
//            Assert.AreEqual(6.0, upperCase.Evaluate(VarTable));

//        }

//        /// <summary>
//        /// This test seed if normalize is working
//        /// 
//        /// 
//        /// </summary>

//        [TestMethod]

//        public void VariableNotFoundTest1()
//        {
//            //x holds 2 X holds 4
           
//            Formula upperCase = new Formula("y+2", s => s.ToUpper(), s => true);
//            IEnumerable<string> set = upperCase.GetVariables();
//           foreach(string str in set)
//               System.Diagnostics.Debug.WriteLine(str);
//           FormulaError error = (FormulaError)upperCase.Evaluate(VarTable1);

//            Assert.AreEqual("Y+2 Is not valid: Y Variable not found", error.Reason);
          

//        }

//        ///<summary>
//        /// This test see if validate is working
//        ///</summary>

//        [TestMethod]
//        [ExpectedException(typeof(FormulaFormatException))]

//        public void ValidateTest1()
//        {
//            Formula x = new Formula("x+2", s => s.ToUpper(), s => false);
//        }
//        /// <summary>
//        /// 
//        /// A simple Normilize delagate
//        /// </summary>
//        /// <param name="s">string</param>
//        /// <returns>string</returns>
//         public string Capitalize(string s)
//        {
//            return s.ToUpper();
//        }
//         /// <summary>
//         /// 
//         /// A simple Validate delagate
//         /// </summary>
//         /// <param name="s">string</param>
//        public bool ReturnTrue(string s)
//        {
//            return true;
//        }
//        /// <summary>
//        /// 
//        /// simple test table
//        /// </summary>
//        /// <param name="s"></param>
//        /// <returns></returns>
//        public double VarTable(string s)
//        {

//            if (s == "x")
//                return 2.0;
//            if (s == "X")
//                return 4.0;
//            return 0.0;
//        }
//        /// <summary>
//        /// simple delagete
//        /// </summary>
//        /// <param name="s"></param>
//        /// <returns></returns>
 

//        public double VarTable1(string s)
//        {
//            IDictionary<string, double> testTable = new Dictionary<string, double>();
//            testTable.Add("x", 2);
//            testTable.Add("X", 4);

//            return testTable[s];

//        }
//    }
//}
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using SpreadsheetUtilities;

namespace GradingTests
{
    [TestClass]
    public class GradingTests
    {


        // Simple tests that return FormulaErrors
        [TestMethod()]
        public void Test16()
        {
            Formula f = new Formula("2+X1");
            Assert.IsInstanceOfType(f.Evaluate(s => { throw new ArgumentException("Unknown variable"); }), typeof(FormulaError));
        }

        [TestMethod()]
        public void Test17()
        {
            Formula f = new Formula("5/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        [TestMethod()]
        public void Test18()
        {
            Formula f = new Formula("(5 + X1) / (X1 - 3)");
            Assert.IsInstanceOfType(f.Evaluate(s => 3), typeof(FormulaError));
        }


        // Tests of syntax errors detected by the constructor
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test19()
        {
            Formula f = new Formula("+");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test20()
        {
            Formula f = new Formula("2+5+");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test21()
        {
            Formula f = new Formula("2+5*7)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test22()
        {
            Formula f = new Formula("((3+5*7)");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test23()
        {
            Formula f = new Formula("5x");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test24()
        {
            Formula f = new Formula("5+5x");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test25()
        {
            Formula f = new Formula("5+7+(5)8");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test26()
        {
            Formula f = new Formula("5 5");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test27()
        {
            Formula f = new Formula("5 + + 3");
        }

        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test28()
        {
            Formula f = new Formula("");
        }

        // Some more complicated formula evaluations
        [TestMethod()]
        public void Test29()
        {
            Formula f = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            Assert.AreEqual(5.14285714285714, (double)f.Evaluate(s => (s == "x7") ? 1 : 4), 1e-9);
        }

        [TestMethod()]
        public void Test30()
        {
            Formula f = new Formula("x1+(x2+(x3+(x4+(x5+x6))))");
            Assert.AreEqual(6, (double)f.Evaluate(s => 1), 1e-9);
        }

        [TestMethod()]
        public void Test31()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            Assert.AreEqual(12, (double)f.Evaluate(s => 2), 1e-9);
        }

        [TestMethod()]
        public void Test32()
        {
            Formula f = new Formula("a4-a4*a4/a4");
            Assert.AreEqual(0, (double)f.Evaluate(s => 3), 1e-9);
        }

        // Test of the Equals method
        [TestMethod()]
        public void Test33()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula("X1+X2");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test34()
        {
            Formula f1 = new Formula("X1+X2");
            Formula f2 = new Formula(" X1  +  X2   ");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test35()
        {
            Formula f1 = new Formula("2+X1*3.00");
            Formula f2 = new Formula("2.00+X1*3.0");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test36()
        {
            Formula f1 = new Formula("1e-2 + X5 + 17.00 * 19 ");
            Formula f2 = new Formula("   0.0100  +     X5+ 17 * 19.00000 ");
            Assert.IsTrue(f1.Equals(f2));
        }

        [TestMethod()]
        public void Test37()
        {
            Formula f = new Formula("2");
            Assert.IsFalse(f.Equals(null));
            Assert.IsFalse(f.Equals(""));
        }


        // Tests of == operator
        [TestMethod()]
        public void Test38()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsTrue(f1 == f2);
        }

        [TestMethod()]
        public void Test39()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsFalse(f1 == f2);
        }

        [TestMethod()]
        public void Test40()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(null == f1);
            Assert.IsFalse(f1 == null);
            Assert.IsTrue(f1 == f2);
        }


        // Tests of != operator
        [TestMethod()]
        public void Test41()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("2");
            Assert.IsFalse(f1 != f2);
        }

        [TestMethod()]
        public void Test42()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("5");
            Assert.IsTrue(f1 != f2);
        }


        // Test of ToString method
        [TestMethod()]
        public void Test43()
        {
            Formula f = new Formula("2*5");
            Assert.IsTrue(f.Equals(new Formula(f.ToString())));
        }


        // Tests of GetHashCode method
        [TestMethod()]
        public void Test44()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("2*5");
            Assert.IsTrue(f1.GetHashCode() == f2.GetHashCode());
        }

        [TestMethod()]
        public void Test45()
        {
            Formula f1 = new Formula("2*5");
            Formula f2 = new Formula("3/8*2+(7)");
            Assert.IsTrue(f1.GetHashCode() != f2.GetHashCode());
        }


        // Tests of GetVariables method
        [TestMethod()]
        public void Test46()
        {
            Formula f = new Formula("2*5");
            Assert.IsFalse(f.GetVariables().GetEnumerator().MoveNext());
        }

        [TestMethod()]
        public void Test47()
        {
            Formula f = new Formula("2*X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod()]
        public void Test48()
        {
            Formula f = new Formula("2*X2+Y3");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "Y3", "X2" };
            Assert.AreEqual(actual.Count, 2);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod()]
        public void Test49()
        {
            Formula f = new Formula("2*X2+X2");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X2" };
            Assert.AreEqual(actual.Count, 1);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        [TestMethod()]
        public void Test50()
        {
            Formula f = new Formula("X1+Y2*X3*Y2+Z7+X1/Z8");
            List<string> actual = new List<string>(f.GetVariables());
            HashSet<string> expected = new HashSet<string>() { "X1", "Y2", "X3", "Z7", "Z8" };
            Assert.AreEqual(actual.Count, 5);
            Assert.IsTrue(expected.SetEquals(actual));
        }

        // Tests to make sure there can be more than one formula at a time
        [TestMethod()]
        public void Test51a()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51b()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51c()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51d()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        [TestMethod()]
        public void Test51e()
        {
            Formula f1 = new Formula("2");
            Formula f2 = new Formula("3");
            Assert.IsTrue(f1.ToString().IndexOf("2") >= 0);
            Assert.IsTrue(f2.ToString().IndexOf("3") >= 0);
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52a()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52b()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52c()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52d()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Stress test for constructor
        [TestMethod()]
        public void Test52e()
        {
            Formula f = new Formula("(((((2+3*X1)/(7e-5+X2-X4))*X5+.0005e+92)-8.2)*3.14159) * ((x2+3.1)-.00000000008)");
        }

        // Test for normalizing variables
        [TestMethod()]
        public void Test53()
        {
            Formula f = new Formula("2+X1", s => "" + s + "_", s => true);
            HashSet<string> variables = new HashSet<string> { "X1_" };
            
            Assert.IsTrue(variables.SetEquals(f.GetVariables()));
        }



    }
}
