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
                                    $"equal to locMax value ({locMax}).");
            if (locMax - locMin < n)
                throw new Exception($"Space between locMin ({locMin}) and " +
                                    $"locMax ({locMax} must be greater than or " +
                                    $"equal to n ({n})).");
            if (rangeMins.Length < dim || rangeMaxes.Length < dim)
                throw new Exception($"Sizes of arrays lowRanges ({rangeMins.Length}) " +
                                    $"and highRanges must equal dimensionality ({dim}).");
            for (int i = 0; i < dim; i++)
                if (rangeMins[i] >= rangeMaxes[i])
                    throw new Exception($"Minimum ({rangeMins[i]}) in orthogonal range " +
                                        "search must be less than or equal to maximum " +
                                        $"({rangeMaxes[i]}) in dimension {dim}.");
            
            // Build range tree

            List<CoordNode> newNodes = new List<CoordNode>();
            newNodes.Add(new CoordNode(56, 10, 79));
            newNodes.Add(new CoordNode(41, 30, 39));
            newNodes.Add(new CoordNode(70, 50, 70));
            newNodes.Add(new CoordNode(28, 51, 62));
            newNodes.Add(new CoordNode(76, 45, 83));
            newNodes.Add(new CoordNode(87, 52, 63));
            newNodes.Add(new CoordNode(49, 55, 18));
            newNodes.Add(new CoordNode(10, 61, 45));
            newNodes.Add(new CoordNode(50, 70, 67));
            newNodes.Add(new CoordNode(74, 83, 27));
            CoordNode[] nodes = newNodes.ToArray();

            // List<CoordNode> newNodes = new List<CoordNode>();
            // newNodes.Add(new CoordNode(1, 56, 61));
            // newNodes.Add(new CoordNode(2, 41, 22));
            // newNodes.Add(new CoordNode(3, 70, 50));
            // newNodes.Add(new CoordNode(4, 28, 31));
            // newNodes.Add(new CoordNode(5, 69, 38));
            // newNodes.Add(new CoordNode(6, 87, 44));
            // newNodes.Add(new CoordNode(7, 49, 53));
            // newNodes.Add(new CoordNode(8, 10, 54));
            // newNodes.Add(new CoordNode(9, 50, 55));
            // newNodes.Add(new CoordNode(10, 74, 71));
            // CoordNode[] nodes = newNodes.ToArray();

            // CoordNode[] nodes = 
            //     NodeGenerator.GetCoordNodeList(n, sort:false, dimensions:dim,
            //                                    dataRangeMin:locMin, dataRangeMax:locMax,
            //                                    locRangeMin:locMin, locRangeMax:locMax,
            //                                    randomSeed:-1);
            
            ArrayUtils.Print(nodes, sep: "\n");

            RangeTree rt = new RangeTree(nodes, dim);
            
            // Print range tree in each dimension
            if (n <= 30) {
                for (int i = 1; i <= dim; i++) {
                    Console.WriteLine($"\n\nDimension {i} {u.Separator(85, 0)}");
                    RangeTreeHelper.VisualizeTree(rt.GetRootByDimension(i), 1, 10);
                }
            }

            // // Delete me - or maybe use as demo to show different next dim roots for each non leaf
            // // Print non leaf node and its next dimension pointer
            // Console.WriteLine("//////////");
            // ArrayUtils.PrintArray(rt.GetRoot().Left().GetCoordNodeList(), sep :"\n");
            // Console.WriteLine("---------- Dimension 1, root.Left()");
            // RangeTreeHelper.VisualizeTree(rt.GetRoot().Left(), 1, 10);
            // Console.WriteLine("\n\n---------- Dimension 2");
            // RangeTreeHelper.VisualizeTree(rt.GetRoot().Left().NextDimRoot(), 1, 10);
            // Console.WriteLine("//////////");

            // Show orthogonal range search
            Console.WriteLine(u.Separator());
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
            
            // Delete me
            // int searchFor = rt.GetRoot().GetCoordNodeList()[3].GetAttr(1) + 1;
            // Console.WriteLine($"Searching for {searchFor}");
            // Console.WriteLine(rt.FindNode(rt.GetRoot(), searchFor));
            ArrayUtils.Print(rt.OrthogonalRangeSearch(rangeMins, rangeMaxes), sep: "\n");
        }
    }
}
