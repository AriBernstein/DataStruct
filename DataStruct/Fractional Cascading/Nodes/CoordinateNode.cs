using System;
namespace Fractional_Cascading {
    public class CoordNode : Node {
        private DataNode data; // attr code 0

        // attr codes 1, 2, 3
        private int xLoc = -1, yLoc = -1, zLoc = -1;

        public CoordNode(int dataVal, int xVal, int yVal, int zVal) {
            // 3-dimensional constructor
            xLoc = xVal;
            yLoc = yVal;
            zLoc = zVal;
            data = new DataNode(dataVal);
        }

        public CoordNode(int dataVal, int xVal, int yVal) {
            // 2-dimensional constructor
            xLoc = xVal;
            yLoc = yVal;
            data = new DataNode(dataVal);
        }

        public CoordNode(int dataVal, int xVal) {
            // 1-dimenstional constructor
            xLoc = xVal;
            data = new DataNode(dataVal);
        }

        public void setData(int newData) {
            data = new DataNode(newData);
        }

        public int getAttr(int attrCode) {
            if (attrCode == 0)      return data.getAttr(attrCode);
            else if (attrCode == 1) return xLoc;
            else if (attrCode == 2) return yLoc;
            else if (attrCode == 3) return zLoc;
            else throw new Exception("bad attribute code on coordNode");
        }

        public void setLoc(int dimension, int location) {
            if (dimension == 1)         xLoc = location;
            else if (dimension == 2)    yLoc = location;
            else if (dimension == 3)    zLoc = location;
            else throw new Exception("invalid dimension parameter when " +
                                     "calling setLoc on coordNode");
        }

        public int dimensionality() {
            if (zLoc != -1)         return 3;
            else if (yLoc != -1)    return 2;
            else if (xLoc != -1)    return 1;
            else throw new Exception("coordNode has no dimension values.");
        }

        public override string ToString() {
            int d = dimensionality();
            switch (d) {
                case 1:
                    return "Data: " + data + 
                           " (x: "  + xLoc + ")";

                case 2:
                    return "Data: " + data +
                           ", (x: " + xLoc + 
                           ", y: "  + yLoc + ")";

                case 3:
                    return "Data: " + data +
                           ", (x: " + xLoc + 
                           ", y: "  + yLoc + 
                           ", z: "  + zLoc + ")";

            } throw new Exception("Invalid dimensionality value on " + 
                                  "coordinateNode at time of print.");
        }
    }
}
