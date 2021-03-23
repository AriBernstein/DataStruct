// C# program for Merge Sorting lists of CoordinateNode objects by a given dimension
using System;

namespace Fractional_Cascading {
    public class MergeSortNodes {
    
        // Merges two subarrays of []arr.
        // First subarray is arr[l..m]
        // Second subarray is arr[m+1..r]
        private void merge(CoordinateNode[] initialArray, int lIndex, int mIndex, int rIndex, int sortAttr) {
            // Find sizes of two subarrays to be merged
            int n1 = mIndex - lIndex + 1;
            int n2 = rIndex - mIndex;
    
            // Create temp arrays
            CoordinateNode[] leftSideArr = new CoordinateNode[n1];
            CoordinateNode[] rightSideArr = new CoordinateNode[n2];
            int i, j;
    
            // Copy data to temp arrays
            for (i = 0; i < n1; ++i)
                leftSideArr[i] = initialArray[lIndex + i];
            for (j = 0; j < n2; ++j)
                rightSideArr[j] = initialArray[mIndex + 1 + j];
    
    
            // Merge the temp arrays
            // Initial indexes of first and second subarrays
            i = j = 0;
    
            // Initial index of merged subarry array
            int k = lIndex;
            while (i < n1 && j < n2) {
                if (leftSideArr[i].getAttr(sortAttr) <= rightSideArr[j].getAttr(sortAttr)) {
                    initialArray[k] = leftSideArr[i];
                    i++;
                } else {
                    initialArray[k] = rightSideArr[j];
                    j++;
                } k++;
            }    
            // Copy remaining elements of L[] if any
            while (i < n1) {
                initialArray[k] = leftSideArr[i];
                i++;
                k++;
            }   
            // Copy remaining elements of R[] if any
            while (j < n2) {
                initialArray[k] = rightSideArr[j];
                j++;
                k++;
            }
        }
    
        // Main function that sorts arr[l..r] using merge()
        private void startSort(CoordinateNode[] unsortedArr, int lIndex, int rIndex, int sortDim) {
            if (lIndex < rIndex) {
                // Find the middle point
                int mIndex = lIndex+ (rIndex-lIndex)/2;
    
                // Sort first and second halves
                startSort(unsortedArr, lIndex, mIndex, sortDim);
                startSort(unsortedArr, mIndex + 1, rIndex, sortDim);
    
                // Merge the sorted halves
                merge(unsortedArr, lIndex, mIndex, rIndex, sortDim);
            }
        }

        public void sort(CoordinateNode[] unsortedArr, int sortDimension) {
            int lIndex = 0;
            int rIndex = unsortedArr.Length - 1;
            startSort(unsortedArr, lIndex, rIndex, sortDimension); 
        }
    }
}
