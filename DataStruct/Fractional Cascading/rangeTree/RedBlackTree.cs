using System;
using System.IO;

namespace Fractional_Cascading {
    class RBTree {
        /**
        LLRBTree -> Left Leaning Red Black Tree */

        private RBTreeNode Root = null;

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

        private void RotateRight(RBTreeNode y) {
            // setup x & y
            RBTreeNode x = y.GetLeftChild();
            y.SetLeftChild(x.GetRightChild());
            
            // if x has a right child, make y the parent of the right child of x.
            if (x.GetRightChild() != null) x.GetRightChild().SetParent(y);
            x.SetParent(y.GetParent());
            
            // if the parent of y is NULL, make x the root of the tree.
            if (y.GetParent() == null) Root = x;
            
            // else if y is the right child of its parent p, make x the right child of p.
            else if (y == y.GetParent().GetRightChild()) y.GetParent().SetRightChild(x);
            
            // else assign x as the left child of p.
            else y.GetParent().SetLeftChild(x);

            // make x the parent of y.
            x.SetRightChild(y);
            y.SetParent(x);
        }


        private void RotateLeft(RBTreeNode x) {
            // setup x & y
            RBTreeNode y = x.GetRightChild();
            x.SetRightChild(y.GetLeftChild());

            // if y has a left child, make x the parent of the left child of y.
            if (y.GetLeftChild() != null) y.GetLeftChild().SetParent(x);
            y.SetParent(x.GetParent());

            // if the parent of x is NULL, make y the root of the tree.
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
            /**
            Output to console & return as string.

            Patameter:  order = 1 -> Inorder
                                2 -> Preorder
                                3 -> Postorder  */

            // Helpers:
            void PrintInOrder(RBTreeNode root) {
                if (root == null) return;
                PrintInOrder(root.GetLeftChild());
                Console.Write($"{root.GetKey()}, ");
                PrintInOrder(root.GetRightChild());
            }

            void PrintPreOrder(RBTreeNode root) {
                if (root == null) return;
                Console.Write($"{root.GetKey()}, ");
                PrintPreOrder(root.GetLeftChild());
                PrintPreOrder(root.GetRightChild());
            }

            void PrintPostOrder(RBTreeNode root) {
                if (root == null) return;
                PrintPostOrder(root.GetLeftChild());
                PrintPostOrder(root.GetRightChild());
                Console.Write($"{root.GetKey()}, ");
            }

            // Write to string using console:
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            if(order == 1) {
                Console.Write("\nInorder traversal: ");
                PrintInOrder(Root);
            } else if (order == 2) {
                Console.Write("\nPreorder traversal: ");
                PrintPreOrder(Root);
            } else if (order == 3) {
                Console.Write("\nPostorder traversal: ");
                PrintPostOrder(Root);
            } else throw new Exception($"Invalid order paramter {order} when "+
                                       "calling traverse.");
            String ret = sw.ToString();
            ret = ret.Remove(ret.Length - 2, 2);   // remove trailing comma & space

            // close StringWriter, reset standard out, return
            sw.Close();
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            return ret;
        }

        public RBTreeNode GetRoot() {
            return Root;
        }
    }
}