using System;
using System.IO;
using System.Text;

namespace Fractional_Cascading {
    class RBTreeHelper {

        public string PrintTreeTraversalOrder(int order, RBTreeNode treeRoot) {
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

        public string PrintSubTree(RBTreeNode node, int verticalSpacing,
                                   int indentPerLevel) {
            /**
            Note, functionality inspired by the following Baeldung article:
                    https://www.baeldung.com/java-print-binary-tree-diagram */

            // Note: the full width horizontal bar character (distinct from the much more
            //       common dash char) seemed to occasionally break the string builder.       
            string pointerChars = new string('─', indentPerLevel);
            // string pointerChars = new string('-', indentPerLevel);

            string oneChildPointer = "└" + pointerChars;
            string twoChildrenPointer = "├" + pointerChars;

            string paddingChars = new string(' ', indentPerLevel + 1);
            string twoChildrenIndent = "│" + paddingChars;
            string whiteSpaceIndent = " " + paddingChars;

            void Traverse(StringBuilder sb, String padding, String pointer,
                          RBTreeNode node, bool hasRightSibling) {

                if (node == null) return;

                // Append vertical line spacing
                for(int i = 0; i < verticalSpacing; i++) {
                    sb.Append("\n" + padding);
                    if(pointer == whiteSpaceIndent) sb.Append('|');
                    else sb.Append("| ");
                }
                
                // Append line rows with values
                sb.Append("\n" + padding + pointer + " " + node.GetData());
                if(node.IsRed()) sb.Append(" (R)");
                else sb.Append(" (B)");

                // Calculate and append padding next row
                StringBuilder paddingSB = new StringBuilder(padding);
                if (hasRightSibling) paddingSB.Append(twoChildrenIndent);
                else paddingSB.Append(whiteSpaceIndent);
                string newPadding = paddingSB.ToString();
                
                // Determine pointer for next row
                string pointerLeft = 
                    (node.Right() != null) ? twoChildrenPointer : oneChildPointer;
                
                // Recurse
                Traverse(sb, newPadding, pointerLeft, node.Left(), node.Right() != null);
                Traverse(sb, newPadding, oneChildPointer, node.Right(), false);
            }

            string TraversePreOrder(RBTreeNode root) {
                
                // Handle root
                if (root == null) return "Empty binary tree.";
                StringBuilder sb = new StringBuilder();
                sb.Append(root.GetData() + " (B)");

                // Determine initial pointer
                string pointerLeft =
                    (root.Right() != null) ? twoChildrenPointer : oneChildPointer;
                
                Traverse(sb, "", pointerLeft, root.Left(), root.Right() != null);
                Traverse(sb, "", oneChildPointer, root.Right(), false);
                return sb.ToString();
            }

            string prettyBinaryTree = TraversePreOrder(node);
            Console.WriteLine(prettyBinaryTree);
            return prettyBinaryTree;
        }
    }
}