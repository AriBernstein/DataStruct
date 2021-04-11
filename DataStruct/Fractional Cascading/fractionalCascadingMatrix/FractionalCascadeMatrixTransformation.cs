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
        // Matrix of k lists of FCNodes, each of size n
        private FCNode[][] nodeMatrix;
       
        // Matrix of k-1 lists of FCNodes, each except for
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
        
        private FCNode[] buildListPrime(FCNode[] FCNodeList1, FCNode[] FCNodeList2,
                                        int unitFracDen) {
            /**
            Combine all elements from fcNodeListOne and a fraction of the values from
            fcNodeListTwo to a new list of FCNode. 

            fcNodeListOne and fcNodeListTwo must be ordered according to their data 

            Note: the first time this is run (for the kth list),  FCNodeList1 = list(k-1)
                                                                  FCNodeList2 = list(k)
                  all other times this is run, FCNodeList1 = list(i-1)'
                                               FCNodeList2 = list(i)
            */
            int numPromotedNodes =
                    (int)(Math.Ceiling(FCNodeList2.Length / (double)unitFracDen));

            // Instantiate promoted list
            int primeListSize = n + numPromotedNodes;       
            FCNode[] primeList = new FCNode[primeListSize];

            // Build list of elements from FCNodeList2 to augment with FCNodeList1
            FCNode[] nodesToPromote = new FCNode[numPromotedNodes];
            
            int c = 0; // index counter for nodesToPromote
            int d = 0; // index counter for FCNodeList1
            int j = 0; // index counter for primeList

            for(int i = 0; i < FCNodeList2.Length; i += unitFracDen) {
                // It is essential that nodes be copied from here because this is the only
                // state at which we can easily both store their location in the previous
                // list and flag them as promoted.
                nodesToPromote[c++] =
                    FCNodeList2[i].makeCopy(setPromoted:true, prevAugmentedIndex:i);
            }
            
            // Perform Fractional Cascading transformation 
            // -> Merge elements from nodesToPromote and FCNodeList1 into primeList
            // -> For each node, point to the previous and next foreign nodes
            // --> ie. prev and next nodes not present in initial list of given node
            FCNode lastPromotedNode = null;    FCNode lastNotPromotedNode = null;

            void setPointers(FCNode currentNode) {  // Handle pointer assignment
                // Assign prev and next non-promoted node pointers
                if(currentNode.isPromoted()) {
                    lastPromotedNode = currentNode;
                    currentNode.setPrevPointer(lastNotPromotedNode);
                    if(lastNotPromotedNode != null)
                        if(lastNotPromotedNode.getNextPointer() == null)
                            lastNotPromotedNode.setNextPointer(currentNode);

                // Assign prev and next promoted node pointers
                } else {
                    lastNotPromotedNode = currentNode;
                    currentNode.setPrevPointer(lastPromotedNode);
                    if (lastPromotedNode != null)
                        if(lastPromotedNode.getNextPointer() == null)
                            lastPromotedNode.setNextPointer(currentNode);
                }

                currentNode.setPrime();
            }

            c = 0;
            while(c < nodesToPromote.Length && d < FCNodeList1.Length) {
                if (FCNodeList1[d].getData() < nodesToPromote[c].getData()) {
                    primeList[j] = FCNodeList1[d++].makeCopy();
                } else primeList[j] = nodesToPromote[c++];
                
                setPointers(primeList[j]);
                j++;
            }

            // Add leftover values:
            while(c < nodesToPromote.Length) {
                primeList[j] = nodesToPromote[c++];
                setPointers(primeList[j]);
                j++;
            }
            while(d < n) {
                primeList[j] = FCNodeList1[d++].makeCopy();
                setPointers(primeList[j]);
                j++;
            }

            return primeList;
        }

        private void setNodeMatrixPrime(int unitFracDen) {
            /**
            Parameter:
                unitFracDen: denominator of the unit fraction indicating the size of the
                             subset promoted list (d-1)' into list d'

            re. k-1 size of nodeMatrixPrime d1, promoted lists are in terms of d and d-1
            st primes of every list total at k-1 lists  */

            nodeMatrixPrime = new FCNode[k-1][];
            
            // build list(k-1)' - only promoted list build using 2 non-promoted lists
            nodeMatrixPrime[k-2] =
                buildListPrime(nodeMatrix[k-2], nodeMatrix[k-1], unitFracDen);

            // build the remaining promoted lists by nodes from list(i - 1') into list(i)
            for(int i = k-3; i >= 0; i--) {
                FCNode[] nonAugmentedNodeListI = nodeMatrix[i];
                FCNode[] augmentedNodeListIPlusOne = nodeMatrixPrime[i + 1];
                nodeMatrixPrime[i] = buildListPrime(nonAugmentedNodeListI,
                                                    augmentedNodeListIPlusOne,
                                                    unitFracDen);
            }
        }

        private void setNodeMatrixFromCoordMatrix(CoordNode[][] coordNodeMatrix) {
            // Convert 2D array of CoordNodes into 2D array of FCNodes, set as nodeMatrix
            nodeMatrix = new FCNode[k][];
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

        private void setCoordMatrix(int insertData) {
            /**
            Build k lists of n CoordinateNodes each, sorted by their data value. If 
            insertData equals -1, each list will have nodes which share data values but
            have various xLoc values. Otherwise, each list will have nodes with random
            xLoc and data values, except for insertData, which will be present in each
            list. Set as inputCoordMatrix */

            CoordinateNodeListGenerator cnlg = new CoordinateNodeListGenerator();
            inputCoordMatrix = new CoordNode[k][];
            for (int i = 0; i < k; i++) {
                inputCoordMatrix[i] = cnlg.getCoordNodeList(n, insertData);
            }
        }

        public FractionalCascadingMatrices(int numValsPerList, int numLists,
                                           int unitFracDen=2, int insertData=-1,
                                           bool print=true) {
            /**
            Parameters:
                numValsPerList: k
                numLists:       n
                insertData:     data value for which to search inserted into each list in
                                coordMatrix and nodeMatrix
                unitFracDen:    denominator of the unit fraction indicating the size of
                                the subset of list (d-1)' to be promoted into list d'   */
                
            n = numValsPerList;
            k = numLists;
            
            if(print) Console.WriteLine("N: " + n + "\tK: " + k);

            if(unitFracDen <= 1)
                throw new Exception("Invalid unitFracDen, must be greater than 1");
            
            // Convert coordNodeMatrix to one of FCNodes, assign as nodeMatrix
            // -> start with matrix of coordNodes sorted by their location (xLoc)
            if(print) Console.WriteLine("Generating matrix of random sorted coordNodes.");
            setCoordMatrix(insertData);
            
            // TODO: combine setCoordMatrix & setNodeMatrixFromCoordMatrix functionality
            if(print) Console.WriteLine("Building FCNode matrix from coord matrix.");
            setNodeMatrixFromCoordMatrix(inputCoordMatrix);

            // Build and assign nodeMatrixPrime from just-assigned nodeMatrix
            if(print) Console.WriteLine("Performing Fractional Cascading transformation" +
                                        " on FCNode matrix.");
            setNodeMatrixPrime(unitFracDen);
        }
    }
}