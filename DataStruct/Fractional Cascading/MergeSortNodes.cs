// C# program for Merge Sorting lists of CoordinateNode objects by a given dimension
using System;

namespace Fractional_Cascading {
    public class MergeSortNodes {
        
        private void merge(CoordNode[] initialArray, int l, int m, int r, int attrCode) {
            // Find sizes of two subarrays to be merged
            int n1 = m - l + 1;
            int n2 = r - m;
    
            // Create temp arrays
            CoordNode[] leftArr = new CoordNode[n1];
            CoordNode[] rightArr = new CoordNode[n2];
            int i, j, k;
    
            // Copy data to temp arrays
            for (i = 0; i < n1; ++i) leftArr[i] = initialArray[l + i];
            for (j = 0; j < n2; ++j) rightArr[j] = initialArray[m + 1 + j];
    
            // Merge the temp arrays
            i = j = 0;  // Initial indexes of first and second subarrays
            k = l;      // Initial index of merged subarry

            while (i < n1 && j < n2) {
                if (leftArr[i].getAttr(attrCode) <= rightArr[j].getAttr(attrCode)) {
                    initialArray[k++] = leftArr[i++];
                } else initialArray[k++] = rightArr[j++];
            } 
               
            // Copy remaining elements of L[] or R[] if any
            while (i < n1) initialArray[k++] = leftArr[i++];
            while (j < n2) initialArray[k++] = rightArr[j++];
        }
    
        // Main function that sorts arr[l..r] using merge()
        private void mergeSort(CoordNode[] unsortedArr, int lIndex,
                               int rIndex, int sortAttrCode) {
            if (lIndex < rIndex) {  // base case
            
                int mIndex = lIndex + (rIndex-lIndex) / 2;  // Find the mid point
    
                // Sort first and second halves
                mergeSort(unsortedArr, lIndex, mIndex, sortAttrCode);
                mergeSort(unsortedArr, mIndex + 1, rIndex, sortAttrCode);
    
                // Merge the sorted halves
                merge(unsortedArr, lIndex, mIndex, rIndex, sortAttrCode);
            }
        }

        public void sort(CoordNode[] unsortedArr, int sortAttributeCode) {
            if(unsortedArr.Length == 0) throw new Exception("unsortedArr is empty");
            int lIndex = 0;
            int rIndex = unsortedArr.Length - 1;
            mergeSort(unsortedArr, lIndex, rIndex, sortAttributeCode); 
        }
    }
}
