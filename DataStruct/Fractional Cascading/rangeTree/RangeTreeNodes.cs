using System;

namespace Fractional_Cascading {
    
    class RangeTreeNode {
        // Key is the average of minIndex and maxIndex 
        private SingleCoordNode[] Data;
        private int Dimension;
        private int Location;   // If leaf, the location in Dimension of the single node
                                // in Data. Else, the highest Location value in Data
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

        public bool IsLeaf() {
            return Data.Length == 1;
        }

        public SingleCoordNode[] GetNodeList() {
            return Data;
        }

        public RangeTreeNode(SingleCoordNode[] data, int dimension, RangeTreeNode parent,
                             int minIndex, int maxIndex) {
            Data = data;
            Dimension = dimension;
            Parent = parent;
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Location = data[maxIndex].GetLocation();
        }

        public override string ToString() {
            if(IsLeaf()) return $"Leaf: {Data[0].ToString()}";
            else return $"[Size: {MaxIndex - MinIndex}, Index range: " +
                        $"({MinIndex}, {MaxIndex}), location range: " +
                        $"({Data[MinIndex].GetLocation()}, " +
                        $"{Data[MaxIndex].GetLocation()})]";
        }
    }
}