// C# program for Merge Sorting lists of CoordinateNode objects by a given dimension
using System;

namespace Fractional_Cascading {
    public class MergeSortNodes {
        
        private void merge(CoordNode[] initialArray, int l, int m, int r, int sortAttrCode) {
            // Find sizes of two subarrays to be merged
            int n1 = m - l + 1;
            int n2 = r - m;
    
            // Create temp arrays
            CoordNode[] leftSideArr = new CoordNode[n1];
            CoordNode[] rightSideArr = new CoordNode[n2];
            int i, j, k;
    
            // Copy data to temp arrays
            for (i = 0; i < n1; ++i) leftSideArr[i] = initialArray[l + i];
            for (j = 0; j < n2; ++j) rightSideArr[j] = initialArray[m + 1 + j];
    
            // Merge the temp arrays
            i = j = 0;  // Initial indexes of first and second subarrays
            k = l;      // Initial index of merged subarry

            while (i < n1 && j < n2) {
                if (leftSideArr[i].getAttr(sortAttrCode) <= rightSideArr[j].getAttr(sortAttrCode)) {
                    initialArray[k++] = leftSideArr[i++];
                } else initialArray[k++] = rightSideArr[j++];
            }    
            // Copy remaining elements of L[] or R[] if any
            while (i < n1) initialArray[k++] = leftSideArr[i++];
            while (j < n2) initialArray[k++] = rightSideArr[j++];
        }
    
        // Main function that sorts arr[l..r] using merge()
        private void mergeSort(CoordNode[] unsortedArr, int lIndex, int rIndex, int sortAttr) {
            if (lIndex < rIndex) {  // base case
                // Find the middle point
                int mIndex = lIndex + (rIndex-lIndex)/2;
    
                // Sort first and second halves
                mergeSort(unsortedArr, lIndex, mIndex, sortAttr);
                mergeSort(unsortedArr, mIndex + 1, rIndex, sortAttr);
    
                // Merge the sorted halves
                merge(unsortedArr, lIndex, mIndex, rIndex, sortAttr);
            }
        }

        public void sort(CoordNode[] unsortedArr, int sortAttributeCode) {
            int lIndex = 0;
            int rIndex = unsortedArr.Length - 1;
            mergeSort(unsortedArr, lIndex, rIndex, sortAttributeCode); 
        }
    }
}
