using System;

namespace Fractional_Cascading {
    
    class RangeTreeNode {
        // Key is the average of minIndex and maxIndex 
        private bool Root;
        private SinCoordNode[] Data;
        private int Dimension;
        private int Location;   // Coordinate in Dimension
        private int MinIndex;
        private int MaxIndex;
        private RangeTreeNode NextDimRoot;
        private RangeTreeNode Parent;
        private RangeTreeNode LeftChild;
        private RangeTreeNode RightChild;

        public RangeTreeNode GetNextDimRoot() {
            return NextDimRoot;
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

        public bool IsRoot() {
            return Root;
        }

        public bool IsLeaf() {
            return Data.Length == 1;
        }

        public SinCoordNode[] GetData() {
            return Data;
        }

        public RangeTreeNode(SinCoordNode[] data, int dimension, RangeTreeNode parent,
                             int minIndex, int maxIndex, bool root=false) {
            Data = data;
            Dimension = dimension;
            Parent = parent;
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Root = root;
        }
    }
}