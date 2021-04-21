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
                root.setLeftChild(Insert(root.getLeftChild(), key));
            else if (key >= root.GetKey())  // Recuse right
                root.setRightChild(Insert(root.getRightChild(), key));

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
            else throw new Exception("Invalid order paramter when calling traverse.");
        } 

        private void TraverseInOrder(BSTNode root) {
            int i = 0;
            int[] inOrderLst = new int[n];
            if (!(root == null)) {
                TraverseInOrder(root.getLeftChild());
                inOrderLst[i++] = root.GetKey();
                TraverseInOrder(root.getRightChild());
            }
            u.PrintIntArray(inOrderLst);
        }

        private void TraversePreOrder(BSTNode root) {
            int i = 0;
            int[] preOrderLst = new int[n];
            if (!(root == null)) {
                preOrderLst[i++] = root.GetKey();
                TraversePreOrder(root.getLeftChild());
                TraversePreOrder(root.getRightChild());
            }
            u.PrintIntArray(preOrderLst);
        }

        private void TraversePostOrder(BSTNode root) {
            int i = 0;
            int [] postOrderLst = new int[n];
            if (!(root == null)) {
                TraversePostOrder(root.getLeftChild());
                TraversePostOrder(root.getRightChild());
                postOrderLst[i++] = root.GetKey();
            }
            u.PrintIntArray(postOrderLst);
        }

        public BSTNode GetRoot() {
            return Root;
        }
    }
}