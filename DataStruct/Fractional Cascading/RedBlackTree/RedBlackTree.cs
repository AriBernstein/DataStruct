using System;

namespace Fractional_Cascading {
    class RBTree {
        /**
        Implementation of a Red-Black tree, a type of self-balancing binary search trees. 
        Here, each node is colored either red or black. If the following color-related
        conditions are met, the tree will be balanced such that search, insertion, and
        deletion (not implemented here) can be accomplished in O(log n) time in all cases.
        
        Red-Black conditions:
            1. Root of the tree is always black.
            2. No two successive red nodes (red node can have a red parent or red child.
            3. Every path from a given node to any leaf in its subtree will always have
               the same number of black nodes.

        Note that meeting these conditions will not result in a perfectly balanced tree.
        They guarantee that the tree's height is less than or equal to 2 * Log2(n+1),
        which means only one more step during search than a perfectly balanced tree. Also
        note that this slight imbalance comes with the advantage of fewer rotations,
        which make for faster insertion and deletion.   */

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

        private void Transplant(RBTreeNode oldNode, RBTreeNode newNode) {
            // Replace oldNode with newNode
            RBTreeNode parent = oldNode.Parent();
            if (parent == null) Root = newNode;
            else if (oldNode == parent.Left()) parent.SetLeft(newNode);
            else parent.SetRight(newNode);

            // Null check for when newNode is null
            if(newNode != null) newNode.SetParent(parent);
        }

        private RBTreeNode Minimum(RBTreeNode root) {
            // Find the node with the smallest sort-value in the given subtree
            while(root.Left() != null) {
                root = root.Left();
            }
            return root;
        }

        public void Delete(RBTreeNode node, int value) {
            // Traverse until we find nodeToDelete
            RBTreeNode nodeToDelete = null;
            while(node != null) {
                if (node.GetData() == value) {
                    nodeToDelete = node;
                    break;
                }
                node = (value < node.GetData()) ? node.Left() : node.Right();
            }
            if (node == null) throw new Exception("Cannot find node to delete with " +
                                                  $"value {value} in Red Black Tree");

            RBTreeNode x, y;

            // Save the color of nodeToDelete
            bool originallyRed = nodeToDelete.IsRed();
            
            // For traversing tree starting at nodeToDelete without reassigning it
            y = nodeToDelete;
            
            // If nodeToDelete only has left children, replace with its left subtree
            if (nodeToDelete.Left() == null) {
                x = nodeToDelete.Right();
                Transplant(nodeToDelete, x);

            // If nodeToDelete only has right children, replace with its right subtree
            } else if (nodeToDelete.Right() == null) {
                x = nodeToDelete.Left();
                Transplant(nodeToDelete, x);
            
            } else {    // nodeToDelete either has two children or is a leaf
                
                // y = smallest node whose value is greater than that of nodeToDelete
                // will replace nodeToDelete
                y = Minimum(nodeToDelete.Right());
                originallyRed = y.IsRed();

                // x = subtree root with values greater than y
                x = y.Right();

                // if y is a child of nodeToDelete, x is already stored in the right place
                if (y.Parent() == nodeToDelete) {
                    x.SetParent(y);
                } else { // else replace y with its right subtree and update attributes
                    Transplant(y, y.Right());
                    y.SetRight(nodeToDelete.Right());
                    y.Right().SetParent(y);
                }

                // replace nodeToDelete with y and update attributes
                Transplant(nodeToDelete, y);
                y.SetLeft(nodeToDelete.Left());
                y.Left().SetParent(y);

                // as y is to replace nodeToDelete's location in tree, update its color
                if (originallyRed) y.SetRed();
                else y.SetBlack();
            }
            // if(!originallyRed) FixDelete(x);
        }

        public void Delete(int key) {
            Delete(Root, key);
        }


