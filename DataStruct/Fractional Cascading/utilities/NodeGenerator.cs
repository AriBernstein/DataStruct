using System;
using System.Collections.Generic;
using System.Linq;

namespace Fractional_Cascading {
    public class NodeGenerator {

        Utils u = new Utils();
        public CoordNode[] GetCoordNodeList(int n, int insertData, bool sort=true,
                                            int sortAttrCode=0, int dimensions=1,
                                            int rangeMin=0, int rangeMax=10000000,
                                            int randomSeed=-1, bool randomizeOrder=true) {
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
                randomSeed: random seed used dataList generation, system default if -1
                insertData: if not -1, replace the data attribute of the node at a
                            random index in the return list  */

            CoordNode[] nodeList = new CoordNode[n];
            
            // Populate location and data values
            int[] xList;                HashSet<int> xSet;
            int[] yList = new int[0];   HashSet<int> ySet = new HashSet<int>{0};
            int[] zList = new int[0];   HashSet<int> zSet = new HashSet<int>{0};

            (int[] dataList, HashSet<int> dataSet) = // We will always need data
                u.RandUniqueIntsRange(n, rangeMin, rangeMax, randomSeed, randomizeOrder);

            // We will always have at least one dimension
            (int[] xL, HashSet<int> xS) = u.RandUniqueIntsRange(n, rangeMin, rangeMax,
                                                                randomSeed,
                                                                randomizeOrder);
            xList = xL; xSet = xS;
            
            // Check for further dimensionality before constructing random lists
            if (dimensions >= 2) {
                (int[] yL, HashSet<int> yS) = u.RandUniqueIntsRange(n, rangeMin, rangeMax,
                                                                    randomSeed,
                                                                    randomizeOrder);
                yList = yL; ySet = yS;
            }
            if (dimensions == 3) {
                (int[] zL, HashSet<int> zS) = u.RandUniqueIntsRange(n, rangeMin, rangeMax,
                                                                    randomSeed,
                                                                    randomizeOrder);
                zList = zL; zSet = zS;
            }
            if(dimensions > 3 || dimensions < 1) {
                string errMsg =
                    "Invalid dimensions parameter value when calling getCoordNodeList";
                throw new Exception(errMsg);
            }

            for(int i = 0; i < n; i++) {  // Build nodes using newly generated lists/sets
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

        
        public BSTNode[] GetRandomBSTNodes(int n, int min=0, int max=-1) {
            if(max == -1) max = n * 10;
            BSTNode[] nodeList = new BSTNode[n];
            (int[] keyList, HashSet<int> keySet) = u.RandUniqueIntsRange(n, min, max);
            for(int i = 0; i < n; i++) nodeList[i] = new BSTNode(keyList[i]);
            return nodeList;
        }
    }
}