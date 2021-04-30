using System;

namespace Fractional_Cascading {
    
    class RangeTreeNode {
        // Key is the average of minIndex and maxIndex 
        private SingleCoordNode[] Data;
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

        public SingleCoordNode[] GetNodeList() {
            return Data;
        }

        public RangeTreeNode(SingleCoordNode[] data, int dimension) {
            Data = data;
            Dimension = dimension;
            Location = data[data.Length - 1].GetLocation();
        }

        public override string ToString() {
            if(IsLeaf()) return $"Leaf: {Data[0].ToString()}";
            else return $"[Size: {Data.Length}, Index range: " +
                        $"location range: ({Data[0].GetLocation()}, " +
                        $"{Data[Data.Length - 1].GetLocation()})]";
        }
    }
}