using System;
using System.IO;

namespace Fractional_Cascading {
    class RBTreeHelper {

        public string TraverseTree(int order, RBTreeNode treeRoot) {
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