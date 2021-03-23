using System;
namespace Fractional_Cascading {
    public class Utils {
        // A utility function tocprint array of size n */
        public void printStringArray(int[] arr) {
            int n = arr.Length;
            for (int i = 0; i < n; ++i)
                Console.Write(arr[i] + " ");
            Console.WriteLine();
        }

        public void printCoordNodeArray(CoordinateNode[] arr) {
            string s = "";
            int n = arr.Length;
            for (int i = 0; i < (n-1); i++) {
                s = s + (arr[i] + "\n");
            }
            s = s + (arr[n-1] + "\n-----\n");
            Console.WriteLine(s);
        }

        public void printFCNodeArray(FractionalCascadingNode[] arr) {
            String s = "";
            int n = arr.Length;
            for (int i = 0; i < (n-1); i++) {
                s = s + (arr[i] + "\n");
            }
            s = s +(arr[n-1] + "\n-----\n");
            Console.WriteLine(s);
        }

        public void printFCNodeMatrix(FractionalCascadingNode[][] matrx) {
            for(int i = 0; i < matrx.Length; i++) printFCNodeArray(matrx[i]);
        }
    }
}