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
        
        private CoordinateNode[][] inputCoordMatrix;
        // Matrix of k lists of FractionalCascadingNodes, each of size n
        private FractionalCascadingNode[][] nodeMatrix;
       
        // Matrix of k-1 lists of FractionalCascadingNodes, each except for
        // the lastpromoted from the previous list
        private FractionalCascadingNode[][] nodeMatrixPrime;
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
                CoordinateNode[] arr = inputCoordMatrix[i];
                int indx = bsn.binarySearchNode(arr, data, 0);
                int locAtIndex = arr[indx].getAttr(1);
                locationsOfData.Add(i + 1, locAtIndex);
            }
            return locationsOfData;
        }

        // private FractionalCascadingNode fcSearchWalkingHelper(FractionalCascadingNode node,
        //                                                       FractionalCascadingNode[] arr,
        //                                                       int startingIndex, int data,
        //                                                       int targetDimension) {
        //     /**
        //         Walk no more than k steps along arr in each direction until node found
        //         with data d and dimension targetDimension
        //     */
        //     int leftSteps = 0;
        //     int leftLim = k;
        //     int rightSteps = 0;
        //     int rightLim = k;
            
        //     // Limit steps to stay in bounds of arr
        //     if (startingIndex >= k) leftLim = startingIndex;
        //     if((arr.Length - startingIndex) >= k) rightLim = arr.Length - startingIndex;

        //     for(int i = 0; i < )                                          
        // }

        private FractionalCascadingNode nodeDimensionHelper(FractionalCascadingNode dataNode,
                                                            int data, int targetDimension) {
            /**
            Check if dataNode's matches target dimension. If not, check left and right nodes
            */
            if(dataNode.getDimension() != targetDimension) {
                FractionalCascadingNode prevNode = dataNode.getPrevPointer();
                FractionalCascadingNode nextNode = dataNode.getNextPointer();
                if(prevNode != null) {
                    if(prevNode.getAttr(0) == data && prevNode.getDimension() == targetDimension) {
                        return prevNode;
                    } else if (nextNode != null) {
                        if (nextNode.getAttr(0) == data && nextNode.getDimension() == targetDimension) {
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
            int dataIndx = bsn.binarySearchNode(nodeMatrixPrime[0], data, 0);
            FractionalCascadingNode dataNode = nodeDimensionHelper(nodeMatrixPrime[0][dataIndx], data, 1);
            locationsOfData[1] = dataNode.getAttr(1);
            return locationsOfData;
        }


        public FractionalCascadeMatricesQuery(int numValsPerList, int numLists, int unitFracDen=2, bool print=true) {
            FractionalCascadingMatrices fcm = new FractionalCascadingMatrices(numValsPerList, numLists);
            n = numValsPerList;
            k = numLists;
            inputCoordMatrix = fcm.getInputCoordMatrix();
            nodeMatrix = fcm.getFractionalCascadingNodeMatix();
            nodeMatrixPrime = fcm.getFractionalCascadingNodeMatixPrime();

            u.printDataLocationDict(trivialSolution(9507), 9507.ToString());
        }
    }
}