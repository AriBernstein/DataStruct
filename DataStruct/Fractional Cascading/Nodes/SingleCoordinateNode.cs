using System;

namespace Fractional_Cascading {
    public class SingleCoordNode : Node {
        private DataNode Data;
        private int Location;
        public int GetAttr(int attrCode) {
            if (attrCode == 0) return Data.GetAttr(0);
            else if (attrCode == 1) return Location;
            else throw new Exception("getAttr != 0 or 1 when called on OneDimNode");
        }
        public int GetData() {
            return Data.GetAttr(0);
        }
        
        public int GetLocation() {
            return Location;
        }

        public SingleCoordNode(DataNode data, int location) {
            Data = data;
            Location = location;
        }

        public override string ToString() {
            return $"(Data: {Data}, Location: {Location})";
        }
    }
}
