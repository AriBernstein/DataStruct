// C# program for Merge Sorting lists of CoordinateNode objects by a given dimension
using System;

namespace Fractional_Cascading {
    public class MergeSortNodes {
        
        private void Merge(Node[] initialArray, int l, int m, int r, int attrCode) {
            // Find sizes of two subarrays to be merged
            int n1 = m - l + 1;
            int n2 = r - m;
    
            // Create temp arrays
            Node[] leftArr = new Node[n1];
            Node[] rightArr = new Node[n2];
            int i, j, k;
    
            // Copy data to temp arrays
            for (i = 0; i < n1; ++i) leftArr[i] = initialArray[l + i];
            for (j = 0; j < n2; ++j) rightArr[j] = initialArray[m + 1 + j];
    
            // Merge the temp arrays
            i = j = 0;  // Initial indexes of first and second subarrays
            k = l;      // Initial index of merged subarray

            while (i < n1 && j < n2) {
                if (leftArr[i].GetAttr(attrCode) <= rightArr[j].GetAttr(attrCode)) {
                    initialArray[k++] = leftArr[i++];
                } else initialArray[k++] = rightArr[j++];
            } 
               
            // Copy remaining elements of L[] or R[] if any
            while (i < n1) initialArray[k++] = leftArr[i++];
            while (j < n2) initialArray[k++] = rightArr[j++];
        }

        private void MergeSort(Node[] unsortedArr, int lIndex,
                               int rIndex, int sortAttrCode) {
            if (lIndex < rIndex) {  // base case
            
                int mIndex = lIndex + (rIndex-lIndex) / 2;  // Find the mid point
    
                // Sort first and second halves
                MergeSort(unsortedArr, lIndex, mIndex, sortAttrCode);
                MergeSort(unsortedArr, mIndex + 1, rIndex, sortAttrCode);
    
                // Merge the sorted halves
                Merge(unsortedArr, lIndex, mIndex, rIndex, sortAttrCode);
            }
        }

        public void Sort(Node[] unsortedArr, int sortAttributeCode) {
            /**
            Recursively sort unsortedArr on node.GetAttr(sortAttributeCode). Throw error
            if unsortedArr is empty.    */

            // Check if we should bother doing any work
            if (unsortedArr.Length <= 1) return;
            
            // Oh well...
            int lIndex = 0;
            int rIndex = unsortedArr.Length - 1;
            MergeSort(unsortedArr, lIndex, rIndex, sortAttributeCode);             
        }
    }
}
