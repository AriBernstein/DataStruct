using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    public class Utils {
        private string sep = "\n-----\n";

        public String PrintIntArray(int[] arr) {
            int n = arr.Length;
            string s = "";
            for (int i = 0; i < (n-1); ++i)
                s = s + (arr[i] + ", ");
            s = s + arr[n-1] + sep;
            Console.WriteLine(s);
            return s;
        }

        public string PrintDataLocationDict(Dictionary<int, int> dict, String searchVal) {
            int n = dict.Count;
            searchVal = searchVal + " located in dimension ";
            string s = "";
            for (int i = 0; i < (n-1); ++i)
                s = s + searchVal + (i + 1) + " at: \t" + dict[i + 1] + '\n';
            s = s + searchVal + n + " at: \t" + dict[n] + sep;

            Console.WriteLine(s);
            return s;
        }

        public string PrintNodeArray(Node[] arr) {
            string s = "";
            int n = arr.Length;
            for (int i = 0; i < (n-1); i++)
                s = s + (arr[i] + "\n");
            s = s + (arr[n-1] + sep);
            Console.WriteLine(s);
            return s;
        }
        public void PrintNodeMatrix(Node[][] matrix) {
            for(int i = 0; i < matrix.Length; i++)
                PrintNodeArray(matrix[i]);
        }
    }
}