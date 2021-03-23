using System;
using System.Collections.Generic;
using System.Text;

namespace DataStruct
{
    public class Timeline<T> //where T : ITimelineable
    {
        Graph<T, TimelineGraphNode<T>> data;
        public Timeline(T baseData)
        {
            data = new Graph<T, TimelineGraphNode<T>>(baseData);
        }

        public T Get(string version)
        {
            return data.BaseNode.Data;
        }

        public int AddVersion(int baseVersion, T value)
        {
            TimelineGraphNode<T> baseNode = (TimelineGraphNode<T>)data.Nodes.GetItemAt(baseVersion);
            TimelineGraphNode<T> newNode = data.Add(value);
            data.DirectionalLinkTo(baseNode, newNode);
            // Set reference to the node that this node is based on
            newNode.ParentNode = baseNode;
            return 1; // TODO: Change this to return the correct number
        }

        public void MergeVersions(int baseVersion1, int baseVersion2, T value)
        {
            
        }
    }
}
