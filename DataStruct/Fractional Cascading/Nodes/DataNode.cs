using System;
namespace Fractional_Cascading {
    public class DataNode : Node {
        private int Data; // attr code 0

        public int GetAttr(int attrCode) {
            if (attrCode == 0) return Data;
            else throw new Exception("getAttr != 0 when called on DataNode");
        }
        
        public DataNode(int dataVal) {
            Data = dataVal;
        }

        public override string ToString() {
            return Data.ToString();
        }
    }
}
