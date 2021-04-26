using System;

namespace Fractional_Cascading {
    class RangeTree {
        private int Dimensionality;
        private int Size;
        private RangeTreeNode[] xLeaves;    // leaves sorted on first dimension locations
        private RangeTreeNode[] yLeaves;    // leaves sorted on second dimension locations
        private RangeTreeNode[] zLeaves;    // leaves sorted on third dimension locations
        private RangeTreeNode Root;
        MergeSortNodes msn = new MergeSortNodes();

        public RangeTreeNode GetRoot() {
            return Root;
        }

        private (SinCoordNode[], SinCoordNode[], SinCoordNode[]) SeparateCoordNodes(CoordNode[] coordNodes) {
            SinCoordNode[] dimOneNodes = new SinCoordNode[coordNodes.Length];
            SinCoordNode[] dimTwoNodes = new SinCoordNode[coordNodes.Length];
            SinCoordNode[] dimThreeNodes = new SinCoordNode[coordNodes.Length];

            for(int i = 0; i < Size; i++) {
                CoordNode coordNode = coordNodes[i];
                DataNode dataNode = coordNode.GetDataNode();
                dimOneNodes[i] = new SinCoordNode(dataNode, coordNode.GetAttr(1));
                if(Dimensionality >= 2)
                    dimTwoNodes[i] = new SinCoordNode(dataNode, coordNode.GetAttr(2));
                if(Dimensionality == 3)
                    dimThreeNodes[i] = new SinCoordNode(dataNode, coordNode.GetAttr(3));
            }

            return (dimOneNodes, dimTwoNodes, dimThreeNodes);
        }

        // public RangeTree(CoordNode[] coordNodes) {
        //     Dimensionality = coordNodes[0].GetDimensionality();
        //     if (Dimensionality < 1 || Dimensionality > 3)
        //         throw new Exception("coordNodes has invalid dimensionality ");

        //     // Populate xLeaves, yLeaves, and/or zLeaves depending on dimensionality
        //     for (int i = 0; i < Dimensionality; i++) {
        //         int currDim = i + 1;
        //         msn.Sort(coordNodes, currDim);
        //         RangeTreeNode[] currList = new RangeTreeNode[coordNodes.Length];
        //         for (int j = 0; j < currList.Length; j++)
        //             currList[j] = new RangeTreeNode(coordNodes[j].GetDataNode(), currDim,
        //                                             coordNodes[j].GetAttr(currDim));
        //         if      (currDim == 1) xLeaves = currList;
        //         else if (currDim == 2) yLeaves = currList;
        //         else if (currDim == 3) zLeaves = currList;
        //         else throw new Exception("WAT?");
        //     }
        // }
    }
}
