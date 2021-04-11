using System;
namespace Fractional_Cascading {
    public class DataNode : Node {
        private int data;

        public int getAttr(int attrCode) {
            if (attrCode != 0) {
                String errMsg = "attrCode must be 0 when calling getAttr on a dataNode." +
                    " Current attrCode value: " + attrCode;
                throw new Exception(errMsg);
            } else return data;
        }

        public int getData() {
            return data;
        }

        public DataNode(int dataVal) {
            data = dataVal;
        }

        public override string ToString() {
            return data.ToString();
        }
    }
}