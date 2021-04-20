using System;

namespace Fractional_Cascading {
    class RangeTree {
        private int dimensionality;
        private RangeTreeLeaf[] xLeaves;    // leaves sorted on first dimension locations
        private RangeTreeLeaf[] yLeaves;    // leaves sorted on second dimension locations
        private RangeTreeLeaf[] zLeaves;    // leaves sorted on third dimension locations
        private RangeTreeNode treeRoot;
        MergeSortNodes msn = new MergeSortNodes();


        public RangeTreeNode getRoot() {
            return treeRoot;
        }

        private RangeTreeNode buildTree(CoordNode[] coordNodes, int dimension) {
            if(dimension < dimensionality)  // build top-down
                buildTree(coordNodes, dimension + 1);
            
            RangeTreeLeaf[] sortedNodes;
            if(dimension == 1) sortedNodes = xLeaves;
            else if (dimension == 2) sortedNodes = yLeaves;
            else if (dimension == 3) sortedNodes = zLeaves;
            else throw new Exception("WATT?");

            
            return new RangeTreeNode(1, treeRoot,0,0);
        }

        public RangeTree(CoordNode[] coordNodes) {
            dimensionality = coordNodes[0].GetDimensionality();
            if(dimensionality < 1 || dimensionality > 3)
                throw new Exception("coordNodes has invalid dimensionality ");

            // Populate xLeaves, yLeaves, and/or zLeaves depending on dimensionality
            for (int i = 0; i < dimensionality; i++) {
                int currDim = i + 1;
                msn.Sort(coordNodes, currDim);
                RangeTreeLeaf[] currList = new RangeTreeLeaf[coordNodes.Length];
                for(int j = 0; j < currList.Length; j++)
                    currList[j] = new RangeTreeLeaf(coordNodes[j].GetDataNode(), currDim,
                                                    coordNodes[j].GetAttr(currDim));
                if(currDim == 1) xLeaves = currList;
                else if(currDim == 2) yLeaves = currList;
                else if(currDim == 3) zLeaves = currList;
                else throw new Exception("WAT?");
            }
        }
    }
}