﻿using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FormulaEvaluator;
using System.Diagnostics;

namespace Tester
{
    class Program
    {

        static void Main(string[] args)
        {
       
            test("(1/1+(1+(2 -3+5-2) ))", (1 / 1 + (1 + (2 - 3 + 5 - 2))), lookUpDel);
            test("a2 + a2", 2 + 2, lookUpDel);
            test("1/2", 1 / 2, lookUpDel);
            test("1/2 + 4", 1 / 2 + 4, lookUpDel);
            test("(1+1)*7", (1 + 1) * 7, lookUpDel);
            test("(1+1)*7", (1 + 1) * 7, lookUpDel);
            test("6 * (5 * 7)", 6 * (5 * 7), lookUpDel);
            test("(6 * (5 * 7))", (6 * (5 * 7)), lookUpDel);
            test("(6*(5*7))", (6 * (5 * 7)), lookUpDel);
            test("(3 + 5 * (4 + 6 - (5 * 7)))", (3 + 5 * (4 + 6 - (5 * 7))), lookUpDel);
            test("(6 * (3 + 5 * 7))", 6 * (3 + 5 * 7), lookUpDel);
            test("(6 - (3 + 5 * 7))", 6 - (3 + 5 * 7), lookUpDel);
            test("(6 * (7*(3 + 5 * 7)))", 6 * (7 * (3 + 5 * 7)), lookUpDel);
            test("(3+4)+7+ (5+(2+2)+1)", (3 + 4) + 7 + (5 + (2 + 2) + 1), lookUpDel);
            test("(1+1)+1+(2-3*(4-5)*(3+5*(4+6-(5*7)))*7)", (1 + 1) + 1 + (2 - 3 * (4 - 5) * (3 + 5 * (4 + 6 - (5 * 7))) * 7), lookUpDel);
            test("3 + 5 - (4 + 6 - 5 * 7)", (3 + 5 - (4 + 6 - 5 * 7)), lookUpDel);

            test("1-1", 1 - 1, lookUpDel);
            test("1/1", 1 / 1, lookUpDel);
            test("(1+1)*2 +1 + 3 + (1+(2*3)+4)", (1 + 1) * 2 + 1 + 3 + (1 + (2 * 3) + 4), lookUpDel);//fails
            test("(3+4)+7+ (5+(2*2)+1)", (3 + 4) + 7 + (5 + (2 * 2) + 1), lookUpDel);

            try
            {
                Console.WriteLine(Evaluator.Evaluate("ab+1", lookUpDel));
                throw new Exception();
            }
            catch (ArgumentException e) { }

            try
            {
                Console.WriteLine(Evaluator.Evaluate("1a+1", lookUpDel));
                throw new Exception();
            }
            catch (ArgumentException e) { }
            try
            {
                Console.WriteLine(Evaluator.Evaluate("aa1a+1", lookUpDel));
                throw new Exception();
            }
            catch (ArgumentException e) { }
            try
            {
                Console.WriteLine(Evaluator.Evaluate("$+1", lookUpDel));
                throw new Exception();
            }
            catch (ArgumentException e) { }
            // Evaluator.Evaluate("AzAzZa1234567890+1", lookUpDel);
            Evaluator.Evaluate("asdf645+1", lookUpDel);



            Console.WriteLine((1 + 1) + 1 + (2 - 3 * (4 - 5) * (3 + 5 * (4 + 6 - (5 * 7))) * 7));
            test("a2 + a2", 2 + 2, lookUpDel);
            test("(1+1)*7", (1 + 1) * 7, lookUpDel);


            test("6 * (5 * 7)", 6 * (5 * 7), lookUpDel);
            test("(6 * (5 * 7))", (6 * (5 * 7)), lookUpDel);
            test("(3 + 5 * (4 + 6 - (5 * 7)))", (3 + 5 * (4 + 6 - (5 * 7))), lookUpDel);
            test("((4 + 6 - (5 * 7)))", ((4 + 6 - (5 * 7))), lookUpDel);
            test("(1+1)*7", (1 + 1) * 7, lookUpDel);
            test("(1+2)+(11+12)", (1 + 2) + (11 + 12), lookUpDel);
            test("(1+2)+(5+(11*12))", (1 + 2) + (5 + (11 * 12)), lookUpDel);
            test("(1+2)+((11*12)+4)", (1 + 2) + ((11 * 12) + 4), lookUpDel);
            test("(1+2)+(5+(11*12)+4)", (1 + 2) + (5 + (11 * 12) + 4), lookUpDel);
            test("abzAZ1234567890+abzAZ1234567890", 10 + 10, lookUpDel);
            //test("2147483647 +1", 2147483647, lookUpDel);

            test("(1+1)*7", (1 + 1) * 7, lookUpDel);
            test("(1+1)*7", (1 + 1) * 7, lookUpDel);
            test("(1+1)+1+(2-3*(4-5)*(3+5*(4+6-(5*7)))*7)", (1 + 1) + 1 + (2 - 3 * (4 - 5) * (3 + 5 * (4 + 6 - (5 * 7))) * 7), lookUpDel);
            test("(1+1)*7+34 - 23 + (1+(2*3)+4)", (1 + 1) * 7 + 34 - 23 + (1 + (2 * 3) + 4), lookUpDel);
            test("(1+1)*7+34 - 23 + (1+(4-3)+4)", (1 + 1) * 7 + 34 - 23 + (1 + (4 - 3) + 4), lookUpDel);
            test("(1+1)*7+34 - 23 + (1+(4+3)+4)", (1 + 1) * 7 + 34 - 23 + (1 + (4 + 3) + 4), lookUpDel);
            test("(1+1)*7+34 - 23 + (1+4-3+4)", (1 + 1) * 7 + 34 - 23 + (1 + 4 - 3 + 4), lookUpDel);
            test("(1+1)*7+34 - 23 + (1+4-3+4)", (1 + 1) * 7 + 34 - 23 + (1 + 4 - 3 + 4), lookUpDel);
            test("(1+1)*7+34 - 23 + (a2-(1+4-3+4))", (1 + 1) * 7 + 34 - 23 + (2 - (1 + 4 - 3 + 4)), lookUpDel);
            test("(1+1)*7+34 - 23 + (a2-(1+4-3+4))", (1 + 1) * 7 + 34 - 23 + (2 - (1 + 4 - 3 + 4)), lookUpDel);
            test("(1/1+(1+(2 -3+5-2) ))*7+34 - 23 + (a2-(1+4-3+4))", (1 / 1 + (1 + (2 - 3 + 5 - 2))) * 7 + 34 - 23 + (2 - (1 + 4 - 3 + 4)), lookUpDel);
            test("(1/1+(1+(2 -3+5-2) ))*7+34 - 23 + (an1-(1+4-3+4))", (1 / 1 + (1 + (2 - 3 + 5 - 2))) * 7 + 34 - 23 + (-1 - (1 + 4 - 3 + 4)), lookUpDel);
            test("(1+1)*7/34 - 23 + (1+(2*3)+4)", (1 + 1) * 7 / 34 - 23 + (1 + (2 * 3) + 4), lookUpDel);
            test("(1+1)*7+34 - 23 + (1+(4-3)+4)", (1 + 1) * 7 + 34 - 23 + (1 + (4 - 3) + 4), lookUpDel);
            test("(3) +5", (3) + 5, lookUpDel);
            test("(((((3) +5))*8))", (3 + 5) * 8, lookUpDel);

            testFail("aaa323a", lookUpDel);
            testFail("a", lookUpDel);
            testFail("-(3+5) ", lookUpDel);
            testFail("(3/0) ", lookUpDel);
            testFail("(3+5)- ", lookUpDel);
            testFail("(3+5)* ", lookUpDel);
            testFail("*(3+5) ", lookUpDel);
            testFail("-3+5 ", lookUpDel);
            testFail("*3+5 ", lookUpDel);
            testFail("3+5() ", lookUpDel);
            testFail("3+5 + () ", lookUpDel);
            testFail("()3+5", lookUpDel);
            testFail("()3+5 ()", lookUpDel);
            testFail("3+(- 5", lookUpDel);
            testFail("3+() 5", lookUpDel);
            testFail("3+)- 5", lookUpDel);
            testFail("(3+) 5", lookUpDel);
            testFail("3+(()) 5", lookUpDel);
            testFail("3+(()) 5", lookUpDel);
            testFail("(9*+9) +9", lookUpDel);
            testFail("(9*9) 9", lookUpDel);
            testFail("(9*9)+= 9", lookUpDel);
            testFail("(9*9)+1 9", lookUpDel);
            testFail("((9*9)", lookUpDel);
            testFail("(9*9))", lookUpDel);
            testFail("(9/9)+ 0 = 2", lookUpDel);
           
            testFail("", lookUpDel);

            testFail("(9/0) +9", lookUpDel);


            test("(3-(3+5)) ", (3 - (3 + 5)), lookUpDel);





            Console.WriteLine("testing done");
            Console.ReadLine();
        }
        public static int getValue(string s)
        {

            IDictionary<string, int> dic = new Dictionary<string, int>();
            dic.Add("an1", -1);
            dic.Add("a1", 1);
            dic.Add("a2", 2);
            dic.Add("a3", 3);
            dic.Add("abzAZ1234567890", 10);


            int number;
            if (dic.TryGetValue(s, out number))
            {

                return number;
            }
            else
            {
                //error
            }
            return 3;
        }

        public static void test(string str, int testNumber, Evaluator.Lookup lookUpDel)
        {

            int testNumber1 = Evaluator.Evaluate(str, lookUpDel);
            if (testNumber != testNumber1)
            {

                Console.WriteLine("fail on expression " + str + " : expected " + testNumber + " got " + testNumber1);
                Console.ReadLine();

            }

        }

        public static void testFail(string str, Evaluator.Lookup lookUpDel)
        {

            try
            {
                Console.Write(Evaluator.Evaluate(str, lookUpDel) + " ");
                Console.WriteLine("failed  " + str + "should throw argumentexception");

            }
            catch (ArgumentException e) { }



        }

        private static void ReportError(string message)
        {
            StackFrame callStack = new StackFrame(1, true);
            Console.WriteLine("Error: " + message + ", File: " + callStack.GetFileName()
                 + ", Line: " + callStack.GetFileLineNumber());
        }
    }
    
}
