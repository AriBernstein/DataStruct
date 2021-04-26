using System;
namespace Fractional_Cascading {
    public class CoordNode : Node {
        private DataNode Data; // attr code 0

        // attr codes 1, 2, 3
        private int xLoc = -1, yLoc = -1, zLoc = -1;

        public CoordNode(int dataVal, int xVal, int yVal, int zVal) {
            // 3-dimensional constructor
            xLoc = xVal;
            yLoc = yVal;
            zLoc = zVal;
            Data = new DataNode(dataVal);
        }

        public CoordNode(int dataVal, int xVal, int yVal) {
            // 2-dimensional constructor
            xLoc = xVal;
            yLoc = yVal;
            Data = new DataNode(dataVal);
        }

        public CoordNode(int dataVal, int xVal) {
            // 1-dimensional constructor
            xLoc = xVal;
            Data = new DataNode(dataVal);
        }

        public void SetData(int newData) {
            Data = new DataNode(newData);
        }

        public DataNode GetDataNode() {
            return Data;
        }

        public int GetAttr(int attrCode) {
            if      (attrCode == 0) return Data.GetAttr(attrCode);
            else if (attrCode == 1) return xLoc;
            else if (attrCode == 2) return yLoc;
            else if (attrCode == 3) return zLoc;
            else throw new Exception("bad attribute code on coordNode");
        }

        public void SetLoc(int dimension, int location) {
            if      (dimension == 1)    xLoc = location;
            else if (dimension == 2)    yLoc = location;
            else if (dimension == 3)    zLoc = location;
            else throw new Exception("invalid dimension parameter when " +
                                     "calling setLoc on coordNode");
        }

        public int GetDimensionality() {
            if      (zLoc != -1)    return 3;
            else if (yLoc != -1)    return 2;
            else if (xLoc != -1)    return 1;
            else throw new Exception("coordNode has no dimension values.");
        }

        public override string ToString() {
            int d = GetDimensionality();
            if (d == 1) return $"Data: {Data} (x: {xLoc})";
            if (d == 2) return $"Data: {Data}, (x: {xLoc}, y: {yLoc})";
            if (d == 3) return $"Data: {Data}, (x: {xLoc}, y: {yLoc}, z: {zLoc})";
            throw new Exception("Invalid dimensionality value on " + 
                                "coordinateNode at time of print.");
        }
    }
}
