using System;
using System.Collections.Generic;
using System.Text;

namespace DataStruct
{
    public interface IGraphNode<T>
    {
        public IGraphNode<T> CreateGraphNode(T data);
    }
}
