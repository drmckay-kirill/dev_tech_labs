using System.Text;
using System.IO;
using System;

namespace BinaryTreeDfs
{
    public class Node<TKey, TValue> 
    {
        public TKey Key { get; set; }
        
        public TValue Value { get; set; }

        public Node<TKey, TValue> Left { get; set; }

        public Node<TKey, TValue> Right { get; set; }
    }

    public class NodeDfs
    {
        public void Recursive(Node<long, Guid> root)
        {
            var filename = Path.GetTempFileName();
            using var sw = File.AppendText(filename);
            RecursiveInternal(root, sw);
            sw.Close();
            
            Console.Write(filename);
        }

        private void RecursiveInternal(Node<long, Guid> root, StreamWriter sw)
        {
            var sb = new StringBuilder();

            sb
                .Append(root.Key)
                .Append(": ")
                .Append(root.Value)
                .Append(" Left: ")
                .Append(root.Left?.Key)
                .Append(" Right: ")
                .Append(root.Right?.Key);

            sw.WriteLine(sb.ToString());

            if (root.Left != null)
                RecursiveInternal(root.Left, sw);
            
            if (root.Right != null)
                RecursiveInternal(root.Right, sw);
        }
    }

    class Program
    {
        private static int MaxDepth = 10;

        static void Main(string[] args)
        {
            var (binaryTree, _) = RecursiveGeneration(0, 1);
            var nodeDfs = new NodeDfs();
            nodeDfs.Recursive(binaryTree);
        }

        private static (Node<long, Guid> root, long index) RecursiveGeneration(int depth, long index)
        {
            if (depth > MaxDepth)
                return (null, index);

            index++;

            var root = new Node<long, Guid>
            {
                Key = index,
                Value = Guid.NewGuid(),
            };
            var (left, leftIndex) = RecursiveGeneration(depth + 1, index);
            var (right, rightIndex) = RecursiveGeneration(depth + 1, leftIndex);

            root.Left = left;
            root.Right = right;

            return (root, rightIndex);
        }
    }
}
