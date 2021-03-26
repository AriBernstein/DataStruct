using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    public class Utils {
        private string sep = "\n-----\n";

        public void printIntArray(int[] arr) {
            int n = arr.Length;
            string s = "";
            for (int i = 0; i < (n-1); ++i) s = s + (arr[i] + ", ");
            s = s + arr[n-1] + sep;
            Console.WriteLine(s);
        }

        public void printDataLocationDict(Dictionary<int, int> dict, String val) {
            int n = dict.Count;
            val = val + ' ';
            string s = "";
            for (int i = 0; i < (n-1); ++i)
                s = s + val + "located in dimension " + 
                    (i + 1) + " at: \t" + dict[i + 1] + '\n';
            s = s + val + "located in dimension " + n + " at: \t" + dict[n] + sep;

            Console.WriteLine(s);
        }

        public void printNodeArray(Node[] arr) {
            string s = "";
            int n = arr.Length;
            for (int i = 0; i < (n-1); i++) s = s + (arr[i] + "\n");
            s = s + (arr[n-1] + sep);
            Console.WriteLine(s);
        }
        public void printNodeMatrix(Node[][] matrx) {
            for(int i = 0; i < matrx.Length; i++) printNodeArray(matrx[i]);
        }
    }
}