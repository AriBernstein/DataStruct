using System;

namespace Fractional_Cascading {
    class RangeTree {
        MergeSortNodes msn = new MergeSortNodes();
        BinarySearchNodes bsn = new BinarySearchNodes();
        private int Dimensionality;
        private int Size;   // Number of total nodes
        private int n;  // Number of leaves
        private RangeTreeNode Root; // Of x dimension
        public RangeTreeNode GetRoot() {
            return Root;
        }

        private SingleCoordNode[] ExtractSingleCoordNodes(CoordNode[] coordNodes,
                                                          int dimension) {

            SingleCoordNode[] singleCoordNodes = new SingleCoordNode[n];

            for (int i = 0; i < n; i++)
                singleCoordNodes[i] =
                    new SingleCoordNode(coordNodes[i].GetDataNode(),
                                        coordNodes[i].GetAttr(dimension));
            
            msn.Sort(singleCoordNodes, dimension);
            return singleCoordNodes;
        }

        private void BuildRangeTree(CoordNode[] coordNodes) {
            // Extract lists of SingleCoordNodes from list of coordNode
            SingleCoordNode[][] singleCoordNodes =
                new SingleCoordNode[Dimensionality][];
            for(int i = 0; i < Dimensionality; i++)
                singleCoordNodes[i] = ExtractSingleCoordNodes(coordNodes, i+1);

            // Recursively build range tree using singleCoordNodes
            RangeTreeNode BuildRangeTree(SingleCoordNode[] coordSubset, int currentDim) {
                int subsetSize = coordSubset.Length;

                // Instantiate current Node
                RangeTreeNode thisNode = new RangeTreeNode(coordSubset, currentDim);

                // Build range tree for next dimension
                if (currentDim < Dimensionality) {
                    // Get subset of next dimension singleCoordNode array
                    // int minIndex = 


                    // RangeTreeNode nextDimRoot =
                    //     BuildRangeTree(nextDimCoordSubset, currentDim + 1);
                    // thisNode.SetNextDimRoot(nextDimRoot);
                }
                    
                // Base case - check if leaf
                if (subsetSize == 1) return thisNode;

                // Else recurse on left and right subsets of coordSubset
                int highestIndex = subsetSize - 1;
                int midIndex = (int)Math.Floor((double)(highestIndex) / 2);

                SingleCoordNode[] leftCoordSubset =     // Left side
                    ArrayUtils.ArraySubset(coordSubset, 0, midIndex);
                thisNode.SetLeft(BuildRangeTree(leftCoordSubset, currentDim));

                SingleCoordNode[] rightCoordSubset =    // Right side
                    ArrayUtils.ArraySubset(coordSubset, midIndex + 1, highestIndex);
                thisNode.SetRight(BuildRangeTree(rightCoordSubset, currentDim));

                return thisNode;
            }

            // Root = BuildRangeTree()
        }

        public RangeTree(CoordNode[] coordNodes) {
            Dimensionality = coordNodes[0].GetDimensionality();
            if (coordNodes.Length == 0)
                throw new Exception("coordNodes is an empty list!");
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("coordNodes has invalid dimensionality.");

            n = coordNodes.Length;
            
        }
    }
}
