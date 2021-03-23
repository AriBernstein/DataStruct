using System;

namespace Fractional_Cascading {
    /**
    Fractional Cascading implementation of search for all locations of
    value in k lists (dimensions) with complexity of O(log(n) + k)

    For each list starting at list i = k-1, build list i'
    -> these lists are  referred to as augmented or prime lists.
    -> list i' contains all elements of list k-1 and every second element
       of list k.
    -> elements from list i-1' are "augmented" into list i to build list i'
    -> list 0' is sized ~2n 
    */
    public class FractionalCascadingMatrices {
        Utils u = new Utils();
        
        private CoordinateNode[][] inputCoordMatrix;
        // Matrix of k lists of FractionalCascadingNodes, each of size n
        private FractionalCascadingNode[][] nodeMatrix;
       
        // Matrix of k-1 lists of FractionalCascadingNodes, each except for
        // the lastaugmented from the previous list
        private FractionalCascadingNode[][] nodeMatrixPrime;
        private int n; // number of elements in each list in nodeMatrix
        private int k; // number of lists

        public CoordinateNode[][] getInputCoordMatrix() {
            return inputCoordMatrix;
        }
        public FractionalCascadingNode[][] getFractionalCascadingNodeMatix() {
            return nodeMatrix;
        }
        public FractionalCascadingNode[][] getFractionalCascadingNodeMatixPrime() {
            return nodeMatrixPrime;
        }

        private void setPointers(FractionalCascadingNode[] fcNodeListAugmented) {
            /**
            For each node in the fcNodeListAugmented, assign a previous and next
            pointer to the previous and next nodes in the list of a different 
            dimension.

            Parameter fcNodeListAugmented - list of FractionalCascadingNodes 
                      containing FractionalCascadingNodes from lists of 
                      higher dimensions, ordered by the data attribute
            */
            
            // Handle previous node pointers
            for(int i = 1; i < fcNodeListAugmented.Length; i++) {
                FractionalCascadingNode currNode = fcNodeListAugmented[i];
                for(int j = i-1; j >= 0; j--) {
                    FractionalCascadingNode prevNode = fcNodeListAugmented[j];
                    if(prevNode.getDimension() != currNode.getDimension()) {
                        currNode.setPrevPointer(prevNode);
                        break;
                    }
                }
            }

            // Handle next node pointers
            for(int i = 0; i < fcNodeListAugmented.Length - 1; i++) {
                FractionalCascadingNode currNode = fcNodeListAugmented[i];
                for(int j = i + 1; j < fcNodeListAugmented.Length; j++) {
                    FractionalCascadingNode nextNode = fcNodeListAugmented[j];
                    if(nextNode.getDimension() != currNode.getDimension()) {
                        currNode.setNextPointer(nextNode);
                        break;
                    }
                }
            }
        }

        private void setFCMatrixFromCordMatrix(CoordinateNode[][] coordNodeMatrix) {
            // Convert 2D array of CoordinateNodes into 2D array of
            // FractionalCascadingNodes. Set as nodeMatrix
            nodeMatrix = new FractionalCascadingNode[k][];
            Utils u = new Utils();
            for (int i = 0; i < k; i++) {
                nodeMatrix[i] = new FractionalCascadingNode[n];
                for(int j = 0; j < n; j++) {
                    CoordinateNode thisCoordNode = coordNodeMatrix[i][j];
                    FractionalCascadingNode thisFCNode =
                        new FractionalCascadingNode(thisCoordNode, (i + 1), j);                    
                    nodeMatrix[i][j] = thisFCNode;
                }
            }
        }

        private int getPrimeListSize(double fraction, int prevPrimeListLen) {
            // Calculate the size of a prime list, or one augmented using
            // the current list of FractinoalCascadingNodes and the previous
            // augmented list of FractinoalCascadingNodes
            return Convert.ToInt32(n + Math.Ceiling(prevPrimeListLen * fraction));
        }

