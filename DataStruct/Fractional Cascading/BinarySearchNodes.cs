using System;

namespace Fractional_Cascading {
    // C# program for using Binary Search to find location of a
    // fractional cascading node in a list by a given attribute
    public class BinarySearchNodes {

        private int binarySearchFCNode(FractionalCascadingNode[] nodeArray,
                                       int l, int r, int data, int attrCode) {
            // Base case
            String xNotInArrException =
                "data: " + data + " cannot be found in FC Node array during binary search";                                                  
            
            if(r < 1) throw new Exception(xNotInArrException); // Base case
            
            int m = l + (r - l) / 2;
            int dataAtMid = nodeArray[m].getAttr(attrCode);

            // Data located at midpoint
            if(data == dataAtMid) return m;

            // Data less than element at mid, search left side
            else if(data < dataAtMid) return binarySearchFCNode(nodeArray, l, m-1, data, attrCode);

            // Data greater than element at mid, search right side
            else return binarySearchFCNode(nodeArray, m+1, r, data, attrCode);
        }

        public int binarySearchFCNode(FractionalCascadingNode[] nodeArray,
                                      int searchValue, int attrCode) {
            return binarySearchFCNode(nodeArray, 0, nodeArray.Length-1, searchValue, attrCode);
        }
    }
}
