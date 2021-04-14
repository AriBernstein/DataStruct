using System;
using System.Linq;
using System.Collections.Generic; 


namespace Fractional_Cascading {
    /**
    Search for an element x located in inputCoordMatrix in k lists using 2
    methods: fractional cascading and the trivial method (k binary searches)
    -> return k locations of x   */
    public class FCMatrixQuery {
        
        private CoordNode[][] InputCoordMatrix;
        private FCNode[][] NodeMatrixPrime;
        private int n; // number of elements in each list in nodeMatrix
        private int k; // number of lists

        public Dictionary<int, int> TrivialSolution(int data) {
            /**
            Iterate through each list of coordinate nodes, perform binary
            search to find index holding node with data, extract location
            -> O(k log(n))  */

            // Return dictionary w/ key=dimension, pair=location in dimension
            Dictionary<int, int> locationsOfData = new Dictionary<int, int>();
            BinarySearchNodes bsn = new BinarySearchNodes();
            for(int i = 0; i < k; i++) {
                CoordNode[] arr = InputCoordMatrix[i];
                int nodeIndex = bsn.BinarySearch(arr, data, 0);
                int nodeData = arr[nodeIndex].GetAttr(1);
                
                // Memory issues, this is a stupid amount of space especially
                // when we're only trying to record function timing
                if(k < 10000) locationsOfData.Add(i + 1, nodeData);
            }

            return locationsOfData;
        }


        private bool TargetNodeCheck(FCNode node, int targetData, int targetDimension) {
            return node != null &&
                   node.GetData() == targetData &&
                   node.GetDim() == targetDimension;
        }


        private FCNode FindNodeFromPointerRange(FCNode dataNode, int targetDimension) {
            /**
            Find and search a range of indices for the FCNode with the target data value
            at the target dimension.
            
            Finding range:
                DataNode comes from targetDimension - 1 . DataNode contains pointers to 
                FCNodes from the augmented list of the next dimension (TargetDimension).
                The search range is between the locations of DataNode.prev and
                DataNode.next in the augmented list in the TargetDimension. */

            int targetData = dataNode.GetData();
            FCNode prevNode = dataNode.getPrevPointer();
            FCNode nextNode = dataNode.getNextPointer();
            
            // Find range
            int lowRange, highRange;
            int targetDimIndex = targetDimension - 1;

            if(prevNode == null) lowRange = 0;
            else lowRange = prevNode.getPreviouslyAugmentedIndex();

            if(nextNode == null) highRange = NodeMatrixPrime[targetDimIndex].Length;
            else highRange = nextNode.getPreviouslyAugmentedIndex();

            IEnumerable<int> range = Enumerable.Range(lowRange, highRange - lowRange);
            
            // Search range
            bool found = false;
            foreach (int j in range) {
                dataNode = NodeMatrixPrime[targetDimIndex][j];
                
                if(TargetNodeCheck(dataNode, targetData, targetDimension)) {
                    found = true;
                    break;
                }
            }
            
            if(!(found)) throw new Exception("Can't find node with data " + targetData +
                                             " in dimension: " + targetDimension +
                                             " during fractional cascading search");
            return dataNode;
        }


        public Dictionary<int, int> FractionalCascadeSearch(int data) {
            /**
            Perform binary search to find data in first dimension. Then iteratively use
            the previous and/or next pointers to search the (tiny) range of the next
            dimension given by the prev and next node pointers    */
            BinarySearchNodes bsn = new BinarySearchNodes();

            // Return dictionary w/ key=dimension, pair=location in dimension
            Dictionary<int, int> locationsOfData = new Dictionary<int, int>();
            
            // Other variables we'll need
            FCNode dataNode = null;
            int dataIndex;
            int currentDim = 1;

            // Find data in first dimension
            dataIndex = bsn.BinarySearch(NodeMatrixPrime[0], data, 0);
            dataNode = NodeMatrixPrime[0][dataIndex];
            
            // Ensure that dataNode is in the correct dimension - if not check neghbors
            if(dataNode.GetDim() != currentDim) {
                FCNode next = dataNode.getNextPointer();
                FCNode prev = dataNode.getPrevPointer();
                if(TargetNodeCheck(next, data, currentDim)) dataNode = next;
                else if (TargetNodeCheck(prev, data, currentDim)) dataNode = prev;
                else {
                    string errorMsg = "Cannot locate data in augmented list 1'\n" +
                                      $"DataNode: {dataNode}\nIndex neighbor left:\t" +
                                      NodeMatrixPrime[0][dataIndex-1] +
                                      "\nIndex neighbor right:\t" +
                                      NodeMatrixPrime[0][dataIndex+1] +
                                      $"\nNextNode: {next}nPrevNode: {prev}";
                    throw new Exception(errorMsg);
                }
            }

            if(k < 10000) // This takes up too much memmory
                // Assign first dimension location in return dictionary
                locationsOfData[currentDim] = dataNode.GetAttr(1);

            for(int i = 1; i < NodeMatrixPrime.Length; i++) {
                // Walk through promoted node pointers, starting with the list 2' until
                // list (k-1)', then do a final check on list k (not prime)
                // -> This logic is handled in findNodeFromPointerRange
                currentDim++ ;
                dataNode = FindNodeFromPointerRange(dataNode, currentDim);
                locationsOfData[currentDim] = dataNode.GetAttr(1);
            }

            return locationsOfData;
        }


        public FCMatrixQuery(int numValsPerList, int numLists, int unitFracDen=2,
                               bool print=true, int insertData=-1,
                               bool randNodeAttrOrders=true) {
            FractionalCascadingMatrix fcm;
            n = numValsPerList;
            k = numLists;
             
            if(insertData == -1) {
                fcm = new FractionalCascadingMatrix(n, k,
                                                    print:print,
                                                    randNodeAttrOrders:
                                                        randNodeAttrOrders);
            }
            else fcm = new FractionalCascadingMatrix(n, k, insertData: insertData,
                                                     print:print,
                                                     randNodeAttrOrders:
                                                        randNodeAttrOrders);
            
            InputCoordMatrix = fcm.GetInputCoordMatrix();
            NodeMatrixPrime = fcm.GetFCNodeMatixPrime();
        }
    }
}