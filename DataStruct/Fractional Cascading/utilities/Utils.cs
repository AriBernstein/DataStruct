using System;
using System.Collections.Generic;
using System.Linq;

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
            if      (sizeClass == 1) sizeClassLabel = "thousand";
            else if (sizeClass == 2) sizeClassLabel = "million";
            else if (sizeClass == 3) sizeClassLabel = "billion";
            else if (sizeClass == 4) sizeClassLabel = "trillion";
            else if (sizeClass == 5) sizeClassLabel = "quadrillion";
            else if (sizeClass == 6) sizeClassLabel = "quintrillion";
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
        
        public (int[], HashSet<int>) RandUniqueIntsRange(int n, int min, int max,
                                                         int randomSeed=-1,
                                                         bool randomizeOrder=true) {
            /**
            Note: this function is a modified implementation the following solution:
                  https://codereview.stackexchange.com/a/61372

            Generate list of random non-repeating integers, return both randomly-ordered
            list and set (for checking whether or not value to insert exists quickly)

            Parameters:
                n:      size of list of random unique integers to generate
                min:    lower bound (inclusive) of the random integers to generate
                max:    upper bound (exclusive) of the random integers to generate
                randomSeed: if -1, use system default, else use this
                randomizeOrder: this function uses a hash set to ensure non-repeating
                                numbers. When converted to a list, its order is not
                                random, so we shuffle it to make it so. Shuffling takes
                                time is not necessary if a random order is not needed
            Algorithm:
                initialize set S to empty
                for J := N-M + 1 to N do
                    T := RandInt(1, J)
                    if T is not in S then insert T in S
                    else insert J in S   */
                        
            Random random;
            if(randomSeed == -1) random = new Random();
            else random = new Random(randomSeed);

            if (max <= min || n < 0 ||  // max - min > 0 required to avoid overflow
                                        (n > max - min && max - min > 0)) {
                // need to use 64-bit to support big ranges (negative min, positive max)
                string errorMsg = "Range " + min + " to " + max + " (" + ((Int64)max -
                                  (Int64)min) + " values), or count " + n + " is illegal";
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            // hash sets don't support duplicate values
            HashSet<int> candidates = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - n; top < max; top++) {
                // May strike a duplicate. Need to add +1 to make inclusive generator
                // ->  +1 is safe even for MaxVal max value because top < max
                if (!candidates.Add(random.Next(min, top + 1)))
                    // Collision! Add inclusive max - could not have been added before.
                    candidates.Add(top);
            }

            // load them in to a list, to sort
            List<int> result = candidates.ToList();

            if(randomizeOrder) {
            // shuffle the results because HashSet has messed with the order, and the
            // algorithm does not produce random-ordered results
            // -> (ex. max-1 will never be the first value)
                for (int i = result.Count - 1; i > 0; i--) {  
                    int k = random.Next(i + 1);  
                    int tmp = result[k];  
                    result[k] = result[i];  
                    result[i] = tmp;  
                }
            }
             
            return (result.ToArray(), candidates);
        }
    }
}