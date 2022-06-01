using NUnit.Framework;
using Coursework2_AaDS;
using System;

namespace tests
{
    public class Tests
    {
        private Net net = new Net();
        [SetUp]
        public void Setup()
        {
            net = new Net();
            net.root = net.LazyGetNode("source");
        }

        [Test]
        public void TestBasics()
        {
            net.AddEdge("source b 3");       
            net.AddEdge("source c 2");
            net.AddEdge("b c 5");
            net.AddEdge("c b 5");
            net.AddEdge("c d 5");
            net.AddEdge("b d 3");

            int res = net.FindFlow(net.LazyGetNode("d"));
            Assert.AreEqual(2 + 3, res);        // source->b and source->c - min slice
        }

        [Test]
        public void TestRandomNet()
        {
            
            var rng = new Random();

            for (int i = 0; i < 10000; i++)
            {
                switch (i) 
                {
                    case 250:
                        net.AddEdge("source a 4");
                        break;
                    case 500:
                        net.AddEdge("source b 5");
                        break;
                    case 750:
                        net.AddEdge("b drain 3");
                        break;
                    case 123:
                        net.AddEdge("c drain 4");
                        break;
                    case 987:
                        net.AddEdge("a c 7");
                        break;
                    default:
                        char ch1 = (char)rng.Next('a', 'z');
                        char ch2 = (char)rng.Next('a', 'z');
                        int weight = rng.Next(10, 999999);

                        net.AddEdge($"{ch1} {ch2} {weight}");
                        break;
                }
            }

            var flow = net.FindFlow(net.LazyGetNode("drain"));
            Assert.AreEqual(7, flow);
            
        }

        [Test]
        public void TestPathfinding()
        {
            // just chaotic graph, not even a net
            // pathfinder must be ok with it
            net.AddEdge("source b 3");
            net.AddEdge("source c 2");
            net.AddEdge("source f 2");
            net.AddEdge("b c 5");
            net.AddEdge("c b 5");
            net.AddEdge("c f 5");
            net.AddEdge("c t 5");
            net.AddEdge("t c 5");
            net.AddEdge("e a 5");
            net.AddEdge("c d 5");
            net.AddEdge("b f 3");
            net.AddEdge("e d 3");

            var path = net.FindPath(net.LazyGetNode("t"), net.LazyGetNode("f"));

            bool check()
            {
                if (path[0].second <= 0)
                    return false;
                for (int i = 1; i < path.Count; i++)
                {
                    if (!path[i - 1].first.next.Contains(path[i]) || path[i].second <= 0)
                        return false;
                }

                return true;
            }

            Assert.True(check());

        }

    }
}