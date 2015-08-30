using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetUtilities
{
    public static class ExtensionMethods
    {
        public static HashSet<string> ToHashSet (this IEnumerable<string> ie)
        {
            HashSet<string> temp = new HashSet<string>();
            temp.UnionWith(ie);
            return temp;
        }
    }
}
