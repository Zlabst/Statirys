using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            SortedSet<int> followers = new SortedSet<int>();
            SortedSet<int> following = new SortedSet<int>();

            followers.Add(1);
            followers.Add(2);
            followers.Add(14);

            following.Add(1);
            following.Add(14);
            following.Add(3);

            foreach(int i in following.Intersect(followers))
            Console.WriteLine(i);

        }



        private static void DoOtherWork()
        {
            for (int i = 0; i < 1000; i++)
            {
                Console.Write(i);
                Console.Clear();
            }
        }
    }
}
