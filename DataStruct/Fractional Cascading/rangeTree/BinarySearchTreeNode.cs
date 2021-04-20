using System;

namespace Fractional_Cascading {

    class BSTNode {
        // Key is the average of minIndex and maxIndex 
        private int Key;
        private bool Root;
        private int Dimension;
        private bool Leaf;
        private BSTNode Parent;
        private BSTNode LeftChild;
        private BSTNode RightChild;
        
        public int getKey() {
            return Key;
        }
        
        public bool isRoot() {
            return Root;
        }

        public bool isLeaf() {
            return Leaf;
        }

        public int getDim() {
            return Dimension;
        }

        public BSTNode getParent() {
            return Parent;
        }
        public void setParent(BSTNode parent) {
            Parent = parent;
        }

        public BSTNode getLeftChild() {
            return LeftChild;
        }
        public void setLeftChild(BSTNode leftChild) {
            LeftChild = leftChild;
        }

        public BSTNode getRightChild() {
            return RightChild;
        }
        public void setRightChild(BSTNode rightChild) {
            RightChild = rightChild;
        }

        public BSTNode(int key, bool leaf=false, bool root=false) {
            Key = key;
            Leaf = leaf;
            Root = root;
        }

    }
}