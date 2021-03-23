using System;
using System.Collections.Generic;

namespace Fractional_Cascading {
    public class CoordinateNodeListGenerator {
        public int[] randomUniqueIntsList(int count, int min=1, int max=100) {
            Random random = new Random();

            if (max <= min || max < count) {
                Console.WriteLine("Bad values for randomUniqueIntsList function input");
                return new int[0];
            }

            // generate count random values.
            HashSet<int> candidates = new HashSet<int>();
            while (candidates.Count < count) {
                // May strike a duplicate.
                candidates.Add(random.Next(min, max));
            }

            // load them in to a list.
            List<int> result = new List<int>();
            result.AddRange(candidates);

            // shuffle the results:
            int i = result.Count;  
            while (i > 1) {  
                i--;  
                int k = random.Next(i + 1);  
                int value = result[k];  
                result[k] = result[i];  
                result[i] = value;  
            }  
            return result.ToArray();
        }

        public CoordinateNode[] getCoordinateNodeList(int size, bool sort=true, int sortAttributeCode=0,
                                                      int dimensions=1, bool randomizeRadomSeed=false,
                                                      int randomSeed=10) {
            CoordinateNode[] nodeList = new CoordinateNode[size];
            int[] x_list = randomUniqueIntsList(size);
            int[] y_list = randomUniqueIntsList(size);
            int[] z_list = randomUniqueIntsList(size);

            if(randomizeRadomSeed) {
                Random r =new Random();
                randomSeed = r.Next();
            }
             
            Random random = new Random(randomSeed);
            int nodeData = random.Next(0, 10000);
            for(int i = 0; i < size; i++) {
                if(dimensions == 1) {
                    nodeList[i] = new CoordinateNode(nodeData, x_list[i]);
                } else if(dimensions == 2) {
                    nodeList[i] = new CoordinateNode(nodeData, x_list[i], y_list[i]);
                } else if(dimensions == 3) {
                    nodeList[i] = new CoordinateNode(nodeData, x_list[i], y_list[i], z_list[i]);
                }
                nodeData++;
            } 

            if(sort) {
                MergeSortNodes msn = new MergeSortNodes();
                msn.sort(nodeList, 1);
            }
            
            return nodeList;
        }

        
    }
}