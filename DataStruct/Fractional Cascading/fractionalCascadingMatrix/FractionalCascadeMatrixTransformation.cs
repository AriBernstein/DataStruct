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
        private CoordNode[][] InputCoordMatrix;
       
        // Matrix of k lists of FCNodes, post FC transformation
        // -> each list except for l(k) has nodes promoted from previous dimensions
        private FCNode[][] NodeMatrixPrime;
        private int n; // number of elements in each list in nodeMatrix
        private int k; // number of lists

        public CoordNode[][] GetInputCoordMatrix() {
            return InputCoordMatrix;
        }
        public FCNode[][] GetFCNodeMatixPrime() {
            return NodeMatrixPrime;
        }
        
        private FCNode[] BuildListPrime(FCNode[] FCNodeList1, FCNode[] FCNodeList2,
                                        int unitFracDen) {
            /**
            Combine all elements from fcNodeListOne and a fraction of the values from
            fcNodeListTwo to a new list of FCNode. 

            fcNodeListOne and fcNodeListTwo must be ordered according to their data 

            Note: FCNodeList1 = list(i-1)'
                  FCNodeList2 = list(i)    */

            int numPromotedNodes =
                    (int)(Math.Ceiling(FCNodeList2.Length / (double)unitFracDen));

            // Instantiate promoted list
            int FCNodeListPrimeSize = n + numPromotedNodes;       
            FCNode[] FCNodeListPrime = new FCNode[FCNodeListPrimeSize];

            // Populate w/ every 2nd elem in FCNodeList2 to be merged into FCNodeListPrime
            FCNode[] nodesToPromote = new FCNode[numPromotedNodes];
            
            int c = 0; // index counter for nodesToPromote
            int d = 0; // index counter for FCNodeList1
            int j = 0; // index counter for FCNodeListPrime

            for(int i = 0; i < FCNodeList2.Length; i += unitFracDen) {
                // It is essential that nodes be copied from here because this is the only
                // state at which we can easily both store their locations in their
                // previous lists and flag them as promoted.
                nodesToPromote[c++] =
                    FCNodeList2[i].makeCopy(setPromoted:true, prevAugmentedIndex:i);
            }
            
            // Perform Fractional Cascading transformation 
            // -> Merge elements from nodesToPromote and FCNodeList1 into FCNodeListPrime
            // -> For each node, point to the previous and next foreign nodes
            // --> ie. prev and next nodes not present in initial list of given node
            FCNode lastPromotedNode = null;    FCNode lastNotPromotedNode = null;

            void setPointers(FCNode currentNode) {  // Handle pointer assignment
                // Assign prev and next pointers to closest non-promoted nodes
                if(currentNode.isPromoted()) {
                    lastPromotedNode = currentNode;
                    currentNode.setPrevPointer(lastNotPromotedNode);
                    if(lastNotPromotedNode != null)
                        if(lastNotPromotedNode.getNextPointer() == null)
                            lastNotPromotedNode.setNextPointer(currentNode);

                // Assign prev and next pointers to closest promoted nodes
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
                if (FCNodeList1[d].GetData() < nodesToPromote[c].GetData()) {
                    FCNodeListPrime[j] = FCNodeList1[d++].makeCopy();
                } else FCNodeListPrime[j] = nodesToPromote[c++];
                
                setPointers(FCNodeListPrime[j]);
                j++;
            }

            // Add leftover values to augmented list:
            while(c < nodesToPromote.Length) {
                FCNodeListPrime[j] = nodesToPromote[c++];
                setPointers(FCNodeListPrime[j]);
                j++;
            }
            while(d < n) {
                FCNodeListPrime[j] = FCNodeList1[d++].makeCopy();
                setPointers(FCNodeListPrime[j]);
                j++;
            }

            return FCNodeListPrime;
        }

        private void SetFCTransformationMatrix(int unitFracDen, bool print=false) {
            /**
            Copy coordNode matrix into FCNode matrix, perform fractional cascading
            transformation

            Parameter:
                unitFracDen: denominator of the unit fraction indicating the size of the
                             subset promoted list (d-1)' into list d'   */

            NodeMatrixPrime = new FCNode[k][];
            
            // Convert coordNodes -> FCNodes
            if(print) Console.WriteLine("Copying coordNodes into (non-promoted) FCNodes");
            for (int i = 0; i < k; i++) {
                NodeMatrixPrime[i] = new FCNode[n];
                for(int j = 0; j < n; j++) {
                    CoordNode thisCoordNode = InputCoordMatrix[i][j];
                    FCNode thisFCNode = new FCNode(thisCoordNode, (i + 1), j);                    
                    NodeMatrixPrime[i][j] = thisFCNode;
                }
            }
            
            // Begin Fractional Cascading Transformation
            if(print) Console.WriteLine("Performing Fractional Cascading transformation" +
                                        " on FCNode matrix.");

            // re. (k-2), nodes are always promoted from lower dimensions
            // -> for (highest dim) k, list(k') = list(k)
            // -> ( list(k) is at index (k-1) )
            // build the remaining promoted lists by nodes from list(i - 1') into list(i)
            for(int i = k-2; i >= 0; i--) {
                FCNode[] augmentedNodeListI = NodeMatrixPrime[i];
                FCNode[] augmentedNodeListIPlusOne = NodeMatrixPrime[i + 1];
                NodeMatrixPrime[i] = BuildListPrime(augmentedNodeListI,
                                                    augmentedNodeListIPlusOne,
                                                    unitFracDen);
            }
        }

        private void SetCoordMatrix(int insertData) {
            /**
            Build k lists of n CoordinateNodes each, sorted by their data value. Each list 
            will have nodes with random xLoc and data values, except for insertData, which
            will be present in each list. Set as inputCoordMatrix */

            CoordinateNodeListGenerator cnlg = new CoordinateNodeListGenerator();
            InputCoordMatrix = new CoordNode[k][];
            
            for (int i = 0; i < k; i++)
                InputCoordMatrix[i] = cnlg.GetCoordNodeList(n, insertData);
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
            SetCoordMatrix(insertData);

            // Build and assign nodeMatrixPrime from just-assigned nodeMatrix
            SetFCTransformationMatrix(unitFracDen, print:print);
        }
    }
}