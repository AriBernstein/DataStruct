using System;
using System.Collections.Generic;
using System.Linq;


namespace Fractional_Cascading {
    public class CoordinateNodeListGenerator {

        private (int[], HashSet<int>) randUniqueIntsRange(int n, int min, int max) {
            /**
            Generate list of random non-repeating integers, return both randomly-ordered
            list and set (for checking whether or not value to insert exists quickly)

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
             
            return (result.ToArray(), candidates);
        }

        public CoordNode[] getCoordNodeList(int n, bool sort=true, int sortAttrCode=0,
                                            int dimensions=1, int locRangeMin=1,
                                            int locRangeMax=10000, int dataRangeMin=0,
                                            int dataRangeMax=2000, int randomSeed=10,
                                            int insertDataVal=-1) {
            /**
            Return a list of coordNodes with random x, y, and z values ranging locRangeMin
            to locRangeMax and data values ranging from dataRangeMin to dataRangeMax
            (inclusive min, exlusive max). 

            Parameters:
                n: the length of the list of nodes to return
                sort: if true, sort return list ordered by coordNode.getAttr(sortAttrCode)
                dimensions: between one and three
                locRangeMin: locRangeMax: randomized range of xyz values
                dataRangeMin: dataRangeMax: randomized range of node data values
                randomSeed: random seed by which to select nodeData
                insertDataVal: if not -1, replace the data attribute of the node at a
                               random index in the return list  */

            CoordNode[] nodeList = new CoordNode[n];
            (int[] xList, HashSet<int> xSet) =
                randUniqueIntsRange(n, locRangeMin, locRangeMax);
            (int[] yList, HashSet<int> ySet) =
                randUniqueIntsRange(n, locRangeMin, locRangeMax);
            (int[] zList, HashSet<int> zSet) =
                randUniqueIntsRange(n, locRangeMin, locRangeMax);
            (int[] dataList, HashSet<int> dataSet) = 
                randUniqueIntsRange(n, dataRangeMin, dataRangeMax);
             
            Random random = new Random(randomSeed);
            for(int i = 0; i < n; i++) {
                if(dimensions == 1) {
                    nodeList[i] = new CoordNode(dataList[i], xList[i]);
                } else if(dimensions == 2) {
                    nodeList[i] = new CoordNode(dataList[i], xList[i], yList[i]);
                } else if(dimensions == 3) {
                    nodeList[i] = new CoordNode(dataList[i], xList[i], yList[i],
                                                zList[i]);
                }
            } 

            if(insertDataVal > -1) {
                if(insertDataVal < -1)
                    throw new Exception("insertDataVal parameter must be positive");
                
                if(!(dataSet.Contains(insertDataVal))) {
                    int randomIndex = random.Next(0, n);
                    nodeList[randomIndex].setData(insertDataVal);
                }
            }

            if(sort) {
                MergeSortNodes msn = new MergeSortNodes();
                msn.sort(nodeList, sortAttrCode);
            }
            
            return nodeList;
        }
    }
}