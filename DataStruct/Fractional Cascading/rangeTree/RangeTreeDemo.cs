using System;

namespace Fractional_Cascading {
    class RangeTreeDemo {
        RangeTreeHelper rth = new RangeTreeHelper();
        Utils u = new Utils();
        public RangeTreeDemo(int n, int dim, int locMin, int locMax,
                             int[] rangeMins, int[] rangeMaxes, int randomSeed=-1) {
            // Check arguments - if any are wrong, this can explode and I'm a pacifist
            if (n < 1) throw new Exception("n parameter must be greater than 0.");
            if (locMin > locMax)
                throw new Exception($"locMin value ({locMin}) must be less than or " +
                                    $"equal to locMax value ({locMax})");
            if (locMax - locMin < n)
                throw new Exception($"Space between locMin ({locMin}) and " +
                                    $"locMax ({locMax} must be greater than or " +
                                    $"equal to n ({n}).)");
            if (rangeMins.Length != dim || rangeMaxes.Length != dim)
                throw new Exception($"Sizes of arrays lowRanges ({rangeMins.Length}) " +
                                    $"and highRanges must equal dimensionality ({dim}).");
            for (int i = 0; i < dim; i++)
                if (rangeMins[i] >= rangeMaxes[i])
                    throw new Exception($"Minimum ({rangeMins[i]}) in orthogonal range " +
                                        "search must be less than or equal to maximum " +
                                        $"({rangeMaxes[i]}) in dimension {dim}");
            
            // Build range tree
            CoordNode[] nodes = 
                NodeGenerator.GetCoordNodeList(n, sort:false, dimensions:dim,
                                               dataRangeMin:locMin,
                                               dataRangeMax:locMax,
                                               locRangeMin:locMin,
                                               locRangeMax:locMax,
                                               randomSeed:-1);
            RangeTree rt = new RangeTree(nodes);
            
            // Print range tree in each dimension
            if (n <= 15) {
                for (int i = 1; i <= dim; i++) {
                    Console.WriteLine($"\n\nDimension {i} {u.Separator(85, 0)}");
                    rth.VisualizeTree(rt.GetRootByDimension(i), 2, 10);
                }
            }

            // Show orthogonal range search
            
            Console.WriteLine($"CoordNodes located in range\n\tx: ({rangeMins[0]}, {rangeMaxes[0]}), \ty: ({rangeMins[1]}, {rangeMaxes[1]}), \tz: ({rangeMins[2]}, {rangeMaxes[2]})");
            ArrayUtils.PrintArray(rt.OrthogonalRangeSearch(rangeMins, rangeMaxes), sep: "\n");
        }
    }
}