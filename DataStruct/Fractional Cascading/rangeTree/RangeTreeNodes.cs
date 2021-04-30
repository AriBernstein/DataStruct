using System;

namespace Fractional_Cascading {
    
    class RangeTreeNode {
        // Key is the average of minIndex and maxIndex 
        private CoordNode[] Data;
        private int Dimension;
        private int Location;   // If leaf, the location in Dimension of the single node
                                // in Data. Else, the highest Location value in Data
        private RangeTreeNode NextDimRoot;
        private RangeTreeNode ParentNode;
        private RangeTreeNode LeftChild;
        private RangeTreeNode RightChild;

        public RangeTreeNode GetNextDimRoot() {
            return NextDimRoot;
        }

        public void SetNextDimRoot(RangeTreeNode root) {
            NextDimRoot = root;
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
            Location = data[data.Length - 1].GetAttr(dimension);
        }

        public override string ToString() {
            if(IsLeaf()) return $"Leaf: {Data[0].ToString()}";
            else return $"[Size: {Data.Length}, Index range: " +
                        $"location range: ({Data[0].GetAttr(Dimension)}, " +
                        $"{Data[Data.Length - 1].GetAttr(Dimension)})]";
        }
    }
}