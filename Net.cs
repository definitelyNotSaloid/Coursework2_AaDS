using Lab1_AaDS;
using Lab21_AaDS;
using System;
using System.Collections.Generic;
using System.Text;

namespace Coursework2_AaDS
{
    public class Pair<T1, T2>
    {
        public T1 first;
        public T2 second;

        public Pair(T1 first, T2 second)
        {
            this.first = first;
            this.second = second;
        }
    }
    public class Node
    {
        public NotAList<Pair<Node,int>> next = new NotAList<Pair<Node, int>>();
        public readonly string name;
        public Node(string name) => this.name = name;
    }

    public class Net
    {
        private NotADictionary<string, Node> nodes;
        public Node root;

        public Net()
        {
            nodes = new NotADictionary<string, Node>();
        }

        public Node LazyGetNode(string name)
        {
            Node node;
            if (nodes.TryGetValue(name, out node))
                return node;

            node = new Node(name);
            nodes.Add(name, node);
            return node;
        }

        // Lord forgive me for this method, as it straight up kills instance of this class with its calculations
        // But, as they say, as long as it works....
        public int FindFlow(Node drain)
        {
            var curNode = root;
            NotAList<Pair<Node, int>> path = new NotAList<Pair<Node, int>>();       // doesnt include root, Pair(Node, edge_weight)

            path = FindPath(root, drain);

            var totalFlow = 0;

            while (path!=null)
            {
                var minEdge = path[0];
                foreach (var edge in path)
                {
                    if (minEdge.second > edge.second)
                        minEdge = edge;
                }

                int minCost = minEdge.second;
                totalFlow += minCost;

                Node from = root;
                

                foreach (var edge in path)
                {
                    edge.second -= minCost;

                    Pair<Node, int> reverseEdge;

                    if (!from.next.Find(edge => edge.first == from, out reverseEdge))
                    {
                        reverseEdge = new Pair<Node, int>(from, minCost);
                        edge.first.next.Add(reverseEdge);
                    }

                    reverseEdge.second += minCost;
                    from = edge.first;
                }

                path = FindPath(root, drain);
            }

            return totalFlow;
        }

        public void AddEdge(string nodeAsStr)
        {
            int start = 0;
            int blocksRead = 0;

            Node from = null;
            Node to = null;

            for (int i=0;i<nodeAsStr.Length; i++)
            {
                if (nodeAsStr[i] == ' ' || i == nodeAsStr.Length - 1)
                {
                    string subStr = 
                        i == nodeAsStr.Length - 1 ?
                        nodeAsStr[start..] :
                        nodeAsStr[start..i];
                    start = i + 1;

                    switch (blocksRead)
                    {
                        case 0:
                            from = LazyGetNode(subStr);
                            break;
                        case 1:
                            to = LazyGetNode(subStr);
                            break;
                        case 2:
                            from.next.Add(new Pair<Node, int>(to, Convert.ToInt32(subStr)));
                            break;
                        default:
                            break;
                    }

                    blocksRead++;
                }
            }
        }

        public NotAList<Pair<Node, int>> FindPath(Node start, Node destination, NotAList<Node> avoid = null)
        {
            if (start.next.Find((el) => el.first == destination && el.second>0, out Pair<Node, int> target))
            {
                var res = new NotAList<Pair<Node, int>>
                {
                    target
                };
                return res;
            }

            avoid ??= new NotAList<Node>();
            avoid.Add(start);

            foreach (var way in start.next)
            {
                if (!avoid.Contains(way.first) && way.second > 0)
                {
                    var subpath = FindPath(way.first, destination, avoid);
                    if (subpath != null)
                    {
                        subpath.PushFront(way);
                        return subpath;
                    }
                }
            }

            return null;
        }
    }
}
