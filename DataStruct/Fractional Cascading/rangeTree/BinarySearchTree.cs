using System;

namespace Fractional_Cascading {
    class BinarySearchTree {

        private RBTreeNode Root = null;

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
                                                          "Binary Search Tree.");

            // recurse left if key less than that of current node
            if (key < curr.GetKey()) curr.SetLeftChild(Insert(curr.GetLeftChild(), key));
            
            // recurse right if key >= current node
            else curr.SetRightChild(Insert(curr.GetRightChild(), key));
            
            return curr;
        }

        public void Traverse(int order=1) {
            /**
            order = 1 -> Inorder
                    2 -> Preorder
                    3 -> Postorder  */
            if (order == 1) {
                Console.Write("\nInorder traversal: ");
                InOrderTraversal(Root);
            } else if (order == 2) {
                Console.Write("\nPreorder traversal: ");
                PreOrderTraversal(Root);
            } else if (order == 3) {
                Console.Write("\nPostorder traversal: ");
                PostOrderTraversal(Root);
            } else throw new Exception($"Invalid order paramter {order} when "+
                                       "calling traverse.");
        } 

        private void InOrderTraversal(RBTreeNode root) {
            if (root == null) return;
            InOrderTraversal(root.GetLeftChild());
            Console.Write($"{root.GetKey()}, ");
            InOrderTraversal(root.GetRightChild());
        }

        private void PreOrderTraversal(RBTreeNode root) {
            if (root == null) return;
            Console.Write($"{root.GetKey()}, ");
            PreOrderTraversal(root.GetLeftChild());
            PreOrderTraversal(root.GetRightChild());
        }

        private void PostOrderTraversal(RBTreeNode root) {
            if (root == null) return;
            PostOrderTraversal(root.GetLeftChild());
            PostOrderTraversal(root.GetRightChild());
            Console.Write($"{root.GetKey()}, ");
        }

        public RBTreeNode GetRoot() {
            return Root;
        }
    }
}