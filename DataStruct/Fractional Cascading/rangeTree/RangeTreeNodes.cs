using System;

namespace Fractional_Cascading {
    interface RangeTreeNodeBase {
        int getDim();
        bool isLeaf();
        RangeTreeNodeBase getNextDimNode();
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
        
        public void setNextDimNode(RangeTreeNode nextDimBranchNode) {
            if(nextDimBranch.isLeaf())
                throw new Exception("nextDimBranchNode must be a BranchNode");
            else
                nextDimBranch = nextDimBranchNode;
        }
        public RangeTreeNodeBase getNextDimNode() {
            return nextDimBranch;
        }
        
        public void setLeftChild(RangeTreeNodeBase leftChildNode)  {
            leftChild = leftChildNode;
        }
        public RangeTreeNodeBase getLeftChild() {
            return leftChild;
        }

        public void setRightChild(RangeTreeNodeBase rightChildNode)  {
            rightChild = rightChildNode;
        }
        public RangeTreeNodeBase getRightChild() {
            return rightChild;
        }

        public int getDim() {
            return dimension;
        }
        public bool isRoot() {
            return root;
        }
        public bool isLeaf() {
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
        public int getDim() {
            return dimension;
        }

        public bool isLeaf() {
            return true;
        }

        public void setNextDimNode(RangeTreeLeaf nextDimLeafNode) {
            if(!(nextDimLeafNode.isLeaf()))
                throw new Exception("nextDimLeafNode must be a LeafNode");
            else
                nextDimLeaf = nextDimLeafNode;
        }

        public void setParent(RangeTreeNode parentNode) {
            parent = parentNode;
        }
        public RangeTreeNode getParent() {
            return parent;
        }

        public RangeTreeNodeBase getNextDimNode() {
            return nextDimLeaf;
        }

        public RangeTreeLeaf(DataNode dataNode, int dimensionVal, int locationVal) {
            dimension = dimensionVal;
            location = locationVal;
            data = dataNode;
        }
    }
}