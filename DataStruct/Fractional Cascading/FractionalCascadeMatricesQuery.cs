using System;

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
        // the lastaugmented from the previous list
        private FractionalCascadingNode[][] nodeMatrixPrime;
        private int n; // number of elements in each list in nodeMatrix
        private int k; // number of lists

        private int[] trivialSolution(int data) {
            // Iterate through each list of coordinate nodes, perform binary
            // search to find index holding node with data, extract location
            int[] locationsOfData = new int[k];
            BinarySearchNodes bsn = new BinarySearchNodes();
            for(int i = 0; i < k; i++) {
                CoordinateNode[] arr = inputCoordMatrix[i];
                int indx = bsn.binarySearchCoordNode(arr, data, 0);
                int locAtIndex = arr[indx].getAttr(1);
                locationsOfData[i] = locAtIndex;
            }

            return locationsOfData;
        }

        public FractionalCascadeMatricesQuery(int numValsPerList, int numLists, int unitFracDen=2, bool print=true) {
            FractionalCascadingMatrices fcm = new FractionalCascadingMatrices(numValsPerList, numLists);
            n = numValsPerList;
            k = numLists;
            inputCoordMatrix = fcm.getInputCoordMatrix();
            nodeMatrix = fcm.getFractionalCascadingNodeMatix();
            nodeMatrixPrime = fcm.getFractionalCascadingNodeMatixPrime();

            u.printDataLocationArray(trivialSolution(9507));
        }
    }
}