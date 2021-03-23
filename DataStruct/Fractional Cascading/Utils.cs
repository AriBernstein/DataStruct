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

        public void printCoordNodeArray(CoordinateNode[] arr) {
            string s = "";
            int n = arr.Length;
            for (int i = 0; i < (n-1); i++) {
                s = s + (arr[i] + "\n");
            }
            s = s + (arr[n-1] + sep);
            Console.WriteLine(s);
        }
        public void printCoordMatrix(CoordinateNode[][] matrx) {
            for(int i = 0; i < matrx.Length; i++) printCoordNodeArray(matrx[i]);
        }

        public void printFCNodeArray(FractionalCascadingNode[] arr) {
            String s = "";
            int n = arr.Length;
            for (int i = 0; i < (n-1); i++) {
                s = s + (arr[i] + "\n");
            }
            s = s +(arr[n-1] + sep);
            Console.WriteLine(s);
        }

        public void printFCNodeMatrix(FractionalCascadingNode[][] matrx) {
            for(int i = 0; i < matrx.Length; i++) printFCNodeArray(matrx[i]);
        }
    }
}