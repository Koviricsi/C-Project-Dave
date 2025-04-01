using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project
{
    class Program
    {

        static void Main(string[] args)
        {
            var marginx = 5;
            var marginy = 10;
            Console.SetWindowSize(Console.LargestWindowWidth-marginx, Console.LargestWindowHeight-marginy);
            var YearC = new RenderYears(2025, 5, 10);
            YearC.Render();

            Thread.Sleep(-1);
        }
    }
}
