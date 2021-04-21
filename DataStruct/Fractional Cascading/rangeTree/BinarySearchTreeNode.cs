using System;

namespace Fractional_Cascading {

    public class BSTNode {
        // Key is the average of minIndex and maxIndex 
        private int Key;
        private bool Root;
        private bool Leaf;
        private bool Red;
        private BSTNode Parent;
        private BSTNode LeftChild;
        private BSTNode RightChild;
        
        public int GetKey() {
            return Key;
        }
        
        public bool isRoot() {
            return Root;
        }

        public bool isLeaf() {
            return Leaf;
        }
        public void makeLeaf() {
            Leaf = true;
        }

        public void changeColor() {
            Red = !(Red);
        }
        public bool isRed() {
            return Red;
        }
        public bool isBlack() {
            return !(Red);
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

        public BSTNode(int key, bool leaf=false, bool red = true, bool root=false) {
            Key = key;
            Leaf = leaf;
            Root = root;
            Red = red;
        }

    }
}