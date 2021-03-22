using System;
using System.Collections.Generic;
using System.Text;

namespace DataStruct
{
    class TimelineGraphNode<T> : GraphNode<T>
    {
        public TimelineGraphNode<T> ParentNode { get; set; }
        public TimelineGraphNode()
        {

        }
    }
}
