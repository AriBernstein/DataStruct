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
        
        public bool IsRoot() {
            return Root;
        }

        public bool IsLeaf() {
            return Leaf;
        }
        public void MakeLeaf() {
            Leaf = true;
        }

        public void ChangeColor() {
            Red = !(Red);
        }
        public bool IsRed() {
            return Red;
        }
        public bool IsBlack() {
            return !(Red);
        }

        public BSTNode GetParent() {
            return Parent;
        }
        public void SetParent(BSTNode parent) {
            Parent = parent;
        }

        public BSTNode GetLeftChild() {
            return LeftChild;
        }
        public void SetLeftChild(BSTNode leftChild) {
            LeftChild = leftChild;
        }

        public BSTNode GetRightChild() {
            return RightChild;
        }
        public void SetRightChild(BSTNode rightChild) {
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