// C# program for Merge Sort
using System;

namespace DataStruct
{
    public class MergeSortInts {
    
        // Merges two subarrays of []arr.
        // First subarray is arr[l..m]
        // Second subarray is arr[m+1..r]
        private void merge(int[] initial_array, int l_index, int m_index, int r_index) {
            // Find sizes of two subarrays to be merged
            int n1 = m_index - l_index + 1;
            int n2 = r_index - m_index;
    
            // Create temp arrays
            int[] left_side_array = new int[n1];
            int[] right_side_array = new int[n2];
            int i, j;
    
            // Copy data to temp arrays
            for (i = 0; i < n1; ++i)
                left_side_array[i] = initial_array[l_index + i];
            for (j = 0; j < n2; ++j)
                right_side_array[j] = initial_array[m_index + 1 + j];
    
            // Merge the temp arrays
    
            // Initial indexes of first and second subarrays
            i = 0;
            j = 0;
    
            // Initial index of merged subarry array
            int k = l_index;
            while (i < n1 && j < n2) {
                if (left_side_array[i] <= right_side_array[j]) {
                    initial_array[k] = left_side_array[i];
                    i++;
                }
                else {
                    initial_array[k] = right_side_array[j];
                    j++;
                }
                k++;
            }
    
            // Copy remaining elements of L[] if any
            while (i < n1) {
                initial_array[k] = left_side_array[i];
                i++;
                k++;
            }
    
            // Copy remaining elements of R[] if any
            while (j < n2) {
                initial_array[k] = right_side_array[j];
                j++;
                k++;
            }
        }
    
        // Main function that sorts arr[l..r] using merge()
        private void startSort(int[] unsorted_array, int l_index, int r_index) {
            if (l_index < r_index) {
                // Find the middle point
                int m = l_index+ (r_index-l_index)/2;
    
                // Sort first and second halves
                startSort(unsorted_array, l_index, m);
                startSort(unsorted_array, m + 1, r_index);
    
                // Merge the sorted halves
                merge(unsorted_array, l_index, m, r_index);
            }
        }

        public void sort(int[] unsorted_array) {
            int l_index = 0;
            int r_index = unsorted_array.Length - 1;
            startSort(unsorted_array, l_index, r_index); 
        }
    
        // A utility function tocprint array of size n */
        public void printArray(int[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n; ++i)
                Console.Write(arr[i] + " ");
            Console.WriteLine();
        }
    }
}
