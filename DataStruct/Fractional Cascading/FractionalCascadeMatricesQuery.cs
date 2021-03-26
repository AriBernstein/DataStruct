using System;
using System.Collections.Generic; 


namespace Fractional_Cascading {
    /**
    Search for an element x located in inputCoordMatrix in k lists using 2
    methods: fractional cascading and the trivial method (k binary searches)
    -> return k locations of x
    */
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

        private Dictionary<int, int> trivialSolution(int data) {
            /**
            Iterate through each list of coordinate nodes, perform binary
            search to find index holding node with data, extract location
            -> O(k log(n))  */

            // Return dictionary w/ key=dimension, pair=location in dimension
            Dictionary<int, int> locationsOfData = new Dictionary<int, int>();

            BinarySearchNodes bsn = new BinarySearchNodes();
            for(int i = 0; i < k; i++) {
                CoordNode[] arr = inputCoordMatrix[i];
                int indx = bsn.binarySearch(arr, data, 0);
                int locAtIndex = arr[indx].getAttr(1);
                locationsOfData.Add(i + 1, locAtIndex);
            }
            return locationsOfData;
        }

        private FCNode nodeNeighborCheck(FCNode dataNode, int data, int targetDimension) {
            /**
            Check if dataNode's matches target dimension. If not, check left and right nodes
            */
            if(dataNode.getDim() != targetDimension) {
                FCNode prevNode = dataNode.getPrevPointer();
                FCNode nextNode = dataNode.getNextPointer();
                if(prevNode != null) {
                    if(prevNode.getData() == data && prevNode.getDim() == targetDimension) {
                        return prevNode;
                    } else if (nextNode != null) {
                        if (nextNode.getData() == data && nextNode.getDim() == targetDimension) {
                            return nextNode;
                        }
                    }
                } throw new Exception("Cannot find node in dimension");
            } else return dataNode;
        }

        private Dictionary<int, int> fractionalCascadeSearch(int data) {
            /**
            Perform binary search to find data in first dimension
            Then recursively use the previous and/or next pointers to search
            the tiny range of the next dimension    */
            BinarySearchNodes bsn = new BinarySearchNodes();

            // Return dictionary w/ key=dimension, pair=location in dimension
            Dictionary<int, int> locationsOfData = new Dictionary<int, int>();
            
            // Find data in first dimension
            int dataIndx = bsn.binarySearch(nodeMatrixPrime[0], data, 0);
            FCNode dataNode = nodeNeighborCheck(nodeMatrixPrime[0][dataIndx], data, 1);
            locationsOfData[1] = dataNode.getAttr(1);
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

            int x = 1503;
            u.printDataLocationDict(trivialSolution(x), x.ToString());
        }
    }
}