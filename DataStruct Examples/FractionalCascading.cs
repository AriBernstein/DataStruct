using System;
using System.Collections.Generic;
using DataStruct;

namespace DataStruct_Examples {
    static class FractionalCascading {
        public static void Demo() {
            Utils u = new Utils();
            Random random = new Random();
            var watch = new System.Diagnostics.Stopwatch();
            
            int x = 99999;
            int n = 200000;
            int k = 100;
            
            FCMatricesQuery fcmq = new FCMatricesQuery(n, k, insertData:x);
            
            Console.WriteLine("\nBinary search:");
            watch.Start();
            u.printDataLocationDict(fcmq.trivialSolution(x), x.ToString());
            watch.Stop();
            int trivialMS = (int)watch.ElapsedMilliseconds;
            
            Console.WriteLine("\nFractional Cascading search:");
            watch.Reset();
            watch.Start();
            u.printDataLocationDict(fcmq.fractionalCascadeSearch(x), x.ToString());
            watch.Stop();
            int fcMS = (int)watch.ElapsedMilliseconds;

            Console.WriteLine("\n------------");
            Console.WriteLine("n: " + n + ", " + "\tk: " + k);
            Console.WriteLine($"Execution Time of trivial solution: {trivialMS} ms");
            Console.WriteLine($"Execution Time of fractional cascading solution: {fcMS} ms");
            Console.WriteLine($"Ratio of duration of trivial solution vs FC: {(float)fcMS / trivialMS}");
        }
    }
}
