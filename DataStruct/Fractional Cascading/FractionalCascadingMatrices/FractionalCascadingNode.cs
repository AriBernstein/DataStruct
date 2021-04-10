using System;
using System.Collections.Generic; 

namespace Fractional_Cascading {
    public class FCNode : Node {        
        private CoordNode baseCoordNode; // attr codes [0, 3]
        private int index; // attr code 4
        private int dimension; // k value of list where nodes are stored
                               // attr code 5
        private FCNode previousNode;
        private FCNode nextNode;
        private bool prime; // Really just for printing      
        private bool promoted = false;  // flag indicating node is being promoted into an
                                        // augmented list, used for pointer assignment

        private int previousAugmentedListIndx;

        // For accessing things attributes from baseCoordNode 
        private static HashSet<int> coordNodeAttrCodes = new HashSet<int>{0, 1, 2, 3};
        
        public int getAttr(int attrCode) {
            if (coordNodeAttrCodes.Contains(attrCode))
                return baseCoordNode.getAttr(attrCode);
            else if(attrCode == 4) return index;
            else if(attrCode == 5) return dimension;
            else throw new Exception("bad attribute code on FCNode");
        }

        public FCNode(CoordNode cordNode, int coordDimension, int coordIndex,
                      bool isPromoted=false, int prevAugListIndx = -1) {
            baseCoordNode = cordNode;
            index = coordIndex;
            dimension = coordDimension;
            promoted = isPromoted;
            previousAugmentedListIndx = prevAugListIndx;
        }
        
        public FCNode(CoordNode cordNode, bool isPrime = false) {
            baseCoordNode = cordNode;
            prime = isPrime;
        }

        public CoordNode getCoordNode() {
            return baseCoordNode;
        }

        public int getData(){
            return baseCoordNode.getAttr(0);
        }

        public int getDim() {
            return dimension;
        }
        
        public void setPrime() {
            prime = true;
        }

        public bool isPromoted() {
            return promoted;
        }
        public int getPreviouslyAugmentedIndex() {
            return previousAugmentedListIndx;
        }

        public int coordNodeLoc(int dim=1) {
            if(dim < 1 || dim > 3) {
                string excStr = "Invalid dimension parameter when calling coordNodeLoc";
                throw new Exception(excStr);
                }
            return baseCoordNode.getAttr(dim);
        }

        public void setPrevPointer(FCNode prev) {
            previousNode = prev;
        }
        public FCNode getPrevPointer() {
            return previousNode;
        }

        public void setNextPointer(FCNode next) {
            nextNode = next;
        }
        public FCNode getNextPointer() {
            return nextNode;
        }

        public FCNode makeCopy(bool setPromoted=false, int prevAugmentedIndex = -1) {
            FCNode ret = new FCNode(baseCoordNode, dimension, index,
                                    setPromoted, prevAugmentedIndex);
            ret.nextNode = nextNode;
            ret.previousNode = previousNode;
            return ret;
        }

        public override string ToString() {
            if(prime == false) {    // If node is not in an augmented list
                return "[FC Node - dim: " + dimension + ", index: " + index +
                       ", " + baseCoordNode + ']';
            } else if(previousNode == null && nextNode == null) {
                return "[FC Node - dim: " + dimension + ", index: " + index +
                       ", " + baseCoordNode + ", next: empty, prev: empty]";
            } else if(previousNode == null && nextNode != null) {
                return "[FC Node - dim: " + dimension + ", index: " + index +
                       ", " + baseCoordNode + ", next: " + nextNode.getData() + 
                       ", prev: empty]";
            } else if (previousNode != null && nextNode == null) {
                return "[FC Node - dim: " + dimension + ", index: " + index +
                       ", " + baseCoordNode + ", next: empty, prev: " + 
                       previousNode.getData() + ']';

            } else return "[FC Node - dim: " + dimension + ", index: " + index + 
                          ", " + baseCoordNode + ", next: " + nextNode.getData() + 
                          ", prev: " + previousNode.getData()+ ']';
        }
    }
}
