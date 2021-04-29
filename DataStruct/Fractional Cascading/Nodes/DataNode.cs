using System;
namespace Fractional_Cascading {
    public class DataNode : Node {
        private int Data; // attr code 0

        public int GetAttr(int attrCode) {
            if (attrCode == 0) return Data;
            else throw new Exception("getAttr does not equal 0 when called on DataNode");
        }
        
        public DataNode(int data) {
            Data = data;
        }

        public override string ToString() {
            return Data.ToString();
        }
    }
}
