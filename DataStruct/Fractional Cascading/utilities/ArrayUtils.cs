using System;

namespace Fractional_Cascading {
    public static class ArrayUtils {
        public static T[] ArraySubset<T>(this T[] array, int startIndex, int endIndex) {
            /**
            Copy elements in array between indexes startIndex & endindex inclusive into
            new array   */
            if (startIndex < endIndex)
                throw new Exception("startIndex cannot be greater than endIndex.");
            int subsetSize = endIndex - startIndex + 1;
            T[] subsetArray = new T[subsetSize];
            Array.Copy(array, startIndex, subsetArray, 0, subsetSize);
            return subsetArray;
        }
    }
}
