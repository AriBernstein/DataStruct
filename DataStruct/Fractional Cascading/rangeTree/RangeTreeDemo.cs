using System;

namespace Fractional_Cascading {
    class RangeTreeDemo {
        RangeTreeHelper rth = new RangeTreeHelper();
        public RangeTreeDemo(int n, int dim, int[] lowRanges, int[] highRanges) {
            if (n < 1) throw new Exception("n parameter must be greater than 0,");
            if (dim > 3 || dim < 0)
                throw new Exception("dimension parameter must be greater than 0 and " +
                                    $"less than 4. Currently, dim = {dim}.");
            if (lowRanges.Length != dim || highRanges.Length != dim)
                throw new Exception("lowRanges and highRanges list sizes must equal " +
                                    "dimensionality.\nCurrently, lowRanges size = " +
                                    $"{lowRanges.Length}, highRanges size = " +
                                    $"{highRanges.Length}, dimensionality = {dim}");
            
            CoordNode[] nodes = 
                NodeGenerator.GetCoordNodeList(n, sort:false, dimensions:dim,
                                               dataRangeMin:n / 10, dataRangeMax:n * 10);
            RangeTree rt = new RangeTree(nodes);
            rth.VisualizeTree(rt.GetRoot(), 2, 10);

        }
    }
}