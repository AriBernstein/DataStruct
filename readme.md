C# Implementations of Fractional Cascading Matrix and Range Tree Data Structures


Note re. Range Tree, had some issues with following test cases:

// List<CoordNode> newNodes = new List<CoordNode>();
// newNodes.Add(new CoordNode(56, 57, 78));
// newNodes.Add(new CoordNode(41, 40, 32));
// newNodes.Add(new CoordNode(70, 13, 12));
// newNodes.Add(new CoordNode(28, 86, 47));
// newNodes.Add(new CoordNode(76, 55, 84));
// newNodes.Add(new CoordNode(87, 33, 72));
// newNodes.Add(new CoordNode(49, 63, 42));
// newNodes.Add(new CoordNode(10, 20, 65));
// newNodes.Add(new CoordNode(50, 45, 60));
// newNodes.Add(new CoordNode(74, 29, 15));
// CoordNode[] nodes = newNodes.ToArray();


// List<CoordNode> newNodes = new List<CoordNode>();
// newNodes.Add(new CoordNode(56, 47, 12));
// newNodes.Add(new CoordNode(41, 30, 61));
// newNodes.Add(new CoordNode(70, 32, 21));
// newNodes.Add(new CoordNode(28, 54, 62));  //
// newNodes.Add(new CoordNode(76, 62, 11));
// newNodes.Add(new CoordNode(87, 45, 38));
// newNodes.Add(new CoordNode(49, 13, 51));
// newNodes.Add(new CoordNode(10, 36, 43));
// newNodes.Add(new CoordNode(50, 11, 81));
// newNodes.Add(new CoordNode(74, 66, 39));
// CoordNode[] nodes = newNodes.ToArray();

// List<CoordNode> newNodes = new List<CoordNode>();
// newNodes.Add(new CoordNode(56, 10, 79));
// newNodes.Add(new CoordNode(41, 30, 39));
// newNodes.Add(new CoordNode(70, 50, 70)); //
// newNodes.Add(new CoordNode(28, 51, 62)); //
// newNodes.Add(new CoordNode(76, 45, 83));
// newNodes.Add(new CoordNode(87, 52, 63)); //
// newNodes.Add(new CoordNode(49, 55, 18));
// newNodes.Add(new CoordNode(10, 61, 45));
// newNodes.Add(new CoordNode(50, 70, 67)); // 
// newNodes.Add(new CoordNode(74, 83, 27));
// CoordNode[] nodes = newNodes.ToArray();

// List<CoordNode> newNodes = new List<CoordNode>();
// newNodes.Add(new CoordNode(1, 56, 61)); //
// newNodes.Add(new CoordNode(2, 41, 22));
// newNodes.Add(new CoordNode(3, 70, 50)); //
// newNodes.Add(new CoordNode(4, 28, 31));
// newNodes.Add(new CoordNode(5, 69, 38));
// newNodes.Add(new CoordNode(6, 87, 44));
// newNodes.Add(new CoordNode(7, 49, 53));
// newNodes.Add(new CoordNode(8, 10, 54));
// newNodes.Add(new CoordNode(9, 50, 55)); //
// newNodes.Add(new CoordNode(10, 74, 71));
// CoordNode[] nodes = newNodes.ToArray();