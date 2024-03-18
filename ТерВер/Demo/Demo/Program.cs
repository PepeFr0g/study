using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;

namespace Demo
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();

            Random rnd = new Random();

            for (int i = 0; i < 20; i++)
            {
                list.Add(rnd.Next(0, 100));
            }

            list.Sort();

            for (int i = 0; i < 20; i++)
            {
                Console.Write($"{list[i]} ");
            }

            List<int> group = list.Distinct().ToList();

            Console.WriteLine();
            for (int i = 0; i < group.Count; i++)
            {
                Console.Write($"{group[i]} ");
            }


            Console.ReadKey();
        }
    }
}
