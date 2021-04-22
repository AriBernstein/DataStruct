using System;
using System.IO;
using System.Text;

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
                PrintInOrder(currRoot.Left());
                Console.Write($"{currRoot.GetData()}, ");
                PrintInOrder(currRoot.Right());
            }

            void PrintPreOrder(RBTreeNode currRoot) {
                if (currRoot == null) return;
                Console.Write($"{currRoot.GetData()}, ");
                PrintPreOrder(currRoot.Left());
                PrintPreOrder(currRoot.Right());
            }

            void PrintPostOrder(RBTreeNode currRoot) {
                if (currRoot == null) return;
                PrintPostOrder(currRoot.Left());
                PrintPostOrder(currRoot.Right());
                Console.Write($"{currRoot.GetData()}, ");
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

        public String PrintSubTree(RBTreeNode node) {
            /**
            Note, functionality largely taken from this article:
                    https://www.baeldung.com/java-print-binary-tree-diagram */

            void Traverse(StringBuilder sb, String padding, String pointer,
                          RBTreeNode node, bool hasRightSibling) {
                if (node != null) {
                    sb.Append("\n");
                    sb.Append(padding);
                    sb.Append(pointer);
                    sb.Append(" " + node.GetData());
                    if(node.IsRed()) sb.Append(" (R)");
                    else sb.Append(" (B)");

                    StringBuilder paddingBuilder = new StringBuilder(padding);
                    
                    if (hasRightSibling) paddingBuilder.Append("│   ");
                    else paddingBuilder.Append("    ");

                    String paddingForBoth = paddingBuilder.ToString();
                    String pointerRight = "└────";
                    String pointerLeft = (node.Right() != null) ? "├────" : "└────";

                    Traverse(sb, paddingForBoth, pointerLeft, node.Left(), node.Right() != null);
                    Traverse(sb, paddingForBoth, pointerRight, node.Right(), false);
                }
            }

            String TraversePreOrder(RBTreeNode root) {
                if (root == null) return "";

                StringBuilder sb = new StringBuilder();
                sb.Append(root.GetData());

                String pointerRight = "└──";
                String pointerLeft = (root.Right() != null) ? "├──" : "└──";

                Traverse(sb, "", pointerLeft, root.Left(), root.Right() != null);
                Traverse(sb, "", pointerRight, root.Right(), false);
                return sb.ToString();
            }

            String prettyBinaryTree = TraversePreOrder(node);
            Console.WriteLine(TraversePreOrder(node));
            return prettyBinaryTree;

        }
    }
}