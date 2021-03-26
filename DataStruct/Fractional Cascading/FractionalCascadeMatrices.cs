using System;

namespace Fractional_Cascading {
    /**
    Fractional Cascading implementation of search for all locations of
    value in k lists (dimensions) with complexity of O(log(n) + k)

    For each list starting at list i = k-1, build list i'
    -> these lists are  referred to as promoted or prime lists.
    -> list i' contains all elements of list k-1 and every second element
       of list k.
    -> elements from list i-1' are "promoted" into list i to build list i'
    -> list 0' is sized ~2n 
    */
    public class FractionalCascadingMatrices {
        Utils u = new Utils();
        private CoordNode[][] inputCoordMatrix;
        // Matrix of k lists of FractionalCascadingNodes, each of size n
        private FCNode[][] nodeMatrix;
       
        // Matrix of k-1 lists of FractionalCascadingNodes, each except for
        // the lastPromoted from the previous list
        private FCNode[][] nodeMatrixPrime;
        private int n; // number of elements in each list in nodeMatrix
        private int k; // number of lists

        public CoordNode[][] getInputCoordMatrix() {
            return inputCoordMatrix;
        }
        public FCNode[][] getFractionalCascadingNodeMatix() {
            return nodeMatrix;
        }
        public FCNode[][] getFractionalCascadingNodeMatixPrime() {
            return nodeMatrixPrime;
        }
        
        private void setPointers(FCNode[] primeList) {
            /**
            primeList represents the augmented list primeList(i). It contains 
            FractionalCascadingNodes from both the intial list(i) and
            primeList(i - 1).

            For each node in primeList(i), assign a previous and next
            pointer to the previous and next nodes in the list that were
            not promoted from primeList(i - 1).

            Parameter primeList
             - list of FractionalCascadingNodes 
                      containing FractionalCascadingNodes from lists of 
                      higher dimensions, ordered by the data attribute
            */
            
            // Handle previous node pointers
            for(int i = 1; i < primeList.Length; i++) {
                FCNode currNode = primeList[i];
                for(int j = i-1; j >= 0; j--) {
                    FCNode prevNode = primeList[j];
                    if(prevNode.isPromoted() != currNode.isPromoted()) {
                        currNode.setPrevPointer(prevNode);
                        break;
                    }
                }
            }

            // Handle next node pointers
            for(int i = 0; i < primeList.Length - 1; i++) {
                FCNode currNode = primeList[i];
                for(int j = i + 1; j < primeList.Length; j++) {
                    FCNode nextNode = primeList[j];
                    if(nextNode.isPromoted() != currNode.isPromoted()) {
                        currNode.setNextPointer(nextNode);
                        break;
                    }
                }
            }
        }

        private void setNodeMatrixFromCoordMatrix(CoordNode[][] coordNodeMatrix) {
            // Convert 2D array of CoordinateNodes into 2D array of
            // FractionalCascadingNodes. Set as nodeMatrix
            nodeMatrix = new FCNode[k][];
            Utils u = new Utils();
            for (int i = 0; i < k; i++) {
                nodeMatrix[i] = new FCNode[n];
                for(int j = 0; j < n; j++) {
                    CoordNode thisCoordNode = coordNodeMatrix[i][j];
                    FCNode thisFCNode =
                        new FCNode(thisCoordNode, (i + 1), j);                    
                    nodeMatrix[i][j] = thisFCNode;
                }
            }
        }

        private int getNumAugmentedNodes(double fraction, int prevPrimeListLen) {
            // Calculate the size of a prime list, or one  using
            // the current list of FractinoalCascadingNodes and the previous
            // promoted list of FractinoalCascadingNodes
            return Convert.ToInt32(Math.Ceiling(prevPrimeListLen * fraction));
        }

