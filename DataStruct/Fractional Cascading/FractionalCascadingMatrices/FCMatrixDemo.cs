using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    class FCMatrixDemo {
        Utils u = new Utils();
        Random random = new Random();

        public void demo(int x, int n, int k, bool print=true, bool consistentSeed=true) {
            Console.WriteLine("Starting Fractional Cascading Matrix Search Demo\n");
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            
            FCMatricesQuery fcmq = new FCMatricesQuery(n, k, insertData:x, print:print,
                                                       consistentSeed:consistentSeed);
            
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
            Console.WriteLine("n: " + n + ", " + "\tk: " + k + "\tx: " + x);
            Console.WriteLine($"Execution Time of trivial solution: {trivialMS} ms");
            Console.WriteLine($"Execution Time of fractional cascading solution: {fcMS} ms");
            Console.WriteLine($"Ratio of duration of FC solution vs trivial: {(float)fcMS / trivialMS}\n\n");
        }

        public void CSV_Loop(int x, String CSVFileName,
                             int kMin, int kMax, int kIncr,
                             int nMin, int nMax, int nIncr) {

            List<Record> fractionalCascadingStats = new List<Record>();
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();

            for(int k = kMin; k < kMax; k+= kIncr) {
                Console.WriteLine("K: " + k);
                    for(int n = nMin; n < nMax; n += nIncr){
                        x = random.Next(0, n);
                        FCMatricesQuery fcmq =
                            new FCMatricesQuery(n, k, insertData:x, print:false);
                        
                        // Trivial solution
                        watch.Start();
                        fcmq.trivialSolution(x);
                        watch.Stop();
                        float trivialTime = watch.ElapsedMilliseconds;
                        
                        // Fractional Cascading solution
                        watch.Reset();
                        watch.Start();
                        fcmq.fractionalCascadeSearch(x);
                        watch.Stop();
                        float FCTime = watch.ElapsedMilliseconds;

                        Record rec = new Record(n, k, x, trivialTime, FCTime);
                        Console.WriteLine("\n\nn: " + n + "\tk: " + k + "\tx: " + x +
                            "\nTrivial: " + trivialTime + "\tFC: " + FCTime);
                        fractionalCascadingStats.Add(rec);
                }
            }
            
            new CSVHelper(CSVFileName).writeCsv(fractionalCascadingStats);
            Console.WriteLine("\n\nAll done :)\n\n");
        }
    }
}