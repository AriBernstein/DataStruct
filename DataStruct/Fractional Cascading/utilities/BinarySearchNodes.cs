using System;

namespace Fractional_Cascading {
    // C# program for using Binary Search to find location of a node in a list by a
    // given attribute
    public class BinarySearchNodes {
        public int BinarySearch(Node[] nodeArray, int searchValue, int attrCode) {
            /** Parameters:
                    nodeArray:      array of nodes ordered by the attribute associated
                                    with attrCode
                                                                    
                    searchValue:    the thing value for which we are searching

                    attrCode:       node[i].getAttr(attrCode) returns the attrubute by
                                    which nodeArray is ordered

                Return: index of searchValue in nodeArray, or -1 if node not found
            */
            if (nodeArray.Length == 0)
                throw new Exception("Attempting binarySearch on empty array");
            return BinarySearch(nodeArray, 0, nodeArray.Length-1, searchValue, attrCode);
        }

        private int BinarySearch(Node[] nodeArray, int l, int r, int x, int attrCode) {
            if (l >= r) {
                if (nodeArray[r].GetAttr(attrCode) == x) return r;
                else return -1;            
            }
            
            int m = l + (r - l) / 2;
            int valueAtMid = nodeArray[m].GetAttr(attrCode);

            // Value located at midpoint of nodeArray
            if (x == valueAtMid) return m;

            // Value is less than element at mid, search left side
            else if (x < valueAtMid) return BinarySearch(nodeArray, l, m, x, attrCode);

            // Value is greater than element at mid, search right side
            else return BinarySearch(nodeArray, m+1, r, x, attrCode);
        }
    }
}
