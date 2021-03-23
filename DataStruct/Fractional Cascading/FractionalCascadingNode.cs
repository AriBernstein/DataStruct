using System;
using System.Collections.Generic; 

namespace Fractional_Cascading {
    public class FractionalCascadingNode : Node {        
        private CoordinateNode data; // attr codes [0, 3]
        private int index; // attr code 4
        private int dimension; // k value of list where nodes are stored
                              // attr code 5
        private FractionalCascadingNode previousNode;
        private FractionalCascadingNode nextNode;
        private bool prime;
        private bool promoted = false;
        private int previousAugmentedListIndx;

        private static HashSet<int> coordNodeAttributesCodes = new HashSet<int>{0, 1, 2, 3};
        
        public int getAttr(int attrCode) {
            if (coordNodeAttributesCodes.Contains(attrCode)) return data.getAttr(attrCode);
            else if(attrCode == 4) return index;
            else if(attrCode == 5) return dimension;
            else throw new Exception("bad attribute code on FCNode");
        }

        public FractionalCascadingNode(CoordinateNode cordNode, int coordDimension,
                                       int coordIndex, bool isPrime=false,
                                       bool isPromoted=false, int prevAugListIndx = -1) {
            // Only relevant for fractional cascading example with lists
            data = cordNode;
            index = coordIndex;
            dimension = coordDimension;
            prime = isPrime;
            promoted = isPromoted;
            previousAugmentedListIndx = prevAugListIndx;
        }
        
        public FractionalCascadingNode(CoordinateNode cordNode, bool isPrime = false) {
            data = cordNode;
            prime = isPrime;
        }

        public int coordNodeData(){
            return data.getAttr(0);
        }

        public int getDimension() {
            return dimension;
        }

        public bool isPrime() {
            return prime;
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

        public int coordNodeLocation(int dim=1) {
            if(dim < 1 || dim > 3) {
                string excStr = "Invalid dimension parameter " +
                                "when calling coordNodeLocation";
                throw new Exception(excStr);
                }
            return data.getAttr(dim);
        }

        public void setPrevPointer(FractionalCascadingNode prev) {
            previousNode = prev;
        }
        public FractionalCascadingNode getPrevPointer() {
            return previousNode;
        }

        public void setNextPointer(FractionalCascadingNode next) {
            nextNode = next;
        }
        public FractionalCascadingNode getNextPointer() {
            return nextNode;
        }

        public FractionalCascadingNode makeCopy(bool setPromoted=false,
                                                int prevAugmentedIndex = -1) {
            FractionalCascadingNode ret = 
                new FractionalCascadingNode(data, dimension, index, prime,
                                            setPromoted, prevAugmentedIndex);
            ret.nextNode = nextNode;
            ret.previousNode = previousNode;
            return ret;
        }

        public override string ToString() {
            if(prime == false) {
                return "[FC Node - dim: " + dimension + ", index: " + index +
                ", " + data + ']';
            } else if(previousNode == null && nextNode == null) {
                return "[FC Node - dim: " + dimension + ", index: " + index +
                ", " + data + ", next: empty, prev: empty]";
            } else if(previousNode == null && nextNode != null) {
                return "[FC Node - dim: " + dimension + ", index: " + index +
                    ", " + data + ", next: " + nextNode.coordNodeData() + 
                    ", prev: empty]";
            } else if (previousNode != null && nextNode == null) {
                return "[FC Node - dim: " + dimension + ", index: " + index +
                    ", " + data + ", next: empty, prev: " + 
                    previousNode.coordNodeData() + ']';

            } else return  "[FC Node - dim: " + dimension + ", index: " + index + 
                ", " + data + ", next: " + nextNode.coordNodeData() + 
                ", prev: " + previousNode.coordNodeData()+ ']';
        }
    }
}
