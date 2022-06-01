using System;
using System.IO;
using System.Text;

namespace Coursework2_AaDS
{
    class Program
    {
        public static string inputPath = "../../../input/data.txt";
        static void Main(string[] args)
        {
            string inputStr;
            using (var file = File.OpenRead(inputPath))
            {
                byte[] buff = new byte[file.Length];
                file.Read(buff);
                inputStr = Encoding.Default.GetString(buff);
            }

            Net net = new Net();
            int start = 0;

            for (int i = 0; i < inputStr.Length; i++)
            {
                if (inputStr[i]=='\n' || inputStr[i] == '\r')
                {
                    if (i!=start)
                        net.AddEdge(inputStr.Substring(start, i-start));

                    start = i + 1;
                }
            }

            if (start != inputStr.Length - 1)
                net.AddEdge(inputStr[start..]);


            net.root = net.LazyGetNode("source");

            Console.WriteLine(net.FindFlow(net.LazyGetNode("drain")));
        }
    }
}
