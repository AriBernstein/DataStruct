using System;
using System.Collections.Generic;
using System.Text;

namespace DataStruct
{
    public class GraphNode<T>
    {
        LinkedList<GraphNode<T>> connectedNodes = new LinkedList<GraphNode<T>>();
        T data;

        public GraphNode()
        {

        }

        public GraphNode(T data)
        {
            this.SetData(data);
        }

        public void SetData(T data)
        {
            this.data = data;
        }

        public void LinkTo(GraphNode<T> target)
        {
            this.connectedNodes.Add(target);
        }
    }
}
