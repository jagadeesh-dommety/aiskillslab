using System.Collections.Generic;
using System.Text;

namespace ReverseLinkedList
{
    public class Node<T>
    {
        public T Value { get; set; }
        public Node<T>? Next { get; set; }
        
        public Node(T value)
        {
            Value = value;
        }

        public override string ToString() => Value?.ToString() ?? string.Empty;
    }

    public class ReversibleLinkedList<T>
    {
        public int maxSize = 100;
        private Node<T>? _head;

        public int Count { get; private set; }

        public bool IsEmpty => _head is null;

        public Node<T>? Head => _head;

        public void AddLast(T value)
        {
            if (Count >= maxSize)
            {
                _head = _head?.Next;
                Count--;
            }

            var node = new Node<T>(value);
            if (_head is null)
            {
                _head = node;
            }
            else
            {
                var current = _head;
                while (current.Next is not null)
                {
                    current = current.Next;
                }
                current.Next = node;
            }
            Count++;
        }

        public void AddFirst(T value)
        {
            var node = new Node<T>(value)
            {
                Next = _head
            };
            _head = node;
            Count++;
        }

        public void Reverse()
        {
            Node<T>? previous = null;
            var current = _head;

            while (current is not null)
            {
                var next = current.Next;
                current.Next = previous;
                previous = current;
                current = next;
            }

            _head = previous;
        }

        public IEnumerable<T> AsEnumerable()
        {
            
                var current = _head;
                while (current is not null)
                {
                    yield return current.Value;
                    current = current.Next;
                }
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            var current = _head;
            while (current is not null)
            {
                builder.Append(current.Value);
                if (current.Next is not null)
                {
                    builder.Append(" -> ");
                }
                current = current.Next;
            }

            return builder.ToString();
        }
    }
}
