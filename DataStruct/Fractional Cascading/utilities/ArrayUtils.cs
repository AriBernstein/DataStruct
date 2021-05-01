using System;

namespace Fractional_Cascading {
    public static class ArrayUtils {
        public static T[] ArraySubset<T>(this T[] array, int startIndex, int endIndex) {
            /**
            Copy elements in array between indexes startIndex & endindex inclusive into
            new array   */
            if (startIndex > endIndex)
                throw new Exception($"startIndex ({startIndex}) cannot be greater than " +
                                    $"endIndex ({endIndex}).");
            int subsetSize = endIndex - startIndex + 1;
            T[] subsetArray = new T[subsetSize];
            Array.Copy(array, startIndex, subsetArray, 0, subsetSize);
            return subsetArray;
        }

        public static string PrintArray<T>(this T[] array, bool print=true,
                                           String sep=", ") {
            /**
            Return visualization of array with each element separated by sep
            Note that the type objects array contains must have a ToString method.  */
            
            string s = "";
            int n = array.Length;
            for (int i = 0; i < (n-1); i++)
                s = s + (array[i] + sep);
            s = s + (array[n-1]);
            if(print) Console.WriteLine(s);
            return s;
        }
    }
}
