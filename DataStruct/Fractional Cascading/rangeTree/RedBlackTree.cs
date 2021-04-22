using System;
using System.IO;

namespace Fractional_Cascading {
    class RBTree {
        /**
        LLRBTree -> Left Leaning Red Black Tree */

        private RBTreeNode Root = null;
        private RBTreeHelper h = new RBTreeHelper();

        Utils u = new Utils();

        public void Insert(int key) {
            Root = Insert(Root, key);
        }

        private RBTreeNode Insert(RBTreeNode curr, int key) {
            
            // lil sapling :)
            if (curr == null) return new RBTreeNode(key);
            
            // nodes with duplicate keys (in a single dimension) should be impossible
            // under current node generation implementation
            if (key == curr.GetKey()) throw new Exception("Attempting insert of node " +
                                                          "with key already present in " +
                                                          "Red Black Tree.");

            // recurse left if key less than that of current node
            if (key < curr.GetKey()) curr.SetLeftChild(Insert(curr.GetLeftChild(), key));
            
            // recurse right if key >= current node
            else curr.SetRightChild(Insert(curr.GetRightChild(), key));

            return curr;
        }

        public void RotateRight(RBTreeNode y) {
            // setup x & y
            RBTreeNode x = y.GetLeftChild();
            y.SetLeftChild(x.GetRightChild());
            
            // if x has a right child, make y the parent of the right child of x.
            if (x.GetRightChild() != null) x.GetRightChild().SetParent(y);
            x.SetParent(y.GetParent());
            
            // if y has no parent, make x the root of the tree.
            if (y.GetParent() == null) Root = x;
            
            // else if y is the right child of its parent p, make x the right child of p.
            else if (y == y.GetParent().GetRightChild()) y.GetParent().SetRightChild(x);
            
            // else assign x as the left child of p.
            else y.GetParent().SetLeftChild(x);

            // make x the parent of y.
            x.SetRightChild(y);
            y.SetParent(x);
        }


        public void RotateLeft(RBTreeNode x) {
            // setup x & y
            RBTreeNode y = x.GetRightChild();
            x.SetRightChild(y.GetLeftChild());

            // if y has a left child, make x the parent of the left child of y.
            if (y.GetLeftChild() != null) y.GetLeftChild().SetParent(x);
            y.SetParent(x.GetParent());

            // if x has no parent, make y the root of the tree.
            if (x.GetParent() == null) Root = y;
            
            // else if x is the left child of p, make y the left child of p.
            else if (x == x.GetParent().GetLeftChild()) x.GetParent().SetLeftChild(y);

            // else make y the right child of p
            else x.GetParent().SetRightChild(y);

            // make y the parent of x
            y.SetLeftChild(x);
            x.SetParent(y);
        }

        public string TraverseTree(int order) {
            return h.TraverseTree(order, Root);
        }

        public RBTreeNode GetRoot() {
            return Root;
        }
    }
}