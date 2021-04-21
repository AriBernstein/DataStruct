using System;
using System.Collections.Generic; 

namespace Fractional_Cascading {
    public class FCNode : Node {        
        private CoordNode BaseCoordNode; // attr codes [0, 3]
        private int Index; // attr code 4
        private int Dimension; // between 1 and k inclusive, attr code 5
        private FCNode PreviousNode;
        private FCNode NextNode;
        private bool Prime; // Really just for printing      
        private bool Promoted = false;  // flag indicating node is has been promoted into
                                        // an augmented list from a previous dimensions,
                                        // used for pointer assignment

        private int PreviousAugmentedListIndex;
     
        public int GetAttr(int attrCode) {  // For accessing attributes from baseCoordNode
            if (0 <= attrCode && attrCode <= 4 ) return BaseCoordNode.GetAttr(attrCode);
            else if (attrCode == 4) return Index;
            else if (attrCode == 5) return Dimension;
            else throw new Exception("bad attribute code on FCNode");
        }

        public FCNode(CoordNode baseCordNode, int dimension, int index,
                      bool promoted=false, int previousAugmentedListIndex = -1) {
            BaseCoordNode = baseCordNode;
            Index = index;
            Dimension = dimension;
            Promoted = promoted;
            PreviousAugmentedListIndex = previousAugmentedListIndex;
        }
        
        public FCNode(CoordNode baseCoordNode, bool prime = false) {
            BaseCoordNode = baseCoordNode;
            Prime = prime;
        }

        public CoordNode GetCoordNode() {
            return BaseCoordNode;
        }

        public int GetData(){
            return BaseCoordNode.GetAttr(0);
        }

        public int GetDim() {
            return Dimension;
        }
        
        public void SetPrime() {
            Prime = true;
        }

        public bool IsPromoted() {
            return Promoted;
        }
        public int GetPreviouslyAugmentedIndex() {
            return PreviousAugmentedListIndex;
        }

        public int CoordNodeLoc(int dim=1) {
            if(dim < 1 || dim > 3) {
                string excStr = "Invalid dimension parameter when calling coordNodeLoc";
                throw new Exception(excStr);
                }
            return BaseCoordNode.GetAttr(dim);
        }

        public void SetPrevPointer(FCNode prev) {
            PreviousNode = prev;
        }
        public FCNode GetPrevPointer() {
            return PreviousNode;
        }

        public void SetNextPointer(FCNode next) {
            NextNode = next;
        }
        public FCNode GetNextPointer() {
            return NextNode;
        }

        public FCNode MakeCopy(bool setPromoted=false, bool keepPointers=false,
                               int prevAugmentedIndex = -1) {
            
            FCNode ret = new FCNode(BaseCoordNode, Dimension, Index,
                                    setPromoted, prevAugmentedIndex);
            if(keepPointers) {
                ret.NextNode = NextNode;
                ret.PreviousNode = PreviousNode;
            }

            return ret;
        }

        public override string ToString() {
            if (Prime == false) {    // If node is not in an augmented list
                return "[FC Node - dim: " + Dimension + ", index: " + Index +
                       ", " + BaseCoordNode + ']';
            } else if (PreviousNode == null && NextNode == null) {
                return "[FC Node - dim: " + Dimension + ", index: " + Index +
                       ", " + BaseCoordNode + ", next: empty, prev: empty]";
            } else if (PreviousNode == null && NextNode != null) {
                return "[FC Node - dim: " + Dimension + ", index: " + Index +
                       ", " + BaseCoordNode + ", next: " + NextNode.GetData() + 
                       ", prev: empty]";
            } else if (PreviousNode != null && NextNode == null) {
                return "[FC Node - dim: " + Dimension + ", index: " + Index +
                       ", " + BaseCoordNode + ", next: empty, prev: " + 
                       PreviousNode.GetData() + ']';

            } else return "[FC Node - dim: " + Dimension + ", index: " + Index + 
                          ", " + BaseCoordNode + ", next: " + NextNode.GetData() + 
                          ", prev: " + PreviousNode.GetData()+ ']';
        }
    }
}
