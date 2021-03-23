using System;
namespace Fractional_Cascading {
    public class Utils {

        private string sep = "\n-----\n";

        // A utility function tocprint array of size n */
        public void printStringArray(string[] arr) {
            int n = arr.Length;
            string s = "";
            for (int i = 0; i < (n-1); ++i)
                s = s + (arr[i] + ", ");
            s = s + arr[n-1] + sep;
            Console.WriteLine(s);
        }

        public void printIntArray(int[] arr) {
            int n = arr.Length;
            string s = "";
            for (int i = 0; i < (n-1); ++i)
                s = s + (arr[i] + ", ");
            s = s + arr[n-1] + sep;
            Console.WriteLine(s);
        }

        public void printDataLocationArray(int[] arr) {
            int n = arr.Length;
            string s = "";
            for (int i = 0; i < (n-1); ++i)
                s = s + "located in dimension " + (i + 1) + " at " + arr[i] + ", ";
            s = s + "located in dimension " + n + " at: " + arr[n-1] + ", " + sep;
            Console.WriteLine(s);
        }

        public void printNodeArray(Node[] arr) {
            string s = "";
            int n = arr.Length;
            for (int i = 0; i < (n-1); i++) {
                s = s + (arr[i] + "\n");
            }
            s = s + (arr[n-1] + sep);
            Console.WriteLine(s);
        }
        public void printNodeMatrix(Node[][] matrx) {
            for(int i = 0; i < matrx.Length; i++) printNodeArray(matrx[i]);
        }
    }
}