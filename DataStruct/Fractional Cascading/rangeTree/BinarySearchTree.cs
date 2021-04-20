using System;

namespace Fractional_Cascading {
    class BinarySearchRangeTree {

        BSTNode Root = null;

        public void insert(int key) {
            Root = insert(Root, key);
        }
        
        private BSTNode insert(BSTNode root, int key) {
            if (root == null)   // Base case
                return new BSTNode(key, root:true);
            else if (key < root.getKey())   // Recurse left
                root.setLeftChild(insert(root.getLeftChild(), key));
            else if (key >= root.getKey())  // Recuse right
                root.setRightChild(insert(root.getRightChild(), key));
            return root;
        }

        public void traverseInOrder() {
            traverseInOrder(Root);
        } 

        private void traverseInOrder(BSTNode root) {
            if (!(root == null)) {
                traverseInOrder(root.getLeftChild());
                Console.WriteLine(root.getKey() + ", ");
                traverseInOrder(root.getRightChild());
            }
        }
    }
}