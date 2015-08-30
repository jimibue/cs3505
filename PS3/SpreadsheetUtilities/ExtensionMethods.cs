using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// This class contains extensions to help determine what format a string is.  For eample is 
    /// it a variable, number, letter, etc
    ///Author James Yeates
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// 
        /// determines whether the token is a valid variable starts with _ or letter
        /// follwed by _ letters number or nothing
        /// </summary>
        /// <param name="str">token</param>
        /// <returns>is a valid variable</returns>
        public static bool isVariable(this string str)
        {
            //if (Regex.IsMatch(str, "[a-zA-Z_][a-zA-Z0-9_]*"))
            //    return true;
            //return false;
            if (!(str[0] == '_' || str[0] >= 65 && str[0] <= 122))
                return false;
            else
            {
                for (int i = 1; i < str.Length; i++)
                {

                    if (!(str[i] == '_' || str[i] >= 65 && str[i] <= 122 || str[i] >= 48 && str[i] <= 57))
                        return false;

                }
            }
            return true;
        }
        /// <summary>
        /// determine if this is a sing letter A -z or a (,),*,+,-/
       ///</summary>
        /// <param name="str">token</param>
        /// <returns> is a single letter or valid operator</returns>
  public static bool isOpORLetter(this string str)
        {
            if (str.Length == 1)
            {
                if (str[0] == '+' || str[0] == '-' || str[0] == '*' || str[0] == '/' || str[0] == ')' || str[0] == '('
                    || str[0] == '_' || str[0] >= 65 && str[0] <= 122)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// Determine wheteher the token is a number
        /// </summary>
        /// <param name="str">token</param>
        /// <returns>is token a number</returns>

        public static bool isNumber(this string str)
        {
            double temp;
            if (double.TryParse(str, out temp))
                return true;
            return false;
        }
        /// <summary>
        /// is this a ( or ) -- true false
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        
        public static bool isParenthesis(this char c)
        {
            if(c =='('|| c == ')')
                return true;
            return false;
        }

        public static bool isOperator(this string c)
        {
            if (c == "*" || c == "/" ||c =="+" || c =="-")
                return true;
            return false;
        }
    
    }

}
