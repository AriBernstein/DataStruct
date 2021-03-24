// C# program for Merge Sorting lists of CoordinateNode objects by a given dimension
using System;

namespace Fractional_Cascading {
    public class MergeSortNodes {
    
        // Merges two subarrays of []arr.
        // First subarray is arr[l..m]
        // Second subarray is arr[m+1..r]
        private void merge(CoordNode[] initialArray, int lIndex,
                           int mIndex, int rIndex, int sortAttrCode) {
            // Find sizes of two subarrays to be merged
            int n1 = mIndex - lIndex + 1;
            int n2 = rIndex - mIndex;
    
            // Create temp arrays
            CoordNode[] leftSideArr = new CoordNode[n1];
            CoordNode[] rightSideArr = new CoordNode[n2];
            int i, j;
    
            // Copy data to temp arrays
            for (i = 0; i < n1; ++i) leftSideArr[i] = initialArray[lIndex + i];
            for (j = 0; j < n2; ++j) rightSideArr[j] = initialArray[mIndex + 1 + j];
    
    
            // Merge the temp arrays
            // Initial indexes of first and second subarrays
            i = j = 0;
    
            // Initial index of merged subarry array
            int k = lIndex;
            while (i < n1 && j < n2) {
                if (leftSideArr[i].getAttr(sortAttrCode) <= rightSideArr[j].getAttr(sortAttrCode)) {
                    initialArray[k] = leftSideArr[i];
                    i++;
                } else {
                    initialArray[k] = rightSideArr[j];
                    j++;
                } k++;
            }    
            // Copy remaining elements of L[] if any
            while (i < n1) initialArray[k++] = leftSideArr[i++];

            // Copy remaining elements of R[] if any
            while (j < n2) initialArray[k++] = rightSideArr[j++];
        }
    
        // Main function that sorts arr[l..r] using merge()
        private void startSort(CoordNode[] unsortedArr, int lIndex, int rIndex, int sortAttr) {
            if (lIndex < rIndex) {
                // Find the middle point
                int mIndex = lIndex + (rIndex-lIndex)/2;
    
                // Sort first and second halves
                startSort(unsortedArr, lIndex, mIndex, sortAttr);
                startSort(unsortedArr, mIndex + 1, rIndex, sortAttr);
    
                // Merge the sorted halves
                merge(unsortedArr, lIndex, mIndex, rIndex, sortAttr);
            }
        }

        public void sort(CoordNode[] unsortedArr, int sortAttributeCode) {
            int lIndex = 0;
            int rIndex = unsortedArr.Length - 1;
            startSort(unsortedArr, lIndex, rIndex, sortAttributeCode); 
        }
    }
}
