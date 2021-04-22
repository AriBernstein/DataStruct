// using System;

// namespace Fractional_Cascading {
//     class RangeTree {
//         private int Dimensionality;
//         private RangeTreeLeaf[] xLeaves;    // leaves sorted on first dimension locations
//         private RangeTreeLeaf[] yLeaves;    // leaves sorted on second dimension locations
//         private RangeTreeLeaf[] zLeaves;    // leaves sorted on third dimension locations
//         private RangeTreeNode Root;
//         MergeSortNodes msn = new MergeSortNodes();

//         public RangeTreeNode GetRoot() {
//             return Root;
//         }

//         private RangeTreeNode buildTree(CoordNode[] coordNodes, int dimension) {
//             if (dimension < Dimensionality)  // build top-down
//                 buildTree(coordNodes, dimension + 1);
            
//             RangeTreeLeaf[] sortedNodes;
//             if      (dimension == 1) sortedNodes = xLeaves;
//             else if (dimension == 2) sortedNodes = yLeaves;
//             else if (dimension == 3) sortedNodes = zLeaves;
//             else throw new Exception("WATT?");

            
//             return new RangeTreeNode(1, Root,0,0);
//         }

//         public RangeTree(CoordNode[] coordNodes) {
//             Dimensionality = coordNodes[0].GetDimensionality();
//             if (Dimensionality < 1 || Dimensionality > 3)
//                 throw new Exception("coordNodes has invalid dimensionality ");

//             // Populate xLeaves, yLeaves, and/or zLeaves depending on dimensionality
//             for (int i = 0; i < Dimensionality; i++) {
//                 int currDim = i + 1;
//                 msn.Sort(coordNodes, currDim);
//                 RangeTreeLeaf[] currList = new RangeTreeLeaf[coordNodes.Length];
//                 for (int j = 0; j < currList.Length; j++)
//                     currList[j] = new RangeTreeLeaf(coordNodes[j].GetDataNode(), currDim,
//                                                     coordNodes[j].GetAttr(currDim));
//                 if      (currDim == 1) xLeaves = currList;
//                 else if (currDim == 2) yLeaves = currList;
//                 else if (currDim == 3) zLeaves = currList;
//                 else throw new Exception("WAT?");
//             }
//         }
//     }
// }