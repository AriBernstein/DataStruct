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
        public bool prime;

        private static HashSet<int> coordNodeAttributesCodes = new HashSet<int>{0, 1, 2, 3};
        
        public int getAttr(int attrCode) {
            if (coordNodeAttributesCodes.Contains(attrCode)) return data.getAttr(attrCode);
            else if(attrCode == 4) return index;
            else if(attrCode == 5) return dimension;
            else throw new Exception("bad attribute code on FCNode");
        }

        public FractionalCascadingNode(CoordinateNode cordNode, int coordDimension,
                                       int coordIndex, bool isPrime = false) {
            // Only relevant for fractional cascading example with lists
            data = cordNode;
            index = coordIndex;
            dimension = coordDimension;
            prime = isPrime;
        }
        public FractionalCascadingNode(CoordinateNode cordNode, bool isPrime = false) {
            data = cordNode;
            prime = isPrime;
        }

        public int coordNodeData(){
            return data.getAttr(0);
        }

        public  int getDimension() {
            return dimension;
        }

        public int coordNodeLocation(int dim=1) {
            if(dim < 1 || dim > 3) throw new Exception("Invalid dimension parameter " +
                                                       "when calling coordNodeLocation");
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

        public FractionalCascadingNode makeCopy() {
            FractionalCascadingNode ret = 
                new FractionalCascadingNode(data, dimension, index, prime);
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
