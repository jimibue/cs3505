// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.)
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {

        private Dictionary<string, HashSet<string>> dependentDict;
        private Dictionary<string, HashSet<string>> dependeeDict;
        private int _size;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            _size = 0;
            dependeeDict = new Dictionary<string, HashSet<string>>();
            dependentDict = new Dictionary<string, HashSet<string>>();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            //this is all this method should do 
            //try to keep track of size outside of here
            get { return _size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            //what to do if "a" is not there

            get
            {
                HashSet<string> tempSet;
                dependeeDict.TryGetValue(s, out tempSet);
                if (tempSet != null)
                    return tempSet.Count;
                return 0;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            return dependentDict.ContainsKey(s);
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return dependeeDict.ContainsKey(s);
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (dependentDict.ContainsKey(s))
                return dependentDict[s];
            return new HashSet<string>();
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (dependeeDict.ContainsKey(s))
                return dependeeDict[s];
            return new HashSet<string>();
        }


        /// <summary>
        /// Adds the ordered pair (s,t), if it doesn't exist
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void AddDependency(string s, string t)
        {
            if (s == null || s == "" || t == null || t == "") { return; }
            //throw new ArgumentException("null or empty strings not allowed");

            // if(s.Equals(t))
            //   throw new ArgumentException(s + " " + t + " are the same this causes self-dependency");

            //if s is in check for t
            _size += AddToDependencyDicts(s, t, dependentDict);

            AddToDependencyDicts(t, s, dependeeDict);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (s == null || t == null) { return; }
            //throw new ArgumentException("null value");
            RemoveFromDependencyDic(s, t, dependentDict);
            RemoveFromDependencyDic(t, s, dependeeDict);
        }



        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (s == null || newDependents == null) { return; }
            // throw new ArgumentException("null value");

            //remove all if key exists
            if (dependentDict.ContainsKey(s))
            {
                string[] templist = dependentDict[s].ToArray<string>();
                for (int i = 0; i < templist.Length; i++)
                    RemoveDependency(s, templist[i]);
            }
            //add all new order pairs(s, newDependents[i])
            foreach (string str in newDependents)
                AddDependency(s, str);


        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            if (s == null || newDependees == null) { return; }
            //throw new ArgumentException("null value");

            //remove all if key exists
            if (dependeeDict.ContainsKey(s))
            {
                string[] templist = dependeeDict[s].ToArray<string>();

                for (int i = 0; i < templist.Length; i++)
                    RemoveDependency(templist[i], s);
            }
            //add all new order pairs(s, newDependents[i])
            foreach (string str in newDependees)
                AddDependency(str, s);

        }
        /// <summary>
        /// Helper method that is used to add given inputs to dependee and dependent dicts.
        /// </summary>
        /// <param name="s">key</param>
        /// <param name="t"> value</param>
        /// <param name="dict">dictonary to add key and value to </param>
        /// <returns>1 if an item was added 0 if not</returns>
        private int AddToDependencyDicts(string s, string t, Dictionary<string, HashSet<string>> dict)
        {
            //if s is already a key in the dict
            if (dict.ContainsKey(s))
            {
                //redundent? but need to check for correct size
                if (dict[s].Contains(t))
                    return 0;

                dict[s].Add(t);
                return 1;
            }
            // s is not key add it to dict
            dict.Add(s, new HashSet<string>());
            dict[s].Add(t);
            return 1;

        }

        /// <summary>
        /// This helper removes dependencies, was added to reduce code reuse
        /// </summary>
        /// <param name="s">key</param>
        /// <param name="t">value</param>
        /// <param name="dict">dictoinary</param>
        private void RemoveFromDependencyDic(string s, string t, Dictionary<string, HashSet<string>> dict)
        {
            if (dict.ContainsKey(s) && dict[s].Contains(t))
            {
                // if dictonary only has one value reomve the whole kvp
                if (dict[s].Count() == 1)
                    dict.Remove(s);
                //just remove t from the hash set
                else
                    dict[s].Remove(t);

                if (dict.Equals(dependentDict))
                {
                    _size--;
                }

                //!!!! this could potentaily be a problem down the road!!!!!!
                //implies t has dependents not sure if I need to check this
                //probaly will for at least size
                //if (dict.ContainsKey(t) && dict[t].Count() > 0)
                //{ }
                //need to do this to keep track of _size
            }
        }


    }







}


