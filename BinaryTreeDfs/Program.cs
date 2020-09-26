using System.Collections.Generic;
using System.Text;
using System.IO;
using System;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;

namespace BinaryTreeDfs
{
    public class Node<TKey, TValue>
        where TKey : struct
        where TValue : struct
    {
        public TKey Key { get; set; }
        
        public TValue Value { get; set; }

        public Node<TKey, TValue>? Left { get; set; }

        public Node<TKey, TValue>? Right { get; set; }
    }

    public class NodeDfs
    {
        private static int MaxDepth = 15;
        private Node<long, Guid> Root { get; set;}

        public NodeDfs()
        {
            var (binaryTree, _) = RecursiveGeneration(0, 0);
            Root = binaryTree;        
        }

        private static (Node<long, Guid>? root, long index) RecursiveGeneration(int depth, long index)
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

        [Benchmark(Baseline = true)]
        public void Recursive()
        {
            var filename = Path.GetTempFileName();
            using var sw = File.AppendText(filename);
            RecursiveInternal(Root, sw, new StringBuilder());
            sw.Close();
            
            File.Delete(filename);
        }

        [Benchmark]
        public void Stack()
        {
            var filename = Path.GetTempFileName();
            var sb = new StringBuilder();
            using var sw = File.AppendText(filename);
            
            var stack = new Stack<Node<long, Guid>>();
            stack.Push(Root);

            while (stack.TryPop(out var nextNode))
            {
                sb
                    .Append(nextNode.Key)
                    .Append(": ")
                    .Append(nextNode.Value)
                    .Append(" Left: ")
                    .Append(nextNode.Left?.Key)
                    .Append(" Right: ")
                    .Append(nextNode.Right?.Key);

                sw.WriteLine(sb.ToString());
                sb.Clear();

                if (nextNode.Right != null)
                    stack.Push(nextNode.Right); 

                if (nextNode.Left != null)
                    stack.Push(nextNode.Left);                         
            }
            sw.Close();
            
            File.Delete(filename);
        }

        private void RecursiveInternal(
            Node<long, Guid> root, 
            StreamWriter sw,
            StringBuilder sb)
        {
            sb
                .Append(root.Key)
                .Append(": ")
                .Append(root.Value)
                .Append(" Left: ")
                .Append(root.Left?.Key)
                .Append(" Right: ")
                .Append(root.Right?.Key);

            sw.WriteLine(sb.ToString());
            sb.Clear();

            if (root.Left != null)
                RecursiveInternal(root.Left, sw, sb);
            
            if (root.Right != null)
                RecursiveInternal(root.Right, sw, sb);
        }
    }

    class Program
    {

        static void Main(string[] args)
        {
            // var nodeDfs = new NodeDfs();
            // nodeDfs.Recursive();
            // nodeDfs.Stack();

            BenchmarkRunner.Run<NodeDfs>();
        }
    }
}
