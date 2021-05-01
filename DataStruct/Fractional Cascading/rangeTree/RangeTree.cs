using System;

namespace Fractional_Cascading {
    class RangeTree {
        MergeSortNodes msn = new MergeSortNodes();
        private int Dimensionality;
        private int n;  // Number of leaves
        private RangeTreeNode Root; // Of x dimension
        public RangeTreeNode GetRoot() {
            return Root;
        }

        public CoordNode[] OrthogonalRangeSearch(int[] rangeMins, int[] rangeMaxes,
                                                 bool sortOnDataAfterQuery=true) {
            int x1 = rangeMins[0];
            int x2 = rangeMaxes[0];
            int y1 = -1;    int y2 = -1;    int z1 = -1;    int z2 = -1;
            if (x1 > x2) throw new Exception($"x1 ({x1}) must be less than x2 ({x2})");
            
            if (Dimensionality >= 2) {
                y1 = rangeMins[1];
                y2 = rangeMaxes[1];
                if (y1 > y2)
                    throw new Exception($"y1 ({y1}) must be less than y2 ({y2})");

            }

            if (Dimensionality == 3) {
                z1 = rangeMins[2];
                z2 = rangeMaxes[2];
                if (z1 > z2)
                    throw new Exception($"z1 ({z1}) must be less than Z2 ({z2})");
            }
            
            RangeTreeNode SearchRec(RangeTreeNode root, int currDim)  {
                int lowRange, highRange;
                if (currDim == 1) {
                    lowRange = x1;
                    highRange = x2;
                } else if (currDim == 2) {
                    lowRange = y1;
                    highRange = y2;
                } else {
                    lowRange = z1;
                    highRange = z2;
                }

                // Find canonical subset
                if (root.Left() != null && highRange < root.Left().GetData())
                    root = SearchRec(root.Left(), currDim);

                if (root.Right() != null && lowRange > root.Right().GetData())
                    root = SearchRec(root.Right(), currDim);
                
                // Recurse on next dimension
                if (currDim < Dimensionality)
                    root = SearchRec(root.NextDimRoot(), currDim + 1);

                if (sortOnDataAfterQuery) msn.Sort(root.GetNodeList(), 0);

                return root;
            }

            return SearchRec(Root, 1).GetNodeList();
        }

        private RangeTreeNode BuildRangeTree(CoordNode[] coordSubset, int currentDim) {

            // Instantiate current Node
            RangeTreeNode thisNode = new RangeTreeNode(coordSubset, currentDim);

            // Build range tree for next dimension
            if (currentDim < Dimensionality)
                thisNode.SetNextDimRoot(BuildRangeTree(coordSubset, currentDim + 1));
                
            // Sort coordSubset based on location in currentDim
            msn.Sort(coordSubset, currentDim);

            int subsetSize = coordSubset.Length;

            // Base case - check if leaf
            if (subsetSize == 1) {
                thisNode.SetLocation(thisNode.GetNodeList()[0].GetAttr(currentDim));
                return thisNode;
            }

            // Else recurse on left and right subsets of coordSubset
            int highestIndex = subsetSize - 1;
            int midIndex = (int)Math.Floor((double)(highestIndex) / 2);

            CoordNode[] leftCoordSubset =     // Left side
                ArrayUtils.ArraySubset(coordSubset, 0, midIndex);
            RangeTreeNode leftChild = BuildRangeTree(leftCoordSubset, currentDim);
            thisNode.SetLeft(leftChild);
            leftChild.SetParent(thisNode);

            CoordNode[] rightCoordSubset =    // Right side
                ArrayUtils.ArraySubset(coordSubset, midIndex + 1, highestIndex);
            RangeTreeNode rightChild = BuildRangeTree(rightCoordSubset, currentDim);
            thisNode.SetRight(rightChild);
            rightChild.SetParent(thisNode);

            // Sort value = highest location in leftChild subtree
            int sortAttribute =
                leftCoordSubset[leftCoordSubset.Length - 1].GetAttr(currentDim);
            thisNode.SetLocation(sortAttribute);

            return thisNode;
        }

        public RangeTreeNode GetRootByDimension(int dimensions) {
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("GetRootByDimension method has invalid " +
                                    " dimensions parameter.");
            RangeTreeNode root = Root;
            for (int i = 2; i <= dimensions; i++)
                root = root.NextDimRoot();
            
            return root;
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
