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

        private (SingleCoordNode[], SingleCoordNode[], SingleCoordNode[]) SepCoordNodes(
                                                                CoordNode[] coordNodes) {
            SingleCoordNode[] dimOneNodes = new SingleCoordNode[coordNodes.Length];
            SingleCoordNode[] dimTwoNodes = new SingleCoordNode[coordNodes.Length];
            SingleCoordNode[] dimThreeNodes = new SingleCoordNode[coordNodes.Length];

            for(int i = 0; i < Size; i++) {
                CoordNode coordNode = coordNodes[i];
                DataNode dataNode = coordNode.GetDataNode();
                dimOneNodes[i] = new SingleCoordNode(dataNode, coordNode.GetAttr(1));
                if(Dimensionality >= 2)
                    dimTwoNodes[i] = new SingleCoordNode(dataNode, coordNode.GetAttr(2));
                if(Dimensionality == 3)
                    dimThreeNodes[i] = new SingleCoordNode(dataNode, coordNode.GetAttr(3));
            }
            return (dimOneNodes, dimTwoNodes, dimThreeNodes);
        }

        public void BuildRangeTree(SingleCoordNode[] coords) {
            
        }

        public RangeTree(CoordNode[] coordNodes) {
            Dimensionality = coordNodes[0].GetDimensionality();
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("coordNodes has invalid dimensionality ");

            // Populate xLeaves, yLeaves, and/or zLeaves depending on dimensionality
            SingleCoordNode[] dimOneNodes;
            SingleCoordNode[] dimTwoNodes;
            SingleCoordNode[] dimThreeNodes;
            (dimOneNodes, dimTwoNodes, dimThreeNodes) = SepCoordNodes(coordNodes);


        }
    }
}