        private FractionalCascadingNode[] buildListPrime(FractionalCascadingNode[] FCNodeList1,
                                                         FractionalCascadingNode[] FCNodeList2,
                                                         int unitFracDen, int prevPrimeListLen) {
            /**
            Combine all elements from fcNodeListOne and a fraction of the values from fcNodeListTwo
            to a new list of FractionalCascadingNodes. 

            fcNodeListOne and fcNodeListTwo must be ordered according to their data 

            Note:   the first time this is run (for the kth list),  FCNodeList1 = list(k-1)
                                                                    FCNodeList2 = list(k)
                    
                    all other times this is run,    FCNodeList1 = list(i-1)'
                                                    FCNodeList2 = list(i)
            */

            double fraction = 1 / (double)unitFracDen;  // get the unit fraction
            
            // Instantiate augmented list
            int primeListSize = getPrimeListSize(fraction, prevPrimeListLen);       
            FractionalCascadingNode[] fcNodeListAugmented = new FractionalCascadingNode[primeListSize];

            // Build list of elements from FCNodeList2 to augment with FCNodeList1
            FractionalCascadingNode[] fcNodeList2AugmentedNodes =
                    new FractionalCascadingNode[(int)Math.Ceiling(prevPrimeListLen * fraction)];
            
            int c = 0; // index counter for fcNodeList2AugmentedNodes
            for(int i = 0; i < FCNodeList2.Length; i += unitFracDen) {
                fcNodeList2AugmentedNodes[c++] = FCNodeList2[i].makeCopy();
            }
            
            // Combine elements from fcNodeList2AugmentedNodes and FCNodeList1 into fcNodeListAugmented
            // Maintain order
            c = 0;      // index counter for fcNodeList2AugmentedNodes
            int d = 0;  // index counter for FCNodeList1
            int j = 0;  // index counter for fcNodeListAugmented
            
            while(c < fcNodeList2AugmentedNodes.Length && d < prevPrimeListLen) {
                if (FCNodeList1[d].getAttr(0) < fcNodeList2AugmentedNodes[c].getAttr(0)) {
                    fcNodeListAugmented[j] = FCNodeList1[d++].makeCopy();
                } else fcNodeListAugmented[j] = fcNodeList2AugmentedNodes[c++].makeCopy();
                j++;
            }

            // Add leftover values:
            while(c < fcNodeList2AugmentedNodes.Length) {
                fcNodeListAugmented[j++] = fcNodeList2AugmentedNodes[c++].makeCopy();
            }
            while(d < n) {
                fcNodeListAugmented[j++] = FCNodeList1[d++].makeCopy();
            }
            
            // Set elements new elements to prime, assign pointers
            // TODO- Make this more efficient
            foreach(FractionalCascadingNode f in fcNodeListAugmented) f.prime = true;
            setPointers(fcNodeListAugmented);
            
            return fcNodeListAugmented;
        }

        private void setFCPrimeMatrix(int unitFracDen) {
            // re. k-1 size of nodeMatrixPrime d1, augmented lists are in terms of d and d-1
            // st. primes of every list total at k-1 lists
            nodeMatrixPrime = new FractionalCascadingNode[k-1][];
            
            // build list(k') - only augmented list build using 2 non-augmented lists
            FractionalCascadingNode[] dim1FCNodelist = nodeMatrix[k-2];
            FractionalCascadingNode[] dim2PrimeFCNodelist = nodeMatrix[k-1];
            nodeMatrixPrime[k-2] = buildListPrime(dim1FCNodelist, dim2PrimeFCNodelist, unitFracDen, n);

            // build the remaining augmented lists by nodes from list(i - 1') into list(i)
            for(int i = k-3; i >= 0; i--) {
                dim1FCNodelist = nodeMatrix[i];
                dim2PrimeFCNodelist = nodeMatrixPrime[i + 1];
                nodeMatrixPrime[i] = buildListPrime(dim1FCNodelist, dim2PrimeFCNodelist,
                                                    unitFracDen, dim2PrimeFCNodelist.Length);
            }
        }

        private void setCoordMatrix() {
            // Build k lists of n CoordinateNodes each, sorted by their location (xLox)
            // Each list will have nodes which share data values but have various xLoc values
            // Set as inputCoordMatrix
            CoordinateNodeListGenerator cnlg = new CoordinateNodeListGenerator();
            CoordinateNode[][] coordNodeMatrix = new CoordinateNode[k][];
            for (int i = 0; i < k; i++) coordNodeMatrix[i] = cnlg.getCoordinateNodeList(n);
            inputCoordMatrix = coordNodeMatrix;
        }

        private void sortCordMatrixOnData(CoordinateNode[][] matrx) {
            // Sort each array in matrx by the data values of the coordinate nodes
            MergeSortNodes msn = new MergeSortNodes();
            foreach(CoordinateNode[] arr in matrx) msn.sort(arr, 0);
        }

        public FractionalCascadingMatrices(int numValsPerList, int numLists, int unitFracDen=2, bool print=true) {
            /**
            Parameters:
                numValsPerList: k
                numLists:       n
                unitFracDen:    Denominator of the unit fraction used to augment the lists into prime lists
                                -> unitFracDen=2 -> augment using every second value st. list i' contains all
                                                    of the values from list i and 1/2 if the values from
                                                    list i-1'
                print:          this is a demo afterall
            */

            n = numValsPerList;
            k = numLists;
            
            if(print) {
                Console.WriteLine("N: " + n);
                Console.WriteLine("K: " + k);
            }
            if(unitFracDen <= 1) throw new Exception("Invalid unitFracDen, must be greater than 1");
            
            // Convert coordNodeMatrix to one of FractionalCascadingNodes, assign as nodeMatrix
            // -> start with matrix of coordNodes sorted by their location (xLoc)
            Console.WriteLine("CoordinateMatrix - input");
            setCoordMatrix();
            if(print) u.printNodeMatrix(inputCoordMatrix);

            // Sort each array in coordinate matrix on its data value and set it
            sortCordMatrixOnData(inputCoordMatrix);
            setFCMatrixFromCordMatrix(inputCoordMatrix);
            Console.WriteLine("NodeMatrix");
            if(print) u.printNodeMatrix(nodeMatrix);

            // Build and assign nodeMatrixPrime from just-assigned nodeMatrix
            setFCPrimeMatrix(unitFracDen);
            Console.WriteLine("NodeMatrixPrime");
            if(print) u.printNodeMatrix(nodeMatrixPrime);
        }
    }
}