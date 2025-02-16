namespace Fractional_Cascading {
    class FCMatrixDemo {
        Utils u = new Utils();

        public void Demo(int x, int n, int k, bool print=true) {
            /**
            Print search output iff k <= 100 */

            Console.WriteLine("Starting Fractional Cascading Matrix Search Demo\n");
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            
            FCMatrixQuery fcmq = new FCMatrixQuery(n, k, insertData:x, print:print,
                                                   randNodeAttrOrders:false);
            
            Console.WriteLine("\nPerfroming binary search...");
            watch.Start();
            if (k <= 100 && print)
                u.PrintDataLocationDict(fcmq.TrivialSolution(x), x.ToString());
            else fcmq.TrivialSolution(x);
            watch.Stop();
            int trivialMS = (int)watch.ElapsedMilliseconds;
            
            Console.WriteLine("\nPerforming Fractional Cascading search...");
            watch.Reset();
            watch.Start();
            if (k <= 100)
                u.PrintDataLocationDict(fcmq.FractionalCascadingSearch(x), x.ToString());
            else fcmq.FractionalCascadingSearch(x);
            watch.Stop();
            int fcMS = (int)watch.ElapsedMilliseconds;

            Console.WriteLine(u.Separator(12, newLinesBelow:0));
            Console.WriteLine("n: " + n + ", " + "\tk: " + k + "\tx: " + x);
            Console.WriteLine($"Execution Time of trivial solution: {trivialMS} ms");
            Console.WriteLine(
                $"Execution Time of fractional cascading solution: {fcMS} ms");
            Console.WriteLine(
                "Ratio of duration of FC solution vs trivial: " +
                $"{(float)fcMS / trivialMS}\n\n");
        }

        public void CSV_Loop(String CSVFileName,
                             int kMin, int kMax, int kIncr,
                             int nMin, int nMax, int nIncr) {

            List<Record> fractionalCascadingStats = new List<Record>();
            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            Random random = new Random();
            int x;
            for (int k = kMin; k <= kMax; k+= kIncr) {
                Console.WriteLine("K: " + k);
                    for (int n = nMin; n <= nMax; n += nIncr){
                        x = random.Next(0, nMin * kMin);
                        FCMatrixQuery fcmq =
                            new FCMatrixQuery(n, k, insertData:x, print:false,
                                              randNodeAttrOrders:false);
                        
                        // Trivial solution
                        watch.Start();
                        fcmq.TrivialSolution(x);
                        watch.Stop();
                        float trivialTime = watch.ElapsedMilliseconds;
                        
                        // Fractional Cascading solution
                        watch.Reset();
                        watch.Start();
                        fcmq.FractionalCascadingSearch(x);
                        watch.Stop();
                        float FCTime = watch.ElapsedMilliseconds;
                        float ratio = FCTime / trivialTime;

                        Record rec = new Record(n, k, x, trivialTime, FCTime, ratio);
                        Console.WriteLine(
                            $"\nn: {n}, k: {k}, x: {x}\nTrivial: {trivialTime} ms\t " + 
                            $"FC: {FCTime} ms\tFC:Trivial Ratio: {ratio}");
                        fractionalCascadingStats.Add(rec);
                }
            }
            
            new CSVHelper(CSVFileName).WriteCSV(fractionalCascadingStats);
            Console.WriteLine("\n\nAll done :)\n\n");
        }
    }
}