        private FCNode[] buildListPrime(FCNode[] FCNodeList1, FCNode[] FCNodeList2,
                                        int unitFracDen) {
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
            int numAugmentedNodes = getNumAugmentedNodes(fraction, FCNodeList2.Length);

            // Instantiate promoted list
            int primeListSize = n + numAugmentedNodes;       
            FCNode[] primeList = new FCNode[primeListSize];

            // Build list of elements from FCNodeList2 to augment with FCNodeList1
            FCNode[] nodesToPromote =
                    new FCNode[numAugmentedNodes];
            
            int c = 0; // index counter for nodesToPromote
            for(int i = 0; i < FCNodeList2.Length; i += unitFracDen) {
                // It is essential that nodes be copied from here because this is the only
                // state at which we can store the location in the previous list
                nodesToPromote[c++] = FCNodeList2[i].makeCopy(true, i);
            }
            
            // Combine elements from nodesToPromote and FCNodeList1 into primeList
            // Maintain order
            c = 0;      // index counter for Fractional_Cascading
            int d = 0;  // index counter for FCNodeList1
            int j = 0;  // index counter for primeList
            
            while(c < nodesToPromote.Length && d < FCNodeList1.Length) {
                if (FCNodeList1[d].getData() < nodesToPromote[c].getData()) {
                    primeList[j] = FCNodeList1[d++].makeCopy();
                } else primeList[j] = nodesToPromote[c++];
                j++;
            }

            // Add leftover values:
            while(c < nodesToPromote.Length) {
                primeList[j++] = nodesToPromote[c++];
            }
            while(d < n) {
                primeList[j++] = FCNodeList1[d++].makeCopy();
            }
            
            // Set elements new elements to prime, assign pointers
            // TODO- Make this more efficient
            foreach(FCNode f in primeList) f.setPrime();
            setPointers(primeList);
            
            return primeList;
        }

        private void setNodeMatrixPrime(int unitFracDen) {
            // re. k-1 size of nodeMatrixPrime d1, promoted lists are in terms of d and d-1
            // st. primes of every list total at k-1 lists
            nodeMatrixPrime = new FCNode[k-1][];
            
            // build list(k-1)' - only promoted list build using 2 non-promoted lists
            nodeMatrixPrime[k-2] = buildListPrime(nodeMatrix[k-2], nodeMatrix[k-1], unitFracDen);

            // build the remaining promoted lists by nodes from list(i - 1') into list(i)
            for(int i = k-3; i >= 0; i--) {
                FCNode[] nonAugmentedNodeListI = nodeMatrix[i];
                FCNode[] augmentedNodeListIPlusOne = nodeMatrixPrime[i + 1];

                nodeMatrixPrime[i] = buildListPrime(nonAugmentedNodeListI,
                                                    augmentedNodeListIPlusOne,
                                                    unitFracDen);
            }
        }

        private void setCoordMatrix() {
            // Build k lists of n CoordinateNodes each, sorted by their location (xLox)
            // Each list will have nodes which share data values but have various xLoc values
            // Set as inputCoordMatrix
            CoordinateNodeListGenerator cnlg = new CoordinateNodeListGenerator();
            CoordNode[][] coordNodeMatrix = new CoordNode[k][];
            for (int i = 0; i < k; i++) coordNodeMatrix[i] = cnlg.getCoordNodeList(n);
            inputCoordMatrix = coordNodeMatrix;
        }

        private void sortCordMatrixOnData(CoordNode[][] matrx) {
            // Sort each array in matrx by the data values of the coordinate nodes
            MergeSortNodes msn = new MergeSortNodes();
            foreach(CoordNode[] arr in matrx) msn.sort(arr, 0);
        }

        public FractionalCascadingMatrices(int numValsPerList, int numLists,
                                           int unitFracDen=2, bool print=true) {
            /**
            Parameters:
                numValsPerList: k
                numLists:       n
                unitFracDen:    Denominator of the unit fraction used to augment the lists into prime lists
                                -> unitFracDen=2 -> augment using every second value st. list i' contains all
                                                    of the values from list i and 1/2 if the values from
                                                    list i-1'
                print:          for demoing
            */
            n = numValsPerList;
            k = numLists;
            
            if(print) {
                Console.WriteLine("N: " + n);
                Console.WriteLine("K: " + k);
            }

            if(unitFracDen <= 1)
                throw new Exception("Invalid unitFracDen, must be greater than 1");
            
            // Convert coordNodeMatrix to one of FractionalCascadingNodes, assign as nodeMatrix
            // -> start with matrix of coordNodes sorted by their location (xLoc)
            setCoordMatrix();
            if(print) {
                Console.WriteLine("CoordinateMatrix - input");
                u.printNodeMatrix(inputCoordMatrix);
            }

            // Sort each array in coordinate matrix on its data value and set it
            sortCordMatrixOnData(inputCoordMatrix);
            setNodeMatrixFromCoordMatrix(inputCoordMatrix);

            if(print) {
                Console.WriteLine("NodeMatrix");
                u.printNodeMatrix(nodeMatrix);
            }

            // Build and assign nodeMatrixPrime from just-assigned nodeMatrix
            setNodeMatrixPrime(unitFracDen);

            if(print) {
                Console.WriteLine("NodeMatrixPrime");
                u.printNodeMatrix(nodeMatrixPrime);
            }
        }
    }
}