using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    public class Utils {
        private string sep = "\n-----\n";

        public string PrettyNumApprox(int n) {
            /**
            Generate string with approximation of large numer
            ex. n = 5001243, return "5 million" */
            double num = (double)n;

            if(num <= 9999) return num.ToString();   // to small for this
            
            double numDigits = Math.Floor(Math.Log(num, 10)) + 1;
            
            // Get leading values
            double numLeadingVals = numDigits % 3;
            if(numLeadingVals == 0) numLeadingVals = 3;
            int leadingVal =
                (int)Math.Truncate((n / Math.Pow(10, numDigits - numLeadingVals)));
            
            // Get size label
            double sizeClass = Math.Floor(numDigits / 3);
            // so that 555555 returns "555 thousand" instead of "555 million"
            if(numLeadingVals == 3) sizeClass -= 1;

            string sizeClassLabel;
            if(sizeClass == 1) sizeClassLabel = "thousand";
            else if (sizeClass == 2) sizeClassLabel = "million";
            else if (sizeClass == 3) sizeClassLabel = "billion";
            else sizeClassLabel = "too many zeros man";

            return $"{leadingVal.ToString()} {sizeClassLabel}";
        }

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