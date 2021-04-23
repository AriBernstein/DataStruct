using System;

namespace Fractional_Cascading {
    class RBTree {

        private RBTreeNode Root = null;
        private RBTreeHelper h = new RBTreeHelper();

        private void RotateRight(RBTreeNode y) {
            /**
            Make left subtree right subtree */

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

        private void RotateLeft(RBTreeNode x) {
            /**
            Make right subtree left subtree */

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

        private void RebalancePostInsert(RBTreeNode newNode) {
            /**
            Insert places newNode as an appropriate leaf. Now use the RED and BLACK labels
            of newNode's neighbors to balance.  */

            RBTreeNode u;   // Will denote pibling (aunt/uncle) or parent of newNode.
                            // ie. given newNode's grandparent (gP), u can be either gP's
                            // left or right child
            
            // While the parent of newNode (p) is RED.
            while (newNode.Parent().IsRed()) {

                // If p is the RIGHT pibling of newNode:
                if (newNode.Parent() == newNode.GrandParent().Right()) {
                    
                    // Set u as the LEFT pibling of newNode
                    u = newNode.GrandParent().Left();

                    // If the color of the LEFT pibling of newNode is RED, set both
                    // piblings of newNode to BLACK, set gP to RED, assign newNode = gP.
                    if (u != null && u.IsRed()) {
                        u.SetBlack();
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        newNode = newNode.GrandParent();
                    
                    } else {  // If the color of the LEFT pibling is BLACK

                        // If newNode is the left child of p, set
                        // newNode = p and right rotate newNode
                        if (newNode == newNode.Parent().Left()) {
                            newNode = newNode.Parent();
                            RotateRight(newNode);
                        }
                        
                        // Set p to BLACK and gP to RED. Then left rotate gP.
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        RotateLeft(newNode.GrandParent());
                    }
                } else {
                    // if p is the left child of grandParent gP of z
                    u = newNode.GrandParent().Right();

                    // If the color of the right child of gP of z is RED, set the color of
                    // both the children of gP as BLACK and the color of gP as RED. Then
                    // assign gP to newNode.
                    if (u != null && u.IsRed()) {
                        u.SetBlack();
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        newNode = newNode.GrandParent();
                    } else {
                        // Else if newNode is the right child of p then, assign p to
                        // newNode. Then left rotate newNode.
                        if (newNode == newNode.Parent().Right()) {
                            newNode = newNode.Parent();
                            RotateLeft(newNode);
                        }

                        // Set color of p as BLACK and color of gP as RED. Then right
                        // rotate gp. 
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        RotateRight(newNode.GrandParent());
                    }
                }
                
                if (newNode == Root) break;
            }
            Root.SetBlack();
        }

        public void Insert(int data) {

            RBTreeNode newNode;
            RBTreeNode y = null; // Will represent a leaf node
            RBTreeNode x;

            if(Root == null) { // Empty tree
                Root = new RBTreeNode(data, red:false);
                return;
            } 

            newNode = new RBTreeNode(data);
            x = Root;

            //  Traverse with x until y is a leaf node
            while (x != null) {
                y = x;
                if(newNode < x) x = x.Left();
                else x = x.Right();
            }

            //  Make newNode child of y
            newNode.SetParent(y);
            if (newNode < y) y.SetLeft(newNode);
            else y.SetRight(newNode);

            // In this case, the tree is already balanced :)
            if(newNode.GrandParent() == null) return;
            else RebalancePostInsert(newNode);
        }

        public string TraverseTree(int order) {
            return h.PrintTreeTraversalOrder(order, Root);
        }

        public void PrintTree(int verticalSpacing=1, int indentPerLevel=5) {
            h.PrintSubTree(Root, verticalSpacing, indentPerLevel);
        }

        public RBTreeNode GetRoot() {
            return Root;
        }
    }
}