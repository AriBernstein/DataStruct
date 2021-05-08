using System;
using System.Linq;
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

        public RangeTreeNode FindNode(RangeTreeNode root, int key,
                                      List<(int, RangeTreeNode)> pathList=null) {
            /**
            Search RangeTreeNode for node with Location value of key or its successor
            
            Parameters:
                root: the root of the current subtree in which we are searching
                key:  the location value in the node for which we are searching
                pathList: list containing (int, RangeTreeNode) to denote search route
                          int = 0 -> recurse left, int = 1 -> recurse right
                          RangeTreeNode - node on which we are recursing left or right
                          (only populate this list if it is not null)   */ 

            if (root.IsLeaf()) {
                if (pathList != null) pathList.Add((-1, root));
                return root;
            } else if (key <= root.GetData()) {
                if (pathList != null) pathList.Add((0, root));
                return FindNode(root.Left(), key, pathList);
            } else {
                if (pathList != null) pathList.Add((1, root));
                return FindNode(root.Right(), key, pathList);
            }
        }

        public CoordNode[] OrthogonalRangeSearch(int[] rangeMins, int[] rangeMaxes,
                                                 bool sortOnDataAfterQuery=true) {
            /**
            Search for subset of nodes in (inclusive) range between rangeMins[i] and
            rangeMaxes[i], where i represents the dimension in question.

            Parameters:
                rangeMins: ith list represents the (inclusive) lower bound of dimension i
                rangeMaxes: ith list represents the (inclusive) upper bound of dimension i
                sortOnDataAfterQuery: if true, return nodes in range sorted on data */

            // Instantiate a bunch of stuff
            int x1 = rangeMins[0];
            int x2 = rangeMaxes[0];
            int y1 = -1;    int y2 = -1;    int z1 = -1;    int z2 = -1;
            if (x1 > x2) throw new Exception($"x1 ({x1}) must be less than x2 ({x2})");

            // Lists of coordNodes representing canonical subsets
            // -> If current dimension is less than dimensionality, recurse on each.
            // -> Else return canonical subset (representing nodes in final dimension)
            var xCanonicalSubsets = new List<RangeTreeNode>();
            var yCanonicalSubsets = new List<RangeTreeNode>();
            var zCanonicalSubsets = new List<RangeTreeNode>();
            
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
                        
            List<RangeTreeNode> SearchRec(RangeTreeNode root, int currDim)  {
                /**
                Helper function responsible for most of the search functionality.
                
                Parameters:
                    root: the root of the subtree on which we are searching
                    currDim: the current dimension in which we are searching
                    
                Return: Canonical subsets of the subtree root which are in range    */
                
                int rangeMin, rangeMax;
                var rangeMinPath = new List<(int, RangeTreeNode)>();
                var rangeMaxPath = new List<(int, RangeTreeNode)>();
                List<RangeTreeNode> canonicalSubsets;
                
                if (currDim == 1) {
                    rangeMin = x1;
                    rangeMax = x2;
                    canonicalSubsets = xCanonicalSubsets;
                } else if (currDim == 2) {
                    rangeMin = y1;
                    rangeMax = y2;
                    canonicalSubsets = yCanonicalSubsets;
                } else {
                    rangeMin = z1;
                    rangeMax = z2;
                    canonicalSubsets = zCanonicalSubsets;
                }

                bool InRange(int data) {
                    return (rangeMin <= data && data <= rangeMax);
                }

                bool rangeMaxNodeInRange;

                // Find leftmost and rightmost nodes in range and populate path lists
                // Note: rangeMaxNode will either be the smallest node with location
                //       greater than lowRange or equal to it.
                RangeTreeNode rangeMinNode = FindNode(root, rangeMin, rangeMinPath);
                RangeTreeNode rangeMaxNode = FindNode(root, rangeMax, rangeMaxPath);
                rangeMaxNodeInRange = InRange(rangeMaxNode.GetData());

                // Find rangeSplitNode (node at which lowRange & highRange paths diverge)
                int rangeSplitPathIndex = 0;
                int shortestPathLen = u.Minimum(rangeMinPath.Count, rangeMaxPath.Count);
                for (int i = 0; i < (shortestPathLen - 1); i++) {
                    (int minStep, RangeTreeNode currentMinNode) = rangeMinPath[i];
                    (int maxStep, RangeTreeNode currentMaxNode) = rangeMaxPath[i];
                    
                    // in this case, currentMinNode == currentMaxNode
                    if (minStep == maxStep && i < rangeMinPath.Count)
                        rangeSplitPathIndex = i;
                    else break;
                }

                // Delete me
                Console.WriteLine("V SPLIT INDEX: " + rangeSplitPathIndex);
                Console.WriteLine($"Min Tree Path: (dim {currDim})");
                foreach((int step, RangeTreeNode rt) in rangeMinPath)
                    Console.WriteLine($"{step} - {rt}");

                Console.WriteLine($"\nMax Tree Path: (dim {currDim})");
                foreach((int step, RangeTreeNode rt) in rangeMaxPath)
                    Console.WriteLine($"{step} - {rt}");                
                ////

                // Find canonical subsets by separately traversing the left and right
                // subtrees of rangeSplitNode

                // Check for edge case in which rangeMin and rangeMax paths are the same
                // until they hit leaf nodes
                bool pathsDiverge = true;
                if (shortestPathLen > 1 &&
                    rangeMinPath.GetRange(0, shortestPathLen).SequenceEqual(
                    rangeMaxPath.GetRange(0, shortestPathLen))) {
                    
                    // Delete me
                    Console.WriteLine(
                        "Paths do not diverge, will not look for canonical subsets for " +
                        $"high range (dim {currDim}).");
                    pathsDiverge = false;
                }
                
                // Handle low range
                // -> check for edge case of one leaf node in path
                // -> else save coordNodes from right subtrees when path veers left
                if (rangeMinPath.Count == 1) {
                    (int _, RangeTreeNode subtree) = rangeMinPath[0];
                    if (subtree.IsLeaf() && InRange(subtree.GetData()))
                        canonicalSubsets.Add(subtree);
                } else {
                    bool lastLeft;
                    for (int i = rangeSplitPathIndex + 1; i < rangeMinPath.Count; i++) {
                        (int direction, RangeTreeNode subtree) = rangeMinPath[i];
                        
                        // Delete me
                        Console.WriteLine($"\n\nDim {currDim} - Low range recursion on ");
                        RangeTreeHelper.VisualizeTree(subtree);
                        // 

                        if (subtree.IsLeaf() && InRange(subtree.GetData())) {
                            canonicalSubsets.Add(subtree);
                        } else if (direction == 0 && subtree.Right() != null) {
                            canonicalSubsets.Add(subtree.Right());
                        }
                    }
                }

                // Handle high range
                // -> check for edge case of one leaf node in path
                // -> save coordNodes from left subtrees when path veers right
                if (rangeMaxPath.Count == 1) {
                    (int _, RangeTreeNode subtree) = rangeMaxPath[0];
                    if (subtree.IsLeaf() && InRange(subtree.GetData()))
                        canonicalSubsets.Add(subtree);
                } else if (pathsDiverge) {  // Else we will have duplicates
                    for (int i = rangeSplitPathIndex + 1; i < rangeMaxPath.Count; i++) {
                        (int direction, RangeTreeNode subtree) = rangeMaxPath[i];

                        // Delete me
                        Console.WriteLine($"\n\nDim {currDim} - High range recursion on");
                        RangeTreeHelper.VisualizeTree(subtree);
                        //
                        
                        // At this point, last canonical subset may be the lowest element
                        // with location greater than rangeMax
                        if (subtree.IsLeaf() && InRange(subtree.GetData())) {
                            canonicalSubsets.Add(subtree);
                        } else if (direction == 1 && subtree.Left() != null) {
                            canonicalSubsets.Add(subtree.Left());
                        }
                    }

                    // Delete me
                    // Console.WriteLine($"\n\nSUPPP - {currDim}\n{canonicalSubsets.Last().GetCoordNodeList().Last()}\n\\\\\\\n");
                }

                // Handle edge case where no nodes are in range
                if (canonicalSubsets.Count == 1 && canonicalSubsets[0].IsLeaf()) {
                    int singleNodeLocation = canonicalSubsets[0].GetData();
                    if (!(InRange(singleNodeLocation))) {
                        canonicalSubsets = new List<RangeTreeNode>();
                    }
                }

                // Recurse on next dimension on each canonical subset
                // -> nodesInRange should contain the RangeTreeNodes that make up the
                //    canonical subsets of the final dimension
                List<RangeTreeNode> nodesInRange = new List<RangeTreeNode>();
                if (currDim < Dimensionality) {
                    foreach (RangeTreeNode canonicalRoot in canonicalSubsets) {
                        nodesInRange.AddRange(
                            SearchRec(canonicalRoot.NextDimRoot(), currDim + 1));
                    }
                } else nodesInRange = canonicalSubsets;

                return nodesInRange;
            }

            // Find canonical subsets, extract and combine lists of coordNodes
            List<RangeTreeNode> canonicalSubsetsInSearchRange = SearchRec(Root, 1);
            List<CoordNode> nodesInSearchRange = new List<CoordNode>();
            foreach (RangeTreeNode rtNode in canonicalSubsetsInSearchRange) {
                nodesInSearchRange.AddRange(rtNode.GetCoordNodeList());
            }

            // Return
            CoordNode[] nodesInSearchRangeArray = nodesInSearchRange.ToArray();
            if (sortOnDataAfterQuery && nodesInSearchRangeArray.Length > 1)
                msn.Sort(nodesInSearchRangeArray, 0);
            return nodesInSearchRangeArray;
        }

        private RangeTreeNode BuildRangeTree(CoordNode[] coordSubset, int currentDim) {

            // Instantiate current Node
            RangeTreeNode thisNode = new RangeTreeNode(coordSubset, currentDim);

            // Build range tree for next dimension
            if (currentDim < Dimensionality)
                thisNode.SetNextDimRoot(BuildRangeTree(coordSubset, currentDim + 1));

            // Base case - check if leaf
            if (coordSubset.Length == 1) {
                thisNode.SetLocation(thisNode.GetCoordNodeList()[0].GetAttr(currentDim));
                return thisNode;
            }

            // Sort coordSubset based on location in currentDim
            msn.Sort(coordSubset, currentDim);

            // Else recurse on left and right subsets of coordSubset
            int highestIndex = coordSubset.Length - 1;
            int midIndex = (int)Math.Floor((double)(highestIndex) / 2);

            CoordNode[] leftCoordSubset =     // Left side
                ArrayUtils.Subset(coordSubset, 0, midIndex);
            RangeTreeNode leftChild = BuildRangeTree(leftCoordSubset, currentDim);
            thisNode.SetLeft(leftChild);
            leftChild.SetParent(thisNode);

            CoordNode[] rightCoordSubset =    // Right side
                ArrayUtils.Subset(coordSubset, midIndex + 1, highestIndex);
            RangeTreeNode rightChild = BuildRangeTree(rightCoordSubset, currentDim);
            thisNode.SetRight(rightChild);
            rightChild.SetParent(thisNode);

            // Sort value = highest location in leftChild subtree
            int sortAttribute =
                leftCoordSubset[leftCoordSubset.Length - 1].GetAttr(currentDim);
            thisNode.SetLocation(sortAttribute);

            return thisNode;
        }

        public RangeTreeNode GetRootByDimension(int dim) {
            // Helper function to return the root of this range tree in dimensions
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("GetRootByDimension method has invalid dimension " +
                                    $"parameter ({dim}). Must be 1, 2, or 3");
            RangeTreeNode root = Root;
            for (int i = 2; i <= dim; i++)
                root = root.NextDimRoot();
            
            return root;
        }

        public RangeTree(CoordNode[] coordNodes, int dimensionality) {
            Dimensionality = dimensionality;
            if (coordNodes.Length == 0)
                throw new Exception("coordNodes is an empty list!");
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("coordNodes has invalid dimensionality.");

            n = coordNodes.Length;

            Root = BuildRangeTree(coordNodes, 1);
        }
    }
}
