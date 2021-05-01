using System;

namespace Fractional_Cascading {
    class RangeTreeDemo {
        RangeTreeHelper rth = new RangeTreeHelper();
        Utils u = new Utils();
        public RangeTreeDemo(int n, int dim, int locMin, int locMax,
                             int[] rangeMins, int[] rangeMaxes) {
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
            
            CoordNode[] nodes = 
                NodeGenerator.GetCoordNodeList(n, sort:false, dimensions:dim,
                                               dataRangeMin:locMin,
                                               dataRangeMax:locMax,
                                               locRangeMin:locMin,
                                               locRangeMax:locMax);
            RangeTree rt = new RangeTree(nodes);
            for (int i = 1; i <= dim; i++) {
                rth.VisualizeTree(rt.GetRootByDimension(i), 2, 10);
                Console.WriteLine(u.Separator(100));
            }
        }
    }
}