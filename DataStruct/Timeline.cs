using System;
using System.Collections.Generic;
using System.Text;

namespace DataStruct
{
    public class Timeline<T> where T : ITimelineable
    {
        Graph<T, TimelineGraphNode<T>> data;
        public Timeline(T baseData)
        {
            data = new Graph<T, TimelineGraphNode<T>>(baseData);
        }
    }
}
