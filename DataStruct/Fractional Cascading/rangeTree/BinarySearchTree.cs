using System;

namespace Fractional_Cascading {
    class BinarySearchTree {

        int n;  // size of tree

        private BSTNode Root = null;

        public void Insert(int key) {
            Root = Insert(Root, key);
        }

        Utils u = new Utils();
        
        private BSTNode Insert(BSTNode root, int key) {
            if (root == null)   // Base case
                return new BSTNode(key, root:true);
            else if (key < root.GetKey())   // Recurse left
                root.SetLeftChild(Insert(root.GetLeftChild(), key));
            else if (key >= root.GetKey())  // Recuse right
                root.SetRightChild(Insert(root.GetRightChild(), key));

            n++;
            return root;
        }

        public void Traverse(int order=1) {
            /**
            order = 1 -> Inorder
                    2 -> Preorder
                    3 -> Postorder  */
            if(order == 1) TraverseInOrder(Root);
            else if (order == 2) TraversePreOrder(Root);
            else if (order == 3) TraversePostOrder(Root);
            else throw new Exception($"Invalid order paramter {order} when "+
                                      "calling traverse.");
        } 

        private void TraverseInOrder(BSTNode root) {
            if (!(root == null)) {
                TraverseInOrder(root.GetLeftChild());
                Console.Write($"{root.GetKey()}, ");
                TraverseInOrder(root.GetRightChild());
            }
        }

        private void TraversePreOrder(BSTNode root) {
            if (!(root == null)) {
                Console.Write($"{root.GetKey()}, ");
                TraversePreOrder(root.GetLeftChild());
                TraversePreOrder(root.GetRightChild());
            }
        }

        private void TraversePostOrder(BSTNode root) {
            if (!(root == null)) {
                TraversePostOrder(root.GetLeftChild());
                TraversePostOrder(root.GetRightChild());
                Console.Write($"{root.GetKey()}, ");
            }
        }

        public BSTNode GetRoot() {
            return Root;
        }
    }
}