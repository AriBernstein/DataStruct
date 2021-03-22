using System;
using System.Collections.Generic;

namespace DataStruct
{
    public class LinkedList<T>
    {
        private LinkNode<T> baseNode = null;
        private LinkNode<T> tailNode = null;
        private int length = 0;

        public LinkedList()
        {

        }

        public LinkedList(T baseNodeData)
        {
            Add(baseNodeData);
        }

        public void Add(T data)
        {
            if (length == 0)
            {
                baseNode = new LinkNode<T>(data);
                tailNode = baseNode;
                length = 1;
            }
            else
            {
                LinkNode<T> newNode = new LinkNode<T>(data);
                newNode.PreviousNode = tailNode;
                tailNode.NextNode = newNode;
                tailNode = tailNode.NextNode;
            }
        }

        public void AddAsHead(T data)
        {
            if (length == 0)
            {
                baseNode = new LinkNode<T>(data);
                tailNode = baseNode;
                length = 1;
            }
            else
            {
                LinkNode<T> newNode = new LinkNode<T>(data);
                newNode.NextNode = baseNode;
                baseNode.PreviousNode = newNode;
                baseNode = newNode;
            }
        }

        public T Head
        {
            get { return baseNode.Data; }
        }

        public T Tail
        {
            get { return tailNode.Data; }
        }

        public T GetItemAt(int index)
        {
            if (index >= length || index < 0) { throw new IndexOutOfRangeException("The index {index} is out of the bounds of the LinkedList."); }

            // Case where index is closer to the head
            if (index < (length / 2))
            {
                LinkNode<T> currentNode = baseNode;
                for (int i = 0; i < length; i++) {
                    currentNode = currentNode.NextNode;
                }
                return currentNode.Data;
            }
            // Case where index is closer to the tail
            else
            {
                LinkNode<T> currentNode = tailNode;
                for (int i = length - 1; i >= 0; i--)
                {
                    currentNode = currentNode.PreviousNode;
                }
                return currentNode.Data;
            }
        }

        public int GetIndexByValue(T value)
        {
            LinkNode<T> currentNode = baseNode;
            int index = 0;

            while (currentNode != null)
            {
                if (EqualityComparer<T>.Default.Equals(currentNode.Data, value)) return index;
                index++;
            }
            return -1;
        }

        private class LinkNode<S> {
            public LinkNode(S content)
            {
                Data = content;
            }

            public LinkNode<S> NextNode { get; set; }

            public LinkNode<S> PreviousNode { get; set; }

            public S Data { get; set; }
        }
    }
}
