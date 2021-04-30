using System;

namespace Fractional_Cascading {
    class RangeTree {
        MergeSortNodes msn = new MergeSortNodes();
        BinarySearchNodes bsn = new BinarySearchNodes();
        private int Dimensionality;
        private int Size = 0;   // Number of total nodes
        private int n;  // Number of leaves
        private RangeTreeNode Root; // Of x dimension
        public RangeTreeNode GetRoot() {
            return Root;
        }

        private RangeTreeNode BuildRangeTree(CoordNode[] coordSubset, int currentDim) {
            // Sort coordSubset based on location in currentDim
            msn.Sort(coordSubset, currentDim);

            int subsetSize = coordSubset.Length;

            // Instantiate current Node
            RangeTreeNode thisNode = new RangeTreeNode(coordSubset, currentDim);

            // Build range tree for next dimension
            if (currentDim < Dimensionality)
                thisNode.SetNextDimRoot(BuildRangeTree(coordSubset, currentDim + 1));
                
            // Base case - check if leaf
            if (subsetSize == 1) return thisNode;

            // Else recurse on left and right subsets of coordSubset
            int highestIndex = subsetSize - 1;
            int midIndex = (int)Math.Floor((double)(highestIndex) / 2);

            CoordNode[] leftCoordSubset =     // Left side
                ArrayUtils.ArraySubset(coordSubset, 0, midIndex);
            thisNode.SetLeft(BuildRangeTree(leftCoordSubset, currentDim));

            CoordNode[] rightCoordSubset =    // Right side
                ArrayUtils.ArraySubset(coordSubset, midIndex + 1, highestIndex);
            thisNode.SetRight(BuildRangeTree(rightCoordSubset, currentDim));

            return thisNode;
        }

        public RangeTree(CoordNode[] coordNodes) {
            Dimensionality = coordNodes[0].GetDimensionality();
            if (coordNodes.Length == 0)
                throw new Exception("coordNodes is an empty list!");
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("coordNodes has invalid dimensionality.");

            n = coordNodes.Length;

            Root = BuildRangeTree(coordNodes, 1);
        }
    }
}
