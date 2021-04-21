using System;

namespace Fractional_Cascading {

    public class RBTreeNode {
        private int Key;
        private bool Leaf, Red;
        private RBTreeNode Parent, LeftChild, RightChild;

        public int GetKey() {
            return Key;
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

        public RBTreeNode GetParent() {
            return Parent;
        }
        public void SetParent(RBTreeNode parent) {
            Parent = parent;
        }

        public RBTreeNode GetLeftChild() {
            return LeftChild;
        }
        public void SetLeftChild(RBTreeNode leftChild) {
            LeftChild = leftChild;
        }

        public RBTreeNode GetRightChild() {
            return RightChild;
        }
        public void SetRightChild(RBTreeNode rightChild) {
            RightChild = rightChild;
        }

        public RBTreeNode(int key, bool leaf=false, bool red = true) {
            Key = key;
            Leaf = leaf;
            Red = red;
        }

    }
}