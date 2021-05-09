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
    public class FractionalCascadingMatrix {
        private CoordNode[][] InputCoordMatrix;
       
        // Matrix of k lists of FCNodes, post FC transformation
        // -> each list except for l(k) has nodes promoted from previous dimensions
        private FCNode[][] NodeMatrixPrime;
        private int n; // number of elements in each list in nodeMatrix
        private int k; // number of lists

        public CoordNode[][] GetInputCoordMatrix() {
            return InputCoordMatrix;
        }

        public FCNode[][] GetFCNodeMatrixPrime() {
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

            // Nested function to handle pointer assignment
            FCNode lastPromotedNode = null;    FCNode lastNotPromotedNode = null;
            void setPointers(FCNode currentNode) {
                // Assign prev and next pointers to closest non-promoted nodes
                if (currentNode.IsPromoted()) {
                    lastPromotedNode = currentNode;
                    currentNode.SetPrevPointer(lastNotPromotedNode);
                    if (lastNotPromotedNode != null)
                        if (lastNotPromotedNode.GetNextPointer() == null)
                            lastNotPromotedNode.SetNextPointer(currentNode);

                } else {    // Assign prev and next pointers to closest promoted nodes
                    lastNotPromotedNode = currentNode;
                    currentNode.SetPrevPointer(lastPromotedNode);
                    if (lastPromotedNode != null)
                        if (lastPromotedNode.GetNextPointer() == null)
                            lastPromotedNode.SetNextPointer(currentNode);
                }

                currentNode.SetPrime();
            }

            // Instantiate promoted list
            int numPromotedNodes =
                    (int)(Math.Ceiling(FCNodeList2.Length / (double)unitFracDen));

            int FCNodeListPrimeSize = n + numPromotedNodes;       
            FCNode[] FCNodeListPrime = new FCNode[FCNodeListPrimeSize];

            // Populate w/ every 2nd elem in FCNodeList2 to be merged into FCNodeListPrime
            FCNode[] nodesToPromote = new FCNode[numPromotedNodes];
            
            int c = 0; // index counter for nodesToPromote
            int d = 0; // index counter for FCNodeList1
            int j = 0; // index counter for FCNodeListPrime

            for (int i = 0; i < FCNodeList2.Length; i += unitFracDen) {
                // It is essential that nodes be copied from here because this is the only
                // state at which we can easily both store their locations in their
                // previous lists and flag them as promoted.
                nodesToPromote[c++] =
                    FCNodeList2[i].MakeCopy(setPromoted:true, prevAugmentedIndex:i);
            }
            
            // Perform Fractional Cascading transformation 
            // -> Merge elements from nodesToPromote and FCNodeList1 into FCNodeListPrime
            // -> For each node, point to the previous and next foreign nodes
            // --> ie. prev and next nodes not present in initial list of given nod
            c = 0;
            while(c < nodesToPromote.Length && d < FCNodeList1.Length) {
                if (FCNodeList1[d].GetData() < nodesToPromote[c].GetData()) {
                    FCNodeListPrime[j] = FCNodeList1[d++].MakeCopy();
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
                FCNodeListPrime[j] = FCNodeList1[d++].MakeCopy();
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
            if (print) Console.WriteLine(
                "Instantiating (not-yet-promoted) FCNodes from coordNode data.");
            for (int i = 0; i < k; i++) {
                NodeMatrixPrime[i] = new FCNode[n];
                for (int j = 0; j < n; j++) {
                    CoordNode thisCoordNode = InputCoordMatrix[i][j];
                    FCNode thisFCNode = new FCNode(thisCoordNode, (i + 1), j);                    
                    NodeMatrixPrime[i][j] = thisFCNode;
                }
            }
            
            // Begin Fractional Cascading Transformation
            if (print) Console.WriteLine(
                "Performing Fractional Cascading transformation on FCNode matrix.");

            // Perform transformation on all lists in reverse order
            // Re. i = (k-2): as nodes are always promoted from higher dimensions into
            //                lower ones, for (highest dim) k, list(k) = list(k')
            for (int i = k-2; i >= 0; i--)
                NodeMatrixPrime[i] = BuildListPrime(NodeMatrixPrime[i],
                                                    NodeMatrixPrime[i + 1],
                                                    unitFracDen);

        }

        private void SetCoordMatrix(int insertData) {
            /**
            Build k lists of n CoordinateNodes each, sorted by their data value. Each list 
            will have nodes with random xLoc and data values, except for insertData, which
            will be present in each list. Set as inputCoordMatrix */
            InputCoordMatrix = new CoordNode[k][];
            
            for (int i = 0; i < k; i++)
                InputCoordMatrix[i] = NodeGenerator.GetCoordNodeList(n, insertData,
                                                                     dataRangeMin:0,
                                                                     dataRangeMax:n*k);
        }

        public FractionalCascadingMatrix(int numValsPerList, int numLists,
                                         int insertData, int unitFracDen=2,
                                         bool print=true, bool randNodeAttrOrders=true) {
            /**
            Parameters:
                numValsPerList: k
                numLists:       n
                insertData:     data value for which to search inserted into each list in
                                coordMatrix and nodeMatrix
                unitFracDen:    denominator of the unit fraction indicating the size of
                                the subset of list (d-1)' to be promoted into list d'
                randNodeAttrOrders:
                                (first read documentation in of method RandUniqueIntsRange
                                in CoordinateNodeListGenerator.cs).
                                Random order of nodes' data attribute is irrelevant as
                                they are later sorted on it. The order of their xLoc
                                attribute is relevant if demonstrating the correctness of
                                the search but irrelevant if testing the speed. */
            n = numValsPerList;
            k = numLists;
            
            if (print) Console.WriteLine($"N: {n}\tK: {k}\tX: {insertData}");

            if (unitFracDen <= 1)
                throw new Exception("Invalid unitFracDen, must be greater than 1");
            
            // Convert coordNodeMatrix to one of FCNodes, assign as nodeMatrix
            // -> start with matrix of coordNodes sorted by their location (xLoc)
            if (print) Console.WriteLine(
                $"Generating matrix of {k} sorted lists of {n} randomly generated " +
                $"coordNodes (totalling {new Utils().PrettyNumApprox(k*n)} nodes).");
            SetCoordMatrix(insertData);

            // Build and assign nodeMatrixPrime from just-assigned nodeMatrix
            SetFCTransformationMatrix(unitFracDen, print:print);
        }
    }
}
