using System;
namespace Fractional_Cascading {
    public class DataNode : Node {
        private int data; // attr code 0

        public int getAttr(int attrCode) {
            if (attrCode == 0) return data;
            else throw new Exception("getAttr != 0 when called on DataNode");
        }
        
        public DataNode(int dataVal) {
            data = dataVal;
        }

        public override string ToString() {
            return data.ToString();
        }
    }
}
