using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    class RangeTree {
        MergeSortNodes msn = new MergeSortNodes();
        private int Dimensionality;
        private int n;  // Number of leaves
        private RangeTreeNode Root; // Of x dimension
        private Utils u = new Utils();
        
        public RangeTreeNode GetRoot() {
            return Root;
        }

        public CoordNode[] OrthogonalRangeSearch(int[] rangeMins, int[] rangeMaxes,
                                                 bool sortOnDataAfterQuery=true) {
            int x1 = rangeMins[0];
            int x2 = rangeMaxes[0];
            int y1 = -1;    int y2 = -1;    int z1 = -1;    int z2 = -1;
            if (x1 > x2) throw new Exception($"x1 ({x1}) must be less than x2 ({x2})");

            // Lists of tuples representing paths taken by rangeMin and rangeMax queries
            // int = 0 -> leftChild, int = 1 -> rightChild
            var xRangeMinPath = new List<(int, RangeTreeNode)>();
            var XRangeMaxPath = new List<(int, RangeTreeNode)>();
            var yRangeMinPath = new List<(int, RangeTreeNode)>();
            var yRangeMaxPath = new List<(int, RangeTreeNode)>();
            var zRangeMinPath = new List<(int, RangeTreeNode)>();
            var zRangeMaxPath = new List<(int, RangeTreeNode)>();
            
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

            RangeTreeNode FindNode(RangeTreeNode root, int key,
                                   List<(int, RangeTreeNode)> pathList) {

                if (root.IsLeaf()) return root; // Base case

                if (key <= root.GetData()) {    // Recurse Right
                    pathList.Add((0, root));
                    FindNode(root.Left(), key, pathList);
                } else {
                    pathList.Add((1, root));    // Recurse Left
                    FindNode(root.Right(), key, pathList);
                }

                throw new Exception("This one really shouldn't ever be hit :/");
            }
            
            RangeTreeNode SearchRec(RangeTreeNode root, int currDim)  {
                
                int lowRange, highRange;
                List<(int, RangeTreeNode)> rangeMinPath, rangeMaxPath;

                if (currDim == 1) {
                    lowRange = x1;
                    highRange = x2;
                    rangeMinPath = xRangeMinPath;
                    rangeMaxPath = XRangeMaxPath;
                } else if (currDim == 2) {
                    lowRange = y1;
                    highRange = y2;
                    rangeMinPath = yRangeMinPath;
                    rangeMaxPath = yRangeMaxPath;
                } else {
                    lowRange = z1;
                    highRange = z2;
                    rangeMinPath = zRangeMinPath;
                    rangeMaxPath = zRangeMaxPath;
                }

                // Find leftmost and rightmost nodes in range
                // Note: Node closest to rangeMax will greater than or equal to it.
                RangeTreeNode rangeMinNode = FindNode(Root, lowRange, rangeMinPath);
                RangeTreeNode rangeMaxNode = FindNode(Root, highRange, rangeMaxPath);

                // Find rangeSplitNode
                int rangeSplitNodeIndex = -1;
                int n = u.Minimum(rangeMinPath.Count - 1, rangeMaxPath.Count - 1);
                for (int i = 0; i < n; i++) {
                    (int currentMinStep, RangeTreeNode currentMinNode) = rangeMinPath[i];
                    (int currentMaxStep, RangeTreeNode currentMaxNode) = rangeMaxPath[i];
                    
                    // in this case, currentMinNode == currentMaxNode
                    if (currentMinStep == currentMaxStep) rangeSplitNodeIndex = i;
                    else break;
                }

                // Recurse on next dimension at split point
                (int _, RangeTreeNode RangeTreeSplitNode) =
                    rangeMinPath[rangeSplitNodeIndex];
                if (currDim < Dimensionality)
                    root = SearchRec(root.NextDimRoot(), currDim + 1);

                // // Find canonical subsets
                // List<RangeTreeNode> canonicalSubsets = new List<RangeTreeNode>();

                // // -> Handle low range
                // for(int i = rangeSplitNodeIndex; i < rangeMinPath.Count; i++) {
                //     (int step, RangeTreeNode subtree) = rangeMaxPath[i];
                //     if (subtree.IsLeaf()) canonicalSubsets.Add(subtree);
                //     else if (step == 0 && subtree.Right() != null)
                //         canonicalSubsets.Add(subtree.Right());
                // }

                // // -> Handle high range
                // for(int i = rangeSplitNodeIndex; i < rangeMaxPath.Count; i++) {
                //     (int step, RangeTreeNode subtree) = rangeMaxPath[i];
                    
                //     // At this point, last canonical subset will be an element either
                //     // equal to rangeMax or the lowest element greater than rangeMax
                //     if (subtree.IsLeaf() && subtree.GetData() == highRange)
                //         canonicalSubsets.Add(subtree);
                //     else if (step == 1 && subtree.Left() != null)
                //         canonicalSubsets.Add(subtree.Left());
                // }

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
