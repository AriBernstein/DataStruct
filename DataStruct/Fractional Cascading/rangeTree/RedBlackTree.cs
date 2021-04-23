using System;

namespace Fractional_Cascading {
    class RBTree {

        private RBTreeNode Root = null;
        private RBTreeHelper h = new RBTreeHelper();

        Utils u = new Utils();

        public void RotateRight(RBTreeNode y) {
            // setup x & y
            RBTreeNode x = y.Left();
            y.SetLeft(x.Right());
            
            // if x has a right child, make y the parent of the right child of x.
            if (x.Right() != null) x.Right().SetParent(y);
            x.SetParent(y.Parent());
            
            // if y has no parent, make x the root of the tree.
            if (y.Parent() == null) Root = x;
            
            // else if y is the right child of its parent p, make x the right child of p.
            else if (y == y.Parent().Right()) y.Parent().SetRight(x);
            
            // else assign x as the left child of p.
            else y.Parent().SetLeft(x);

            // make x the parent of y.
            x.SetRight(y);
            y.SetParent(x);
        }


        public void RotateLeft(RBTreeNode x) {
            // setup x & y
            RBTreeNode y = x.Right();
            x.SetRight(y.Left());

            // if y has a left child, make x the parent of the left child of y.
            if (y.Left() != null) y.Left().SetParent(x);
            y.SetParent(x.Parent());

            // if x has no parent, make y the root of the tree.
            if (x.Parent() == null) Root = y;
            
            // else if x is the left child of p, make y the left child of p.
            else if (x == x.Parent().Left()) x.Parent().SetLeft(y);

            // else make y the right child of p
            else x.Parent().SetRight(y);

            // make y the parent of x
            y.SetLeft(x);
            x.SetParent(y);
        }

        private void ReadjustPostInsert(RBTreeNode newNode) {

            RBTreeNode u;   // Will denote "pibling" of newNode; ie. if newNode's parent
                            // is the left child of newNode's grandparent, u = newNode's
                            // grandparent's right child and vice versa.
            
            // While the parent of newNode (p) is RED.
            while (newNode.Parent().IsRed()) {

                // If p is the right child of grandParent (gP) of left child (z):
                if (newNode.Parent() == newNode.GrandParent().Right()) {

                    u = newNode.GrandParent().Left();

                    if (u.IsRed()) {
                    // If the color of the left child of gP of z is RED, set the color of
                    // both the children of gP as BLACK, set the color of gP as RED, and
                    // assign gP to newNode.
                        u.SetBlack();
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        newNode = newNode.GrandParent();
                    } else {
                    // Else if newNode is the left child of p then, assign p to newNode
                    // and Right-Rotate newNode.
                        if (newNode == newNode.Parent().Left()) {
                            newNode = newNode.Parent();
                            RotateRight(newNode);
                        }
                        // Set color of p as BLACK and color of gP as RED.
                        // Then Left-Rotate gP.
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        RotateLeft(newNode.GrandParent());
                    }
                } else {
                    // p is the left child of grandParent gP of z
                    u = newNode.GrandParent().Right();

                    if (u.IsRed()) {
                    // If the color of the right child of gP of z is RED, set the color of
                    // both the children of gP as BLACK and the color of gP as RED. Then
                    // assign gP to newNode.
                        u.SetBlack();
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        newNode = newNode.GrandParent();
                    } else {
                        // Else if newNode is the right child of p then, assign p to
                        // newNode. Then rotate left newNode.
                        if (newNode == newNode.Parent().Right()) {
                            newNode = newNode.Parent();
                            RotateLeft(newNode);
                        }

                        // Set color of p as BLACK and color of gP as RED. Then rotate
                        // right gp. 
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        RotateRight(newNode.GrandParent());
                    }
                }
                
                if (newNode == Root) break;
            }
            Root.SetBlack();
        }

        private RBTreeNode Insert(RBTreeNode root, int data) {
            
            // lil sapling :)
            if (root == null) return new RBTreeNode(data);
            
            // nodes with duplicate data values (in a single dimension) should be
            // impossible under current implementation of node generation
            if (data == root.GetData())
                throw new Exception("Attempting insert of node with data value already " +
                                    "present in Red Black Tree.");

            // recurse left if data value is less than that of current node
            if (data < root.GetData()) root.SetLeft(Insert(root.Left(), data));
            
            // recurse right if data value is >= current node
            else root.SetRight(Insert(root.Right(), data));

            return root;
        }
        
        public void Insert(int data) {
            Root = Insert(Root, data);
        }

        public string TraverseTree(int order) {
            return h.PrintTreeTraversalOrder(order, Root);
        }

        public void PrintTree() {
            h.PrintSubTree(Root);
            // h.print2D(Root);
        }

        public RBTreeNode GetRoot() {
            return Root;
        }
    }
}