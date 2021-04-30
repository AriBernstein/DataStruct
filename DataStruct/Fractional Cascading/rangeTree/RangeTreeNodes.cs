using System;

namespace Fractional_Cascading {
    
    class RangeTreeNode {
        // Key is the average of minIndex and maxIndex 
        private CoordNode[] Data;
        private int Dimension;
        private int LocationVal; // If leaf, location in Dimension of the node in Data
                                 // Else, highest location value in the LeftChild subtree
        private RangeTreeNode NextDimRootNode;
        private RangeTreeNode ParentNode;
        private RangeTreeNode LeftChild;
        private RangeTreeNode RightChild;

        public RangeTreeNode NextDimRoot() {
            return NextDimRootNode;
        }

        public void SetNextDimRoot(RangeTreeNode root) {
            NextDimRootNode = root;
        }

        public int GetLocation() {
            return LocationVal;
        }

        public void SetLocation(int location) {
            LocationVal = location;
        }

        public void SetParent(RangeTreeNode parent) {
            ParentNode = parent;
        }

        public RangeTreeNode Parent() {
            return ParentNode;
        }
        
        public void SetLeft(RangeTreeNode leftChildNode)  {
            LeftChild = leftChildNode;
        }
        
        public RangeTreeNode Left() {
            return LeftChild;
        }

        public void SetRight(RangeTreeNode rightChildNode)  {
            RightChild = rightChildNode;
        }

        public RangeTreeNode Right() {
            return RightChild;
        }

        public int GetDim() {
            return Dimension;
        }

        public bool IsLeaf() {
            return Data.Length == 1;
        }

        public CoordNode[] GetNodeList() {
            return Data;
        }

        public RangeTreeNode(CoordNode[] data, int dimension) {
            if (dimension < 1 || dimension > 3) 
                throw new Exception("Invalid dimensionality value, must be 1, 2, or 3.");
            Data = data;
            Dimension = dimension;
        }

        public override string ToString() {
            if(IsLeaf()) return $"Leaf: {Data[0].ToString()}";
            else return $"[Size: {Data.Length}, Index range: " +
                        $"location range: ({Data[0].GetAttr(Dimension)}, " +
                        $"{Data[Data.Length - 1].GetAttr(Dimension)})]";
        }
    }
}
