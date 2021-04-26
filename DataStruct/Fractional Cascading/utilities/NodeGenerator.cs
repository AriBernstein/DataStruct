using System;
using System.Collections.Generic;
using System.Linq;

namespace Fractional_Cascading {
    public class NodeGenerator {
        public (int[], HashSet<int>) RandUniqueInts(int n, int min, int max,
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
            if (randomSeed == -1) random = new Random();
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

            if (randomizeOrder) {
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
        
        public CoordNode[] GetCoordNodeList(int n, int insertData, bool sort=true,
                                            int sortAttrCode=0, int dimensions=1,
                                            int rangeMin=0, int rangeMax=10000000,
                                            int randomSeed=-1, bool randomizeOrder=true) {
            /**
            Return a list of coordNodes with random x, y, and z values ranging locRangeMin
            to locRangeMax and data values ranging from dataRangeMin to dataRangeMax
            (inclusive min, exclusive max). 

            Parameters:
                n: the length of the list of nodes to return
                sort: if true, sort return list ordered by coordNode.getAttr(sortAttrCode)
                dimensions: between one and three
                locRangeMin: locRangeMax: randomized range of xyz values
                dataRangeMin: dataRangeMax: randomized range of node data values
                randomSeed: random seed used dataList generation, system default if -1
                insertData: if not -1, replace the data attribute of the node at a
                            random index in the return list  */

            CoordNode[] nodeList = new CoordNode[n];
            
            // Populate location and data values
            int[] xList;                HashSet<int> xSet;
            int[] yList = new int[0];   HashSet<int> ySet = new HashSet<int>{0};
            int[] zList = new int[0];   HashSet<int> zSet = new HashSet<int>{0};

            (int[] dataList, HashSet<int> dataSet) = // We will always need data
                RandUniqueInts(n, rangeMin, rangeMax, randomSeed, randomizeOrder);

            // We will always have at least one dimension
            (xList, xSet) = RandUniqueInts(n, rangeMin, rangeMax, randomSeed,
                                           randomizeOrder);
            
            // Check for further dimensionality before constructing random lists
            if (dimensions >= 2) {
                (int[] yL, HashSet<int> yS) = RandUniqueInts(n, rangeMin, rangeMax,
                                                            randomSeed, randomizeOrder);
                yList = yL; ySet = yS;
            }
            if (dimensions == 3) {
                (int[] zL, HashSet<int> zS) = RandUniqueInts(n, rangeMin, rangeMax,
                                                             randomSeed, randomizeOrder);
                zList = zL; zSet = zS;
            }
            if (dimensions > 3 || dimensions < 1) {
                string errMsg =
                    "Invalid dimensions parameter value when calling getCoordNodeList";
                throw new Exception(errMsg);
            }

            for (int i = 0; i < n; i++) {  // Build nodes using newly generated lists/sets
                if (dimensions == 1) {
                    nodeList[i] = new CoordNode(dataList[i], xList[i]);
                } else if (dimensions == 2) {
                    nodeList[i] = new CoordNode(dataList[i], xList[i], yList[i]);
                } else if (dimensions == 3) {
                    nodeList[i] =
                        new CoordNode(dataList[i], xList[i], yList[i], zList[i]);
                }
            }

            // Insert expected search value
            if (insertData >= 0) {   // note: n/2 index in nodeList is arbitrary
                if (!(dataSet.Contains(insertData))) nodeList[n/2].SetData(insertData);
            } else throw new Exception("insertData parameter must be positive");
            
            // Sort randomly generated attributes on sortAttrCode
            if (sort) new MergeSortNodes().Sort(nodeList, sortAttrCode);
            
            return nodeList;
        }

        public RBTreeNode[] GetRandomBSTNodes(int n, int min=0, int max=-1) {
            if (max == -1) max = n * 10;
            RBTreeNode[] nodeList = new RBTreeNode[n];
            (int[] keyList, HashSet<int> keySet) = RandUniqueInts(n, min, max);
            for (int i = 0; i < n; i++) nodeList[i] = new RBTreeNode(keyList[i]);
            return nodeList;
        }
    }
}
