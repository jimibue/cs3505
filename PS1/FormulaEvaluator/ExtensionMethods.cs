
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace FormulaEvaluator
//{
//    ///<summary>
//    ///these Extensions help when parsing tokens to see what they are ie variable, number etc
//    ///Author James Yeates
//    ///</summary>
//    public  static class ExtensionMethods
//    {
//        /// <summary>
//        /// 
//        /// determines whether the token is a valid variable starts with _ or letter
//        /// follwed by _ letters number or nothing
//        /// </summary>
//        /// <param name="str">token</param>
//        /// <returns>is a valid variable</returns>
//        public static bool isVariable(this string str)
//        {
//            //if (Regex.IsMatch(str, "[a-zA-Z_][a-zA-Z0-9_]*"))
//            //    return true;
//            //return false;
//            if (!(str[0] == '_' || str[0] >= 65 && str[0] <= 122))
//                return false;
//            else
//            {
//                for (int i = 1; i < str.Length; i++)
//                {

//                    if (!(str[i] == '_' || str[i] >= 65 && str[i] <= 122 || str[i] >= 48 && str[i] <= 57))
//                        return false;

//                }
//            }
//            return true;
//        }
//    }
//}
