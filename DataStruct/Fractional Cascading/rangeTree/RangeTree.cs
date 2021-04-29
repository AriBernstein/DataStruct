using System;

namespace Fractional_Cascading {
    class RangeTree {
        private int Dimensionality;
        private int Size;
        private RangeTreeNode Root; // Of x dimension

        public RangeTreeNode GetRoot() {
            return Root;
        }

        private void PrepSingleCoordNodes(CoordNode[] coordNodes) {
            
            // Populate SingleCoordNode lists
            SingleCoordNode[] dimOneNodes = new SingleCoordNode[coordNodes.Length];
            SingleCoordNode[] dimTwoNodes = new SingleCoordNode[coordNodes.Length];
            SingleCoordNode[] dimThreeNodes = new SingleCoordNode[coordNodes.Length];

            for (int i = 0; i < Size; i++) {
                CoordNode coordNode = coordNodes[i];
                DataNode dataNode = coordNode.GetDataNode();
                dimOneNodes[i] = new SingleCoordNode(dataNode, coordNode.GetAttr(1));
                if (Dimensionality >= 2)
                    dimTwoNodes[i] = new SingleCoordNode(dataNode, coordNode.GetAttr(2));
                if (Dimensionality == 3)
                    dimThreeNodes[i] = new SingleCoordNode(dataNode, coordNode.GetAttr(3));
            }

            // Sort lists by location
            MergeSortNodes msn = new MergeSortNodes();
            msn.Sort(dimOneNodes, 1);
            msn.Sort(dimTwoNodes, 1);
            msn.Sort(dimThreeNodes, 1);
            SingleCoordNode[][] singleCoords =
                new SingleCoordNode[][]{dimOneNodes, dimTwoNodes, dimThreeNodes};
        }

        public void BuildRangeTree(SingleCoordNode[][] coords) {
        }

        public RangeTree(CoordNode[] coordNodes) {
            Dimensionality = coordNodes[0].GetDimensionality();
            if (Dimensionality < 1 || Dimensionality > 3)
                throw new Exception("coordNodes has invalid dimensionality ");

            // Populate xLeaves, yLeaves, and/or zLeaves depending on dimensionality

        }
    }
}
