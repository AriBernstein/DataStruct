using System;
using System.Linq;
using System.Collections.Generic; 


namespace Fractional_Cascading {
    /**
    Search for an element x located in inputCoordMatrix in k lists using 2
    methods: fractional cascading and the trivial method (k binary searches)
    -> return k locations of x   */
    public class FractionalCascadeMatricesQuery {
        Utils u = new Utils();
        
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

        private FCNode nodeNeighborCheck(FCNode node, int data, int targetDim) {
            // If dataNode's matches target dimension; else, check left and right nodes.
            if(node.getDim() == targetDim) return node; // Check dataNode

            // Check neighbors
            FCNode prevNode = node.getPrevPointer();
            FCNode nextNode = node.getNextPointer();

            if(prevNode != null) {  // Check left neighbor
                if(prevNode.getData() == data && prevNode.getDim() == targetDim) {
                    return prevNode;
                } else if (nextNode != null) {  // Check right neighbor
                    if (nextNode.getData() == data && nextNode.getDim() == targetDim) {
                        return nextNode;
                    }
                }
            } throw new Exception("Cannot find node in dimension " + targetDim);
        }

        private FCNode findNodeFromPointerRange(FCNode node, int currDim, int targetData) {
            /**
            Find the range of indices of either nodeMatrix or nodeMatrixPrime that contain
            the node we are looking for at the target dimension.
            Walk through promoted node pointers, starting with the list 2' and ending
            // with list (k-1)', then do a final check on list k (not prime)    */
            
            FCNode prevNode = node.getPrevPointer();
            FCNode nextNode = node.getNextPointer();
            
            // Find range
            int lowRange, highRange;
            int currIndex = currDim - 1;

            if(prevNode == null) lowRange = 0;
            else lowRange = prevNode.getPreviouslyAugmentedIndex();

            if(nextNode == null) highRange = nodeMatrixPrime[currIndex].Length - 1;
            else highRange = nextNode.getPreviouslyAugmentedIndex();

            IEnumerable<int> range = Enumerable.Range(lowRange, highRange - lowRange);
            
            // Search range
            bool found = false;
            foreach (int j in range) {
                if(currDim == k) node = nodeMatrix[currIndex][j];
                else node = nodeMatrixPrime[currIndex][j];
                
                if(node.getData() == targetData && node.getDim() == currDim) {
                    found = true;
                    break;
                }
            }
                
            if(!(found)) throw new Exception("Can't find node in dimension: " + currDim);
            return node;
        }


        public Dictionary<int, int> fractionalCascadeSearch(int data) {
            /**
            Perform binary search to find data in first dimension
            Then recursively use the previous and/or next pointers to search
            the tiny range of the next dimension    */
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
            
            // Insure that dataNode is in the correct dimension
            dataNode = nodeNeighborCheck(dataNode, data, 1);
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


        public FractionalCascadeMatricesQuery(int numValsPerList, int numLists,
                                              int unitFracDen=2, bool print=true) {
            FractionalCascadingMatrices fcm = 
                new FractionalCascadingMatrices(numValsPerList, numLists);
            n = numValsPerList;
            k = numLists;
            inputCoordMatrix = fcm.getInputCoordMatrix();
            nodeMatrix = fcm.getFractionalCascadingNodeMatix();
            nodeMatrixPrime = fcm.getFractionalCascadingNodeMatixPrime();
        }
    }
}