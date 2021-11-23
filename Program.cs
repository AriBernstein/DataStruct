namespace Fractional_Cascading {
    class TestingClass {
        static void Main(string[] args) {
            
            // Demo fractional cascading matrices
            // -> Modify ---------
            int n1 = 200;
            int k1 = 1000;
            int x1 = (n1 * k1) / 3;
            // --------------------
            FCMatrixDemo fcmd = new FCMatrixDemo();
            fcmd.Demo(x1, n1, k1);


            // Demo for RangeTree
            // -> Modify ---------
            int n2 = 20;
            int k2 = 3;
            int locMin = 0;
            int locMax = n2 * k2;
            int[] rangeMins = new int[] {n2, n2, n2};
            int[] rangeMaxes = new int[] {n2 * 2, n2 * 2, n2 * 2};
            // --------------------
            RangeTreeDemo rtd = new RangeTreeDemo(n2, k2, locMin, locMax, rangeMins, rangeMaxes);
            rtd.demo();

        }
    }
}