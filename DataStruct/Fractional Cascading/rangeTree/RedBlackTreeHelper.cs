using System;
using System.IO;

namespace Fractional_Cascading {
    class RBTreeHelper {

        public string PrintTreeTraversal(int order, RBTreeNode treeRoot) {
            /**
            Output to console & return as string.

            Patameters:
                order:  1 -> Inorder
                        2 -> Preorder
                        3 -> Postorder
                treeRoot: the root node of the Red-Black tree to traverse   */

            // Helpers:
            void PrintInOrder(RBTreeNode currRoot) {
                if (currRoot == null) return;
                PrintInOrder(currRoot.GetLeftChild());
                Console.Write($"{currRoot.GetKey()}, ");
                PrintInOrder(currRoot.GetRightChild());
            }

            void PrintPreOrder(RBTreeNode currRoot) {
                if (currRoot == null) return;
                Console.Write($"{currRoot.GetKey()}, ");
                PrintPreOrder(currRoot.GetLeftChild());
                PrintPreOrder(currRoot.GetRightChild());
            }

            void PrintPostOrder(RBTreeNode currRoot) {
                if (currRoot == null) return;
                PrintPostOrder(currRoot.GetLeftChild());
                PrintPostOrder(currRoot.GetRightChild());
                Console.Write($"{currRoot.GetKey()}, ");
            }

            // Write to string using console:
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            if (order == 1) {
                Console.Write("Inorder traversal: ");
                PrintInOrder(treeRoot);
            } else if (order == 2) {
                Console.Write("Preorder traversal: ");
                PrintPreOrder(treeRoot);
            } else if (order == 3) {
                Console.Write("Postorder traversal: ");
                PrintPostOrder(treeRoot);
            } else throw new Exception($"Invalid order paramter {order} when "+
                                       "calling traverse.");
            
            // store string & remove trailing comma & space
            String output = sw.ToString();
            output = output.Remove(output.Length - 2, 2);

            // close StringWriter, reset standard out, print, and return
            sw.Close();
            var standardOutput = new StreamWriter(Console.OpenStandardOutput());
            standardOutput.AutoFlush = true;
            Console.SetOut(standardOutput);
            Console.WriteLine(output);
            return output;
        }
    }
}