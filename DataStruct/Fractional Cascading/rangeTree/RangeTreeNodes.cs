using System;

namespace Fractional_Cascading {
    interface RangeTreeNodeBase {
        int GetDim();
        bool IsLeaf();
        RangeTreeNodeBase GetNextDimNode();
    }
    
    class RangeTreeNode : RangeTreeNodeBase {
        // Key is the average of minIndex and maxIndex 
        private bool root;
        private int dimension;
        private int minIndex;
        private int maxIndex;
        private RangeTreeNode nextDimBranch;
        private RangeTreeNodeBase parent;
        private RangeTreeNodeBase leftChild;
        private RangeTreeNodeBase rightChild;
        
        public void SetNextDimNode(RangeTreeNode nextDimBranchNode) {
            if(nextDimBranch.IsLeaf())
                throw new Exception("nextDimBranchNode must be a BranchNode");
            else
                nextDimBranch = nextDimBranchNode;
        }
        public RangeTreeNodeBase GetNextDimNode() {
            return nextDimBranch;
        }
        
        public void SetLeftChild(RangeTreeNodeBase leftChildNode)  {
            leftChild = leftChildNode;
        }
        public RangeTreeNodeBase GetLeftChild() {
            return leftChild;
        }

        public void SetRightChild(RangeTreeNodeBase rightChildNode)  {
            rightChild = rightChildNode;
        }
        public RangeTreeNodeBase GetRightChild() {
            return rightChild;
        }

        public int GetDim() {
            return dimension;
        }
        public bool IsRoot() {
            return root;
        }
        public bool IsLeaf() {
            return false;
        }
        public RangeTreeNode(int dimensionVal, RangeTreeNode parentNode, int minIndexVal,
                             int maxIndexVal, bool rootVal=false) {
            dimension = dimensionVal;
            parent = parentNode;
            minIndex = minIndexVal;
            maxIndex = maxIndexVal;
            root = rootVal;
        }
    }

    class RangeTreeLeaf: RangeTreeNodeBase {
        private int dimension;
        private int location;   // location in dimension
        private RangeTreeLeaf nextDimLeaf;
        private RangeTreeNode parent;
        private DataNode data;
        public int GetDim() {
            return dimension;
        }

        public bool IsLeaf() {
            return true;
        }

        public void SetNextDimNode(RangeTreeLeaf nextDimLeafNode) {
            if(!(nextDimLeafNode.IsLeaf()))
                throw new Exception("nextDimLeafNode must be a LeafNode");
            else
                nextDimLeaf = nextDimLeafNode;
        }

        public void SetParent(RangeTreeNode parentNode) {
            parent = parentNode;
        }
        public RangeTreeNode GetParent() {
            return parent;
        }

        public RangeTreeNodeBase GetNextDimNode() {
            return nextDimLeaf;
        }

        public RangeTreeLeaf(DataNode dataNode, int dimensionVal, int locationVal) {
            dimension = dimensionVal;
            location = locationVal;
            data = dataNode;
        }
    }
}