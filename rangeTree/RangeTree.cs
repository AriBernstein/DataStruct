namespace Fractional_Cascading {
    class RangeTree {

        // Local class instances
        private Utils u = new Utils();
        MergeSortNodes msn = new MergeSortNodes();

        // Global variables
        private int Dimensionality;
        private RangeTreeNode Root; // Of lowest dimension
        private readonly int left = 0;  // represent direction in traversal paths
        private readonly int right = 1;
        
        public RangeTreeNode GetRoot() {
            return Root;
        }

        public RangeTreeNode FindNode(RangeTreeNode root, int target,
                                      List<(int, RangeTreeNode)> pathList=null) {
            /**
            Search RangeTreeNode for node with Location value of key or its successor
            
            Parameters:
                root: the root of the current subtree in which we are searching
                target: the location value in the current dimension node for which we are
                        searching
                pathList: list containing (int, RangeTreeNode) to denote search route
                          int = 0 -> recurse left, int = 1 -> recurse right
                          RangeTreeNode - node on which we are recursing left or right
                          (only populate this list if it is not null)   */ 

            if (root.IsLeaf()) {
                if (pathList != null) pathList.Add((-1, root));
                return root;
            } else if (target <= root.GetData()) {
                if (pathList != null) pathList.Add((left, root));
                return FindNode(root.Left(), target, pathList);
            } else {
                if (pathList != null) pathList.Add((right, root));
                return FindNode(root.Right(), target, pathList);
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
                        
            List<RangeTreeNode> SearchRec(RangeTreeNode root, int currDim)  {
                /**
                Helper function responsible for most of the search functionality.
                
                Parameters:
                    root: the root of the subtree in which we are searching
                    currDim: the current dimension in which we are searching
                    
                Return: Canonical subsets of the subtree root which are in range    */
                
                int rangeMin = rangeMins[currDim - 1];
                int rangeMax = rangeMaxes[currDim - 1];

                bool InRange(int data) {
                    return (rangeMin <= data && data <= rangeMax);
                }

                // Find leftmost and rightmost nodes in range and populate path lists
                // Note: rangeMaxNode will either be the smallest node with location
                //       greater than lowRange or equal to it.
                var rangeMinPath = new List<(int, RangeTreeNode)>();
                var rangeMaxPath = new List<(int, RangeTreeNode)>();
                RangeTreeNode rangeMinNode = FindNode(root, rangeMin, rangeMinPath);
                RangeTreeNode rangeMaxNode = FindNode(root, rangeMax, rangeMaxPath);
                bool pathsDiverge = rangeMinNode.GetData() != rangeMaxNode.GetData();

                // Find rangeSplitNode (node at which lowRange & highRange paths diverge)
                int pathsDivergeIndex = 0;
                if (!(pathsDiverge)) {
                    pathsDivergeIndex = rangeMinPath.Count - 2;
                } else {
                    int shortestPathLen = u.Minimum(rangeMinPath.Count, rangeMaxPath.Count);
                    for (int i = 0; i < shortestPathLen; i++) {
                        (int minStep, RangeTreeNode currentMinNode) = rangeMinPath[i];
                        (int maxStep, RangeTreeNode currentMaxNode) = rangeMaxPath[i];
                        
                        if (currentMinNode.IsLeaf() || currentMaxNode.IsLeaf()) break;
                        else if (minStep == maxStep) pathsDivergeIndex = i + 1;
                        else break;
                    }
                }

                // Lists of RangeTreeNodes representing canonical subsets
                // -> If current dimension is less than dimensionality, recurse on each.
                // -> Else return canonical subset (representing nodes in final dimension)
                var canonicalSubsets = new List<RangeTreeNode>();

                // Find canonical subsets by separately traversing the left and right
                // subtrees of rangeSplitNode
                
                // Handle low range
                // -> check for edge case of one leaf node in path
                // -> else save coordNodes from right subtrees when path veers left
                if (rangeMinPath.Count == 1) {
                    (int _, RangeTreeNode subtree) = rangeMinPath[0];
                    if (subtree.IsLeaf() && InRange(subtree.GetData()))
                        canonicalSubsets.Add(subtree);
                } else {
                    for (int i = pathsDivergeIndex + 1; i < rangeMinPath.Count; i++) {
                        (int direction, RangeTreeNode subtree) = rangeMinPath[i];

                        // Edge case where right-most subtree is out of range
                        if (subtree.IsLeaf() && InRange(subtree.GetData())) {
                            canonicalSubsets.Add(subtree);

                        // Regular case
                        } else if (direction == left && subtree.Right() != null) {
                            canonicalSubsets.Add(subtree.Right());
                        }
                    }
                }

                // Handle high range
                // -> save coordNodes from left subtrees when path veers right
                // -> before doing this, ensure that paths aren't identical (ie all of the
                //    work hasn't already been finished in low range traversal)
                if (pathsDiverge) {
                    if (rangeMaxPath.Count == 1) {
                        // To avoid scenario where rightmost node is out of range
                        (int _, RangeTreeNode subtree) = rangeMaxPath[0];
                        if (subtree.IsLeaf() && InRange(subtree.GetData()))
                            canonicalSubsets.Add(subtree);
                    } else {
                        for (int i = pathsDivergeIndex + 1; i < rangeMaxPath.Count; i++) {
                            (int direction, RangeTreeNode subtree) = rangeMaxPath[i];
                        
                            if (subtree.IsLeaf() && InRange(subtree.GetData())) {
                                canonicalSubsets.Add(subtree);
                            } else if (direction == right && subtree.Left() != null) {
                                canonicalSubsets.Add(subtree.Left());
                            }
                        }
                    }
                }

                // Handle edge case where no nodes are in range
                if (canonicalSubsets.Count == 1 && canonicalSubsets[0].IsLeaf()) {
                    if (!(InRange(canonicalSubsets[0].GetData())))
                        canonicalSubsets = new List<RangeTreeNode>();
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

            // Find canonical subsets from final dimension,
            // Extract and combine lists of coordNodes
            List<RangeTreeNode> canonicalSubsetsInSearchRange = SearchRec(Root, 1);
            List<CoordNode> nodesInSearchRange = new List<CoordNode>();
            foreach (RangeTreeNode rtNode in canonicalSubsetsInSearchRange)
                nodesInSearchRange.AddRange(rtNode.GetCoordNodeList());

            // Return
            CoordNode[] nodesInSearchRangeArray = nodesInSearchRange.ToArray();
            if (sortOnDataAfterQuery) msn.Sort(nodesInSearchRangeArray, 0);
            return nodesInSearchRangeArray;
        }

        private RangeTreeNode BuildRangeTree(CoordNode[] coordSubset, int currDim) {

            // Instantiate current Node
            RangeTreeNode thisNode = new RangeTreeNode(coordSubset, currDim);

            // Build range tree for next dimension
            if (currDim < Dimensionality)
                thisNode.SetNextDimRoot(BuildRangeTree(coordSubset, currDim + 1));

            // Base case - check if leaf
            if (coordSubset.Length == 1) {
                thisNode.SetLocation(thisNode.GetCoordNodeList()[0].GetAttr(currDim));
                return thisNode;
            }

            // Sort coordSubset based on location in currDim
            msn.Sort(coordSubset, currDim);

            // Else recurse on left and right subsets of coordSubset
            int highestIndex = coordSubset.Length - 1;
            int midIndex = (int)Math.Floor((double)(highestIndex) / 2);

            CoordNode[] leftCoordSubset =     // Left side
                ArrayUtils.Subset(coordSubset, 0, midIndex);
            RangeTreeNode leftChild = BuildRangeTree(leftCoordSubset, currDim);
            thisNode.SetLeft(leftChild);
            leftChild.SetParent(thisNode);

            CoordNode[] rightCoordSubset =    // Right side
                ArrayUtils.Subset(coordSubset, midIndex + 1, highestIndex);
            RangeTreeNode rightChild = BuildRangeTree(rightCoordSubset, currDim);
            thisNode.SetRight(rightChild);
            rightChild.SetParent(thisNode);

            // Sort value = highest location in leftChild subtree
            int sortAttribute =
                leftCoordSubset[leftCoordSubset.Length - 1].GetAttr(currDim);
            thisNode.SetLocation(sortAttribute);

            return thisNode;
        }

        public RangeTreeNode GetRootByDimension(int dim) {
            // Helper function to return the root of this range tree in dimensions
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("GetRootByDimension method has invalid dimension " +
                                    $"parameter ({dim}). Must be 1, 2, or 3");
            RangeTreeNode root = Root;
            for (int i = 2; i <= dim; i++) root = root.NextDimRoot();
            
            return root;
        }

        public RangeTree(CoordNode[] coordNodes, int dimensionality) {
            if (coordNodes.Length == 0)
                throw new Exception("coordNodes is an empty list!");
            if (dimensionality < 1 || dimensionality > 3)
                throw new Exception("coordNodes has invalid dimensionality.");

            Dimensionality = dimensionality;
            Root = BuildRangeTree(coordNodes, 1);
        }
    }
}
