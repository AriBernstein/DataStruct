using System;

namespace Fractional_Cascading {

    public class RBTreeNode {
        private int Data;
        private bool Leaf, Red;
        private RBTreeNode ParentNode, LeftChild, RightChild;

        public int GetData() {
            return Data;
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

        public void SetRed() {
            Red = true;
        }

        public void SetBlack() {
            Red = false;
        }

        public bool IsRed() {
            return Red;
        }

        public bool IsBlack() {
            return !(Red);
        }

        public RBTreeNode Parent() {
            return ParentNode;
        }

        public void SetParent(RBTreeNode parent) {
            ParentNode = parent;
        }

        public RBTreeNode GrandParent() {
            return ParentNode.Parent();
        }

        public RBTreeNode Left() {
            return LeftChild;
        }

        public void SetLeft(RBTreeNode leftChild) {
            LeftChild = leftChild;
        }

        public RBTreeNode Right() {
            return RightChild;
        }

        public void SetRight(RBTreeNode rightChild) {
            RightChild = rightChild;
        }

        public int Size() {
            int nodeCount = 0;
            void FindSize(RBTreeNode n) {
                if (n == null) return;
                FindSize(n.Left());
                nodeCount++;
                FindSize(n.Right());
            }
            return nodeCount;
        }
        
        public RBTreeNode(int data, bool leaf=false, bool red = true) {
            Data = data;
            Leaf = leaf;
            Red = red;
        }

        // Overide default methods and operators
        public override bool Equals(object obj) {
            var nodeForComparison = obj as RBTreeNode;
            if (nodeForComparison == null) return false;
            else if (nodeForComparison.GetData() != Data) return false;
            else return true;
        }

        public static bool operator ==(RBTreeNode x, RBTreeNode y) {
            
            // Handle null equality check
            if (System.Object.ReferenceEquals(x, null)) {
                if (System.Object.ReferenceEquals(y, null)) return true;
                else return false;

            // Otherwise compare class instances
            } else return x.Equals(y);
        }

        public static bool operator !=(RBTreeNode x, RBTreeNode y) { 
            return !(x == y);
        }

        public static bool operator >(RBTreeNode x, RBTreeNode y) {
            return x.GetData() > y.GetData();
        }

        public static bool operator >=(RBTreeNode x, RBTreeNode y) {
            return x.GetData() >= y.GetData();
        }

        public static bool operator <(RBTreeNode x, RBTreeNode y) {
            return x.GetData() < y.GetData();
        }

        public static bool operator <=(RBTreeNode x, RBTreeNode y) {
            return x.GetData() <= y.GetData();
        }
        
        public override int GetHashCode() {
            return Data.GetHashCode();
        }
    }
}
