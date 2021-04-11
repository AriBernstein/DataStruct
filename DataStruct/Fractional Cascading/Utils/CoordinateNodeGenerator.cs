using System;
using System.Collections.Generic;
using System.Linq;


namespace Fractional_Cascading {
    public class CoordinateNodeGenerator {

        private (int[], HashSet<int>) randUniqueIntsRange(int n, int min, int max,
                                                          int randomSeed=-1) {
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
            if (randomSeed != -1) random = new Random(randomSeed);

            if (max <= min || n < 0 || (n > max - min && max - min > 0)) {
                // need to use 64-bit to support big ranges (negative min, positive max)
                string errorMsg = "Range " + min + " to " + max + " (" + ((Int64)max -
                                  (Int64)min) + " values), or count " + n + " is illegal";
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            // hash sets don't support duplicate values
            HashSet<int> randomValueSet = new HashSet<int>();

            // start count values before max, and end at max
            for (int top = max - n; top < max; top++) {
                // May strike a duplicate. Need to add +1 to make inclusive generator
                // ->  +1 is safe even for MaxVal max value because top < max
                if (!randomValueSet.Add(random.Next(min, top + 1))) {
                    // Collision! Add inclusive max - could not have been added before.
                    randomValueSet.Add(top);
                }
            }

            // load them in to a list, to sort
            List<int> randomUniqueList = randomValueSet.ToList();

            // shuffle the results because HashSet has messed
            // with the order, and the algorithm does not produce
            // random-ordered results (e.g. max-1 will never be the first value)
            for (int i = randomUniqueList.Count - 1; i > 0; i--) {  
                int k = random.Next(i + 1);  
                int tmp = randomUniqueList[k];  
                randomUniqueList[k] = randomUniqueList[i];  
                randomUniqueList[i] = tmp;  
            }
             
            return (randomUniqueList.ToArray(), randomValueSet);
        }

        public CoordNode[] getCoordNodeList(int n, int insertData, bool sort=true,
                                            int sortAttrCode=0, int dimensions=1,
                                            int rangeMin=0, int rangeMax=10000000,
                                            int randomSeed=-1) {
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
                randomSeed: if not -1, random seed by which to select nodeData. Else
                            seed is system default
                insertData: if not -1, replace the data attribute of the node at a
                            random index in the return list  */

            CoordNode[] nodeList = new CoordNode[n];
            
            // Populate location and data values
            int[] xList;                HashSet<int> xSet;
            int[] yList = new int[0];   HashSet<int> ySet = new HashSet<int>{0};
            int[] zList = new int[0];   HashSet<int> zSet = new HashSet<int>{0};

            (int[] dataList, HashSet<int> dataSet) = // We will always need data
                randUniqueIntsRange(n, rangeMin, rangeMax, randomSeed);

            // We will always have at least one dimension
            (int[] xL, HashSet<int> xS) = randUniqueIntsRange(n, rangeMin, rangeMax);
            xList = xL; xSet = xS;
            
            // Check for further dimensionality before constructing random lists
            if (dimensions >= 2) {
                (int[] yL, HashSet<int> yS) = randUniqueIntsRange(n, rangeMin, rangeMax);
                yList = yL; ySet = yS;
            }
            if (dimensions == 3) {
                (int[] zL, HashSet<int> zS) = randUniqueIntsRange(n, rangeMin, rangeMax);
                zList = zL; zSet = zS;
            }
            if(dimensions > 3 || dimensions < 1) {
                string errMsg =
                    "Invalid dimensions parameter value when calling getCoordNodeList";
                throw new Exception(errMsg);
            }

            for(int i = 0; i < n; i++) {  // Build nodes using newly generated lists/sets
                if(dimensions == 1) {
                    nodeList[i] = new CoordNode(dataList[i], xList[i]);
                } else if(dimensions == 2) {
                    nodeList[i] = new CoordNode(dataList[i], xList[i], yList[i]);
                } else if(dimensions == 3) {
                    nodeList[i] =
                        new CoordNode(dataList[i], xList[i], yList[i], zList[i]);
                }
            }

            // Insert expected search value
            if(insertData >= 0) {   // note: n/2 index in nodeList is arbitrary
                if(!(dataSet.Contains(insertData))) nodeList[n/2].setData(insertData);
            } else throw new Exception("insertData parameter must be positive");
            
            // Sort randomly generated attributes on sortAttrCode
            if(sort) new MergeSortNodes().sort(nodeList, sortAttrCode);
            
            return nodeList;
        }
    }
}