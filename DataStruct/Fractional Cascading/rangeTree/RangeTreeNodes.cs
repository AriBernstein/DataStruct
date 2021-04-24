using System;

namespace Fractional_Cascading {
    interface RangeTreeNodeBase {
        int GetDim();
        bool IsLeaf();
        RangeTreeNodeBase GetNextDimRoot();
    }
    
    class RangeTreeNode : RangeTreeNodeBase {
        // Key is the average of minIndex and maxIndex 
        private bool Root;
        private int Dimension;
        private int MinIndex;
        private int MaxIndex;
        private RangeTreeNode NextDimRoot;
        private RangeTreeNodeBase Parent;
        private RangeTreeNodeBase LeftChild;
        private RangeTreeNodeBase RightChild;
        
        public void SetNextDimNode(RangeTreeNode nextDimBranchNode) {
            if (NextDimRoot.IsLeaf())
                throw new Exception("nextDimBranchNode must be a BranchNode");
            else
                NextDimRoot = nextDimBranchNode;
        }

        public RangeTreeNodeBase GetNextDimRoot() {
            return NextDimRoot;
        }
        
        public void SetLeftChild(RangeTreeNodeBase leftChildNode)  {
            LeftChild = leftChildNode;
        }

        public RangeTreeNodeBase GetLeftChild() {
            return LeftChild;
        }

        public void SetRightChild(RangeTreeNodeBase rightChildNode)  {
            RightChild = rightChildNode;
        }

        public RangeTreeNodeBase GetRightChild() {
            return RightChild;
        }

        public int GetDim() {
            return Dimension;
        }
        public bool IsRoot() {
            return Root;
        }
        public bool IsLeaf() {
            return false;
        }
        public RangeTreeNode(int dimension, RangeTreeNode parent, int minIndex,
                             int maxIndex, bool root=false) {
            Dimension = dimension;
            Parent = parent;
            MinIndex = minIndex;
            MaxIndex = maxIndex;
            Root = root;
        }
    }

    class RangeTreeLeaf: RangeTreeNodeBase {
        private int Dimension;
        private int Location;   // location in dimension
        private RangeTreeLeaf NextDimLeaf;
        private RangeTreeNode Parent;
        private DataNode Data;

        public int GetDim() {
            return Dimension;
        }

        public bool IsLeaf() {
            return true;
        }

        public void SetNextDimNode(RangeTreeLeaf nextDimLeafNode) {
            if (!(nextDimLeafNode.IsLeaf()))
                throw new Exception("nextDimLeafNode must be a LeafNode");
            else
                NextDimLeaf = nextDimLeafNode;
        }

        public void SetParent(RangeTreeNode parentNode) {
            Parent = parentNode;
        }
        
        public RangeTreeNode GetParent() {
            return Parent;
        }

        public RangeTreeNodeBase GetNextDimRoot() {
            return NextDimLeaf;
        }

        public RangeTreeLeaf(DataNode dataNode, int dimensionVal, int locationVal) {
            Dimension = dimensionVal;
            Location = locationVal;
            Data = dataNode;
        }
    }
}
