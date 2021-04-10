using System;
using System.Collections.Generic;
using DataStruct;

namespace DataStruct_Examples {
    static class FractionalCascading {
        public static void Demo() {
            int x = 1234567;
            int n = 2000000;
            int k = 1000;

            FCMatrixDemo fcmd = new FCMatrixDemo();
            fcmd.demo(x, n, k);
        }
    }
}
