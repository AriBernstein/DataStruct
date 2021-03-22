using System;
using System.Collections.Generic;
using System.Text;

namespace DataStruct
{
    public class Graph<T, TGraphNode> where TGraphNode : GraphNode<T>, new()
    {
        LinkedList<GraphNode<T>> nodes;
        public Graph()
        {
            nodes = new LinkedList<GraphNode<T>>();
        }

        public Graph(T baseNodeData)
        {
            nodes = new LinkedList<GraphNode<T>>(new GraphNode<T>(baseNodeData));
        }

        public TGraphNode Add(T data)
        {
            TGraphNode newNode = new TGraphNode();
            newNode.SetData(data);
            nodes.Add(newNode);
            return newNode;
        }

        public void DirectionalLinkTo(GraphNode<T> source, GraphNode<T> target)
        {
            source.LinkTo(target);
        }

        public void BidirectionalLinkTo(GraphNode<T> nodeA, GraphNode<T> nodeB)
        {
            nodeA.LinkTo(nodeB);
            nodeB.LinkTo(nodeA);
        }

    }
}
