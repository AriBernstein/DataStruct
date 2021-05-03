using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    class RangeTreeDemo {
        Utils u = new Utils();
        public RangeTreeDemo(int n, int dim, int locMin, int locMax,
                             int[] rangeMins, int[] rangeMaxes,
                             int insertXValue=-1, int insertYValue=-1,
                             int insertZValue=-1, int randomSeed=-1) {
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
            
            List<CoordNode> nodes = new List<CoordNode>();
            nodes.Add(new CoordNode(1, 68));
            nodes.Add(new CoordNode(2, 72));
            nodes.Add(new CoordNode(3, 76));
            nodes.Add(new CoordNode(4, 83));
            // CoordNode[] nodes = 
            //     NodeGenerator.GetCoordNodeList(n, sort:false, dimensions:dim,
            //                                    dataRangeMin:locMin, dataRangeMax:locMax,
            //                                    locRangeMin:locMin, locRangeMax:locMax,
            //                                    randomSeed:-1);
            RangeTree rt = new RangeTree(nodes.ToArray());
            
            // Print range tree in each dimension
            if (n <= 30) {
                for (int i = 1; i <= dim; i++) {
                    Console.WriteLine($"\n\nDimension {i} {u.Separator(85, 0)}");
                    RangeTreeHelper.VisualizeTree(rt.GetRootByDimension(i), 2, 10);
                }
            }

            // Show orthogonal range search
            if (dim == 1) 
                Console.WriteLine($"CoordNodes located in range: x: ({rangeMins[0]}, {rangeMaxes[0]})");
            if (dim == 2)
                Console.WriteLine($"CoordNodes located in range:\n" +
                                  $"\tx: ({rangeMins[0]}, {rangeMaxes[0]}), " +
                                  $"\ty: ({rangeMins[1]}, {rangeMaxes[1]})");
            if (dim == 3)
                Console.WriteLine($"CoordNodes located in range:\n" +
                                  $"\tx: ({rangeMins[0]}, {rangeMaxes[0]}), " +
                                  $"\ty: ({rangeMins[1]}, {rangeMaxes[1]}), " +
                                  $"\tz: ({rangeMins[2]}, {rangeMaxes[2]})");
            
            // int searchFor = rt.GetRoot().GetCoordNodeList()[3].GetAttr(1) + 1;
            // Console.WriteLine($"Searching for {searchFor}");
            // Console.WriteLine(rt.FindNode(rt.GetRoot(), searchFor));
            ArrayUtils.PrintArray(rt.OrthogonalRangeSearch(rangeMins, rangeMaxes), sep: "\n");
        }
    }
}
