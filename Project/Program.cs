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

        static void Default()
        {
            var marginx = 10;
            var marginy = 5;

            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(Console.LargestWindowWidth - marginx, Console.LargestWindowHeight - marginy);
            Console.SetWindowSize(Console.LargestWindowWidth - marginx, Console.LargestWindowHeight - marginy);
            Console.SetWindowPosition(0, 0);

        }

        static async Task Main(string[] args)
        {
            Default();

            var YearCalendar = new RenderYears(5, 10);
            var MonthCalendar = new RenderMonths(5, 10);

            //await Task.Run(() => YearCalendar.Render());

            var asd = await Task.Run(() => MonthCalendar.Render()); ;
            Console.WriteLine(asd);

            Thread.Sleep(-1);
        }
    }
}
