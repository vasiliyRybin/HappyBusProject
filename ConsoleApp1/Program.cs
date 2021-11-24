using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<string> list = null;

            if (null != list && !list.Any() ) {
                Console.WriteLine("dsds");
            }

            Console.WriteLine("Hello World!");
        }
    }
}
