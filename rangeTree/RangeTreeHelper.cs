using System.Text;

namespace Fractional_Cascading {
    static class RangeTreeHelper {
        public static string Visualize(RangeTreeNode node, int verticalSpacing=2,
                                       int indentPerLevel=10, bool print=true, 
                                       bool safeChars=true) {
            /**
            Parameters:
                node:   RangeTreeNode treated as the root in the visualization
                verticalSpacing: number of lines between each displayed node
                indentPerLevel: width of the line separating each level of the tree
                print: if true, write output to console
                safeChars: if true, use standard dash '-' character for horizontal lines.
                           else, use the special full-width horizontal bar character
                           which seems to occasionally break StringBuilder

            Note: functionality inspired by the following Baeldung article:
                    https://www.baeldung.com/java-print-binary-tree-diagram */       
            
            string pointerChars;
            if (safeChars) pointerChars = new string('-', indentPerLevel);
            else pointerChars = new string('─', indentPerLevel);
            

            string oneChildPointer = "└" + pointerChars;
            string twoChildrenPointer = "├" + pointerChars;

            string paddingChars = new string(' ', indentPerLevel + 1);
            string twoChildrenIndent = "│" + paddingChars;
            string whiteSpaceIndent = " " + paddingChars;

            void Traverse(StringBuilder sb, String padding, String pointer,
                          RangeTreeNode node, bool hasRightSibling) {

                if (node == null || node.IsEmpty()) return;

                // Append vertical line spacing
                for (int i = 0; i < verticalSpacing; i++) {
                    sb.Append("\n" + padding);
                    if (pointer == whiteSpaceIndent) sb.Append('|');
                    else sb.Append("| ");
                }
                
                // Append line rows with values
                sb.Append($"\n{padding}{pointer} {node.VisualizerString()}");

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

            string TraversePreOrder(RangeTreeNode root) {
                
                // Handle root
                if (root == null) return "Empty range tree.";
                StringBuilder sb = new StringBuilder();
                sb.Append(root.VisualizerString());

                // Determine initial pointer
                string pointerLeft =
                    (root.Right() != null) ? twoChildrenPointer : oneChildPointer;
                
                Traverse(sb, "", pointerLeft, root.Left(), root.Right() != null);
                Traverse(sb, "", oneChildPointer, root.Right(), false);
                return sb.ToString();
            }

            string prettyBinaryTree = TraversePreOrder(node);
            if (print) Console.WriteLine(prettyBinaryTree);
            return prettyBinaryTree;
        }
    }
}