        private void RebalancePostInsertion(RBTreeNode newNode) {
            /**
            Insert function places newNode as an appropriate leaf for a regular BST but
            not necessarily a Red-black tree. Check if newNode's parent is RED (ie. if it
            breaks a only red-black tree condition). If so, use the RED and BLACK labels
            of newNode's parent, grandparent, and/or pibling (aunt/uncle) to balance.
   
            Note on conceptualizing this process:
                At this point, newNode is a RED leaf whose addition may cause the tree to
                not meet the required RED-BLACK conditions (noted in class description)
                such that it is imbalanced beyond the red-black tree threshold. In order
                to correct for this, we re-balance and/or recolor the subtree rooted at
                newNode's grandparent. We perform this process iteratively, reassigning
                newNode to its ancestors (parents/grandparents), until we reach the root.
                At no point do we consider decendants of newNode.

                See this page for a more detailed overview of the process:
                    https://www.geeksforgeeks.org/red-black-tree-set-2-insert/  */

            if (newNode.GrandParent() == null) return;

            RBTreeNode u;   // Will represent pibling (aunt/uncle) or parent of newNode.
                            // ie. given newNode's grandparent (gP), u can be either gP's
                            // left or right child
            
            while (newNode.Parent().IsRed()) {

                // Case: p is the RIGHT child of gP:
                if (newNode.Parent() == newNode.GrandParent().Right()) {
                    
                    // Set u as the (LEFT) pibling of newNode
                    u = newNode.GrandParent().Left();

                    // Case: u exists and is RED: set both u & p to BLACK, set gP to RED,
                    // assign newNode = gP
                    if (u != null && u.IsRed()) {
                        u.SetBlack();
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        newNode = newNode.GrandParent();
                    
                    } else {  // Case: the (LEFT) pibling of newNode is BLACK

                        // Case: newNode is the left child of p: assign newNode = p and
                        // right rotate newNode
                        if (newNode == newNode.Parent().Left()) {
                            newNode = newNode.Parent();
                            RotateRight(newNode);
                        }
                        
                        // Set p to BLACK and gP to RED. Then left rotate gP.
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        RotateLeft(newNode.GrandParent());
                    }
                } else { // Case: p is the LEFT child of gP:
                    
                    // Assign u as the (RIGHT) pibling of newNode
                    u = newNode.GrandParent().Right();

                    // Case: u is RED: set the color of both children of gP to BLACK, set
                    // gp to RED. Assign gP = newNode.
                    if (u != null && u.IsRed()) {
                        u.SetBlack();
                        newNode.Parent().SetBlack();
                        newNode.GrandParent().SetRed();
                        newNode = newNode.GrandParent();
                    } else {
                        // Case: newNode is the right child of p then: set p = newNode.
                        // Then left rotate newNode.
                        if (newNode == newNode.Parent().Right()) {
                            newNode = newNode.Parent();
                            RotateLeft(newNode);
                        }

                        // Set color of p as BLACK and color of gP as RED. Then right
                        // rotate gP. 
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
            RBTreeNode y = null;
            RBTreeNode x;

            if (Root == null) { // Empty tree
                Root = new RBTreeNode(data, red:false);
                return;
            } 

            newNode = new RBTreeNode(data);
            x = Root;

            //  Traverse with x until y is a leaf node
            while (x != null) {
                y = x;
                if (newNode < x) x = x.Left();
                else x = x.Right();
            }

            //  Make newNode child of y
            newNode.SetParent(y);
            if (newNode < y) y.SetLeft(newNode);
            else y.SetRight(newNode);

            RebalancePostInsertion(newNode);
        }

        public string TraverseTree(int order) {
            return h.PrintTreeTraversalOrder(order, Root);
        }

        public void PrintTree(int verticalSpacing=1, int indentPerLevel=5) {
            h.VisualizeTree(Root, verticalSpacing, indentPerLevel);
        }

        public int Size() {
            return Root.SubTreeSize();
        }

        public RBTreeNode GetRoot() {
            return Root;
        }

        public override String ToString() {
            return h.VisualizeTree(Root, 0, 5, false);
        }
    }
}
