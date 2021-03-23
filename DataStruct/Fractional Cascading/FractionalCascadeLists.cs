using System;

namespace DataStruct 
{
    /**
    Fractional Cascading implementation of search for location of values in
    k dimensions with complexity of O(log(n) + k)
    */
    public class FractionalCascadingLists {
        Utils u = new Utils();
        public FractionalCascadingNode[][] nodeMatrix;
        public FractionalCascadingNode[][] nodeMatrixPrime;
        int n; // number of elements in each list
        int k; // number of lists

        private int getPrimeListSize(double fraction) {
            return Convert.ToInt32(Math.Ceiling((1 + fraction) * n));
        }

        private void setPointers(FractionalCascadingNode[] fcNodeListAugmented) {
            // Handle previous node pointers
            for(int i = 1; i < fcNodeListAugmented.Length; i++) {
                FractionalCascadingNode currNode = fcNodeListAugmented[i];
                for(int j = i-1; j >= 0; j--) {
                    FractionalCascadingNode prevNode = fcNodeListAugmented[j];
                    if(prevNode.dimension != currNode.dimension) {
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
                    if(nextNode.dimension != currNode.dimension) {
                        currNode.setNextPointer(nextNode);
                        break;
                    }
                }
            }
        }

        private FractionalCascadingNode[] buildListPrime(FractionalCascadingNode[] FCNodeList1,
                                                         FractionalCascadingNode[] FCNodeList2,
                                                         int unitFracDen) {
            /**
            Combine all elements from fcNodeListOne and a fraction of the values from fcNodeListTwo
            to a new list of FractionalCascadingNodes. 

            fcNodeListOne and fcNodeListTwo must be ordered according to their data 

            Note: the first time this is run (for the kth list), 
            */

            double fraction = 1 / (double)unitFracDen;
            
            // Instantiate augmented list
            int primeListSize = getPrimeListSize(fraction);
            
            FractionalCascadingNode[] fcNodeListAugmented = new FractionalCascadingNode[primeListSize];

            // Build list of elements from FCNodeList2 to augment with FCNodeList1
            FractionalCascadingNode[] fcNodeList2AugmentedNodes =
                    new FractionalCascadingNode[primeListSize - n];
            
            int c = 0; // index counter for fcNodeList2AugmentedNodes
            for(int i = 0; i < n; i += unitFracDen) {
                fcNodeList2AugmentedNodes[c++] = FCNodeList2[i].makeCopy();
            }
            
            // Combine elements from fcNodeList2AugmentedNodes and FCNodeList1 into fcNodeListAugmented
            c = 0; // index counter for fcNodeList2AugmentedNodes
            int d = 0; // index counter for FCNodeList1
            int j = 0; // index counter for fcNodeListAugmented
            
            while(c < fcNodeList2AugmentedNodes.Length && d < n) {
                if (FCNodeList1[d].coordNodeLocation() < fcNodeList2AugmentedNodes[c].coordNodeLocation()) {
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
            
            // TODO- Make this more efficient
            foreach(FractionalCascadingNode f in fcNodeListAugmented) f.prime = true;
            setPointers(fcNodeListAugmented);
            
            return fcNodeListAugmented;
        }


        private void setFCMatrixFromCordMatrix(CoordinateNode[][] coordNodeMatrix) {
            // Set nodeMatrix value
            // Convert 2D array of CoordinateNodes into 2D array of FractionalCascadingNodes
            nodeMatrix = new FractionalCascadingNode[k][];
            Utils u = new Utils();
            for (int i = 0; i < k; i++) {    // Populate this matrix
                nodeMatrix[i] = new FractionalCascadingNode[n];

                for(int j = 0; j < n; j++) {
                    CoordinateNode thisCoordNode = coordNodeMatrix[i][j];
                    FractionalCascadingNode thisFCNode =
                        new FractionalCascadingNode(thisCoordNode, (i + 1), j);                    
                    nodeMatrix[i][j] = thisFCNode;
                }
            }
        }

        private void setFCPrimeMatrix(int unitFracDen) {
            // re. k-1, augmented lists are in terms of d and d-1
            // st. primes of every list total at k-1 lists
            nodeMatrixPrime = new FractionalCascadingNode[k-1][];

            for(int i = 0; i < k-1; i++) {  // augmenting dim2FCNodelist into dim1FCNodelist
                FractionalCascadingNode[] dim1FCNodelist = nodeMatrix[i];
                FractionalCascadingNode[] dim2FCNodelist = nodeMatrix[i + 1];
                nodeMatrixPrime[i] = buildListPrime(dim1FCNodelist, dim2FCNodelist, unitFracDen);
            }
        }

        private CoordinateNode[][] getCoordMatrix() {
            // Build k lists of n CoordinateNodes each
            // Each list will have nodes which share data values but have various xLoc values
            CoordinateNodeListGenerator cnlg = new CoordinateNodeListGenerator();
            CoordinateNode[][] coordNodeMatrix = new CoordinateNode[k][];
            for (int i = 0; i < k; i++) coordNodeMatrix[i] = cnlg.getCoordinateNodeList(n);
            return coordNodeMatrix;
        }

        
        public FractionalCascadingLists(int numValsPerList, int numLists, int unitFracDen=2) {
            n = numValsPerList;
            k = numLists;

            if(unitFracDen <= 1) throw new Exception("Invalid unitFracDen, must be greater than 1");
            
            // Convert coordNodeMatrix to one of FractionalCascadingNodes, assign as nodeMatrix
            setFCMatrixFromCordMatrix(getCoordMatrix());
            Console.WriteLine("NodeMatrix");
            u.printFCNodeMatrix(nodeMatrix);

            // Build and assign nodeMatrixPrime from just-assigned nodeMatrix
            setFCPrimeMatrix(unitFracDen);
            Console.WriteLine("NodeMatrixPrime");
            u.printFCNodeMatrix(nodeMatrixPrime);
            Console.WriteLine("N: " + n);
            Console.WriteLine("N': " + Convert.ToInt32(Math.Ceiling((1 + (1 / (double)unitFracDen)) * n)));
            Console.WriteLine("K: " + k);
            Console.WriteLine("\n\n\nSearch for index of 9506");
        }
    }
}