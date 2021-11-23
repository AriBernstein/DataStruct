namespace Fractional_Cascading {
    class RangeTreeDemo {
        Utils u = new Utils();

        RangeTree rt;
        int n;
        int dim;
        int[] rangeMins;
        int[] rangeMaxes;

        public RangeTreeDemo(int n, int dim, int locMin, int locMax,
                             int[] rangeMins, int[] rangeMaxes,
                             int insertXValue=-1, int insertYValue=-1,
                             int insertZValue=-1, int randomSeed=-1) {
                                 
            // Check arguments - if any are wrong, this can explode and I'm a pacifist
            if (n < 1) {
                throw new Exception("n parameter must be greater than 0.");
            } else if (locMin > locMax) {
                throw new Exception(
                    $"locMin value ({locMin}) must be less than or equal to locMax " +
                    $"value ({locMax}).");
            } else if (locMax - locMin < n) {
                throw new Exception(
                    $"Space between locMin ({locMin}) and locMax ({locMax} must be " +
                    $"greater than or equal to n ({n})).");
            } else if (rangeMins.Length != rangeMaxes.Length) {
                throw new Exception(
                    $"Sizes of arrays rangeMins ({rangeMins.Length}) and rangeMaxes " +
                    $"({rangeMaxes.Length}) must be equal.");
            } else if (rangeMins.Length < dim) {
                throw new Exception(
                    $"Sizes of arrays rangeMins and rangeMaxes ({rangeMins.Length}) " +
                    $"must be less than or equal to dimensionality ({dim}).");
            } else {
                for (int i = 0; i < dim; i++) {
                    if (rangeMins[i] >= rangeMaxes[i])
                        throw new Exception(
                            $"Minimum ({rangeMins[i]}) in orthogonal range search must " +
                            $"be less than maximum ({rangeMaxes[i]}) - dimension {dim}.");
                }
            }

            this.n = n;
            this.dim = dim;
            this.rangeMins = rangeMins;
            this.rangeMaxes = rangeMaxes;

            CoordNode[] nodes = 
                NodeGenerator.GetCoordNodeList(n, sort:false, dimensions:dim,
                                               dataRangeMin:locMin, dataRangeMax:locMax,
                                               locRangeMin:locMin, locRangeMax:locMax,
                                               randomSeed:-1);
            
            ArrayUtils.Print(nodes, sep: "\n");
            this.rt = new RangeTree(nodes, dim);
        }

        public RangeTree rangeTree() {
            return this.rt;
        }

        public void demo() {
            
            // Print range tree in each dimension
            if (n <= 30) {
                for (int i = 1; i <= dim; i++) {
                    Console.WriteLine($"\n\nDimension {i} {u.Separator(85, 0)}");
                    RangeTreeHelper.Visualize(rt.GetRootByDimension(i), 0, 10);
                }
            }

            // Delete me - or maybe use as demo to show different next dim roots for each non leaf
            // Print non leaf node and its next dimension pointer
            Console.WriteLine("//////////");
            ArrayUtils.Print(rt.GetRoot().Left().GetCoordNodeList(), sep :"\n");
            Console.WriteLine("---------- Dimension 1, root.Left()");
            RangeTreeHelper.Visualize(rt.GetRoot().Left(), 0, 10);
            Console.WriteLine("\n\n---------- Dimension 2");
            RangeTreeHelper.Visualize(rt.GetRoot().Left().NextDimRoot(), 0, 10);
            Console.WriteLine("//////////");

            // Show orthogonal range search
            Console.WriteLine(u.Separator());
            if (dim == 1) 
                Console.WriteLine("CoordNodes located in range: " +
                                  $"x: ({rangeMins[0]}, {rangeMaxes[0]})");
            if (dim == 2)
                Console.WriteLine("CoordNodes located in range:\n" +
                                  $"\tx: ({rangeMins[0]}, {rangeMaxes[0]}), " +
                                  $"\ty: ({rangeMins[1]}, {rangeMaxes[1]})");
            if (dim == 3)
                Console.WriteLine("CoordNodes located in range:\n" +
                                  $"\tx: ({rangeMins[0]}, {rangeMaxes[0]}), " +
                                  $"\ty: ({rangeMins[1]}, {rangeMaxes[1]}), " +
                                  $"\tz: ({rangeMins[2]}, {rangeMaxes[2]})");
            
            // Perform Search
            CoordNode [] searchResult = rt.OrthogonalRangeSearch(rangeMins, rangeMaxes);
                        
            if (searchResult.Length > 0) {
                ArrayUtils.Print(searchResult, sep: "\n");
            } else {
                Console.WriteLine("No nodes found in specified range.");
            }
        }
    }
}
