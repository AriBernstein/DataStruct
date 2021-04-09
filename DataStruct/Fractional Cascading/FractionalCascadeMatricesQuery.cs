using System;
using System.Linq;
using System.Collections.Generic; 


namespace Fractional_Cascading {
    /**
    Search for an element x located in inputCoordMatrix in k lists using 2
    methods: fractional cascading and the trivial method (k binary searches)
    -> return k locations of x   */
    public class FCMatricesQuery {
        
        private CoordNode[][] inputCoordMatrix;
        
        // Matrix of k lists of FractionalCascadingNodes, each of size n
        private FCNode[][] nodeMatrix;
       
        // Matrix of k-1 lists of FractionalCascadingNodes, each except for
        // the lastpromoted from the previous list
        private FCNode[][] nodeMatrixPrime;
        
        private int n; // number of elements in each list in nodeMatrix
        private int k; // number of lists


        public Dictionary<int, int> trivialSolution(int data) {
            /**
            Iterate through each list of coordinate nodes, perform binary
            search to find index holding node with data, extract location
            -> O(k log(n))  */

            // Return dictionary w/ key=dimension, pair=location in dimension
            Dictionary<int, int> locationsOfData = new Dictionary<int, int>();

            for(int i = 0; i < k; i++) {
                CoordNode[] arr = inputCoordMatrix[i];
                int nodeIndex = new BinarySearchNodes().binarySearch(arr, data, 0);
                int nodeData = arr[nodeIndex].getAttr(1);
                locationsOfData.Add(i + 1, nodeData);
            }

            return locationsOfData;
        }


        private bool targetNodeCheck(FCNode node, int targetData, int targetDimension) {
            return node != null &&
                   node.getData() == targetData &&
                   node.getDim() == targetDimension;
        }


        private FCNode findNodeFromPointerRange(FCNode node, int dim, int targetData) {
            /**
            Find the range of indices of either nodeMatrix or nodeMatrixPrime that contain
            the node we are looking for at the target dimension.
            
            Walk through promoted node pointers, starting with the list 2' and ending
            with list (k-1)', then do a final check on list k (not prime)    */
            
            FCNode prevNode = node.getPrevPointer();
            FCNode nextNode = node.getNextPointer();
            
            // Find range
            int lowRange, highRange;
            int dimIndex = dim - 1;

            if(prevNode == null) lowRange = 0;
            else lowRange = prevNode.getPreviouslyAugmentedIndex();

            if(nextNode == null) {
                if(dim == k) // list(k-1)' is built using lists k and k-1 (not prime)
                    highRange = nodeMatrix[dimIndex].Length;
                else highRange = nodeMatrixPrime[dimIndex].Length;
            } else highRange = nextNode.getPreviouslyAugmentedIndex();

            IEnumerable<int> range = Enumerable.Range(lowRange, highRange - lowRange);
            
            // Search range
            bool found = false;
            foreach (int j in range) {
                if(dim == k) // list(k-1)' is built using lists k and k-1 (not prime)
                    node = nodeMatrix[dimIndex][j];
                else node = nodeMatrixPrime[dimIndex][j];
                
                if(targetNodeCheck(node, targetData, dim)) {
                    found = true;
                    break;
                }
            }
            if(!(found)) Console.WriteLine(range.ToList()[range.ToArray().Length - 1]);
            if(!(found)) Console.WriteLine(nodeMatrixPrime[dimIndex][nodeMatrixPrime[dimIndex].Length - 1]);
            if(!(found)) throw new Exception("Can't find node with data " + targetData +
                                             " in dimension: " + dim +
                                             " during fractional cascading search");
            return node;
        }


        public Dictionary<int, int> fractionalCascadeSearch(int data) {
            /**
            Perform binary search to find data in first dimension.
            Then iteratively use the previous and/or next pointers to search the tiny
            range of the next dimension given by the prev and next node pointers    */
            BinarySearchNodes bsn = new BinarySearchNodes();

            // Return dictionary w/ key=dimension, pair=location in dimension
            Dictionary<int, int> locationsOfData = new Dictionary<int, int>();
            
            // Other variables we'll need
            FCNode dataNode = null;
            int dataIndex;
            int currentDim = 1;

            // Find data in first dimension
            dataIndex = bsn.binarySearch(nodeMatrixPrime[0], data, 0);
            dataNode = nodeMatrixPrime[0][dataIndex];
            
            // Ensure that dataNode is in the correct dimension - if not check neghbors
            if(dataNode.getDim() != currentDim) {
                FCNode next = dataNode.getNextPointer();
                FCNode prev = dataNode.getPrevPointer();
                if(targetNodeCheck(next, data, currentDim)) dataNode = next;
                else if (targetNodeCheck(prev, data, currentDim)) dataNode = prev;
                else throw new Exception("Cannot locate data in list 1' ");
            }

            // Assign first dimension location in return dictionary
            locationsOfData[currentDim] = dataNode.getAttr(1);

            for(int i = 1; i < nodeMatrix.Length; i++) {
                // Walk through promoted node pointers, starting with the list 2' until
                // list (k-1)', then do a final check on list k (not prime)
                // -> This logic is handled in findNodeFromPointerRange
                currentDim++ ;
                dataNode = findNodeFromPointerRange(dataNode, currentDim, data);
                locationsOfData[currentDim] = dataNode.getAttr(1);
            }

            return locationsOfData;
        }


        public FCMatricesQuery(int numValsPerList, int numLists, int unitFracDen=2,
                               bool print=true, int insertData=-1) {
            FractionalCascadingMatrices fcm;
            n = numValsPerList;
            k = numLists;
             
            if(insertData == -1) fcm = new FractionalCascadingMatrices(n, k);
            else fcm = new FractionalCascadingMatrices(n, k,
                                                       insertData: insertData,
                                                       print:print);
            
            inputCoordMatrix = fcm.getInputCoordMatrix();
            nodeMatrix = fcm.getFractionalCascadingNodeMatix();
            nodeMatrixPrime = fcm.getFractionalCascadingNodeMatixPrime();
        }
    }
}