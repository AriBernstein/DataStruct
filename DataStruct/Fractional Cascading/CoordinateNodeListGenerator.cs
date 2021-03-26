using System;
using System.Collections.Generic;
using System.Linq;


namespace Fractional_Cascading {
    public class CoordinateNodeListGenerator {
        public int[] randUniqueIntsRange(int n, int min=1, int max=10000) {
            /**
            Generate list of random non-repeating integers

            Parameters:
                n:      size of list of random unique integers to generate
                min:    lower bound (inclusive) of the random integers to generate
                max:    upper bound (exclusive) of the random integers to generate

            Algorithm:
                initialize set S to empty
                for J := N-M + 1 to N do
                    T := RandInt(1, J)
                    if T is not in S then insert T in S
                    else insert J in S   */
                        
            Random random = new Random();

            if (max <= min || n < 0 || (n > max - min && max - min > 0)) {
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
                if (!candidates.Add(random.Next(min, top + 1))) {
                    // Collision! Add inclusive max - could not have been added before.
                    candidates.Add(top);
                }
            }

            // load them in to a list, to sort
            List<int> result = candidates.ToList();

            // shuffle the results because HashSet has messed
            // with the order, and the algorithm does not produce
            // random-ordered results (e.g. max-1 will never be the first value)
            for (int i = result.Count - 1; i > 0; i--) {  
                int k = random.Next(i + 1);  
                int tmp = result[k];  
                result[k] = result[i];  
                result[i] = tmp;  
            }
             
            return result.ToArray();
        }

        public CoordNode[] getCoordNodeList(int n, bool sort=true, int sortAttrCode=0,
                                            int dimensions=1,
                                            bool randomizeRadomSeed=false,
                                            int randomSeed=10) {
            /**
            NOTE: randomizeRadomSeed must be false when this is used to build matrices
                  for FractionalCascadingMatrices */

            CoordNode[] nodeList = new CoordNode[n];
            int[] x_list = randUniqueIntsRange(n);
            int[] y_list = randUniqueIntsRange(n);
            int[] z_list = randUniqueIntsRange(n);

            if(randomizeRadomSeed) randomSeed = new Random().Next();
             
            Random random = new Random(randomSeed);
            for(int i = 0; i < n; i++) {
                int nodeData = random.Next(0, 2000);
                if(dimensions == 1) {
                    nodeList[i] = new CoordNode(nodeData, x_list[i]);
                } else if(dimensions == 2) {
                    nodeList[i] = new CoordNode(nodeData, x_list[i], y_list[i]);
                } else if(dimensions == 3) {
                    nodeList[i] = new CoordNode(nodeData, x_list[i], y_list[i], z_list[i]);
                }
                nodeData++;
            } 

            if(sort) {
                MergeSortNodes msn = new MergeSortNodes();
                msn.sort(nodeList, sortAttrCode);
            }
            
            return nodeList;
        }
    }
}