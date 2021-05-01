using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    public class Utils {
        public String Separator(int separatorLength=10, int newLinesAbove=1,
                                int newLinesBelow=1) {
            return new String('\n', newLinesAbove) +
                   new String('-', separatorLength) +
                   new String('\n', newLinesBelow);
        }

        public int RoundInt(int num, int place) {
            return (num + 50) / place * place;
        }
        
        public string PrettyNumApprox(int n) {
            /**
            Generate string with approximation of large number
            ex. n = 5001243, return "5 million" */
            double num = (double)n;

            if (num <= 9999) return num.ToString();   // to small for this
            
            double numDigits = Math.Floor(Math.Log(num, 10)) + 1;
            double numLeadingVals = numDigits % 3;
            
            // Round num to the nearest place that would affect the leading vals, update n
            double numTrailingVals = (int)(numDigits - numLeadingVals);
            n = RoundInt(n, Int32.Parse('1' + new String('0', (int)numTrailingVals -1)));
            numDigits = Math.Floor(Math.Log(num, 10)) + 1;
            numLeadingVals = numDigits % 3;
            numTrailingVals = numDigits - numLeadingVals;

            // Handle case where numDigits is evenly divisible by three
            if (numLeadingVals == 0) numLeadingVals = 3;
                int leadingVal =
                    (int)Math.Truncate((n / Math.Pow(10, numTrailingVals)));
            
            // Get size label
            double sizeClass = Math.Floor(numDigits / 3);
            if (numLeadingVals == 3) sizeClass -= 1;

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

        public string PrintDataLocationDict(Dictionary<int, int> dict, String searchVal) {
            int n = dict.Count;
            searchVal = $"{searchVal} located in dimension";
            string s = "";
            for (int i = 0; i < (n-1); ++i)
                s = $"{s}{searchVal} {i + 1} at:\t{dict[i + 1]}\n";
            s = $"{s}{searchVal} {n} at:\t{dict[n]}";

            Console.WriteLine(s);
            return s;
        }

        public String PrintNodeMatrix(Node[][] matrix) {
            String s = "";
            for (int i = 0; i < matrix.Length; i++)
                s = s + ArrayUtils.PrintArray(matrix[i], sep:"\n");
            return s;
        }

        public SingleCoordNode[] ExtractSingleCoordNodes(CoordNode[] coordNodes,
                                                         int dimension) {
            int n = coordNodes.Length;
            SingleCoordNode[] singleCoordNodes = new SingleCoordNode[n];

            for (int i = 0; i < n; i++)
                singleCoordNodes[i] =
                    new SingleCoordNode(coordNodes[i].GetDataNode(),
                                        coordNodes[i].GetAttr(dimension));
            
            new MergeSortNodes().Sort(singleCoordNodes, dimension);
            return singleCoordNodes;
        }
    }
}
