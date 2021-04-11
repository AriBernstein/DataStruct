using System;
using System.Collections.Generic;

namespace Fractional_Cascading {

    class RangeTreeLeafNode {
        private DataNode data;
        private RangeTreeBranchNode parent;
    }

    class RangeTreeBranchNode {
        /** 
        Branch -> Non-root node */



    }

    class RangeTree {
        private int dimension;
        private int x;
        public RangeTree(int dimensionalityVal, int xVal) {
            dimension = dimensionalityVal;
            x = xVal; 
        }
    }
}