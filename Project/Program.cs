﻿using Project;
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

        static void Default(int marginx, int marginy)
        {
            Console.SetWindowSize(1, 1);
            Console.SetBufferSize(Console.LargestWindowWidth - marginx, Console.LargestWindowHeight - marginy);
            Console.SetWindowSize(Console.LargestWindowWidth - marginx, Console.LargestWindowHeight - marginy);
            Console.SetWindowPosition(0, 0);

        }

        static async Task Main(string[] args)
        {

            var marginx = 10;
            var marginy = 5;

            Default(marginx,marginy);

            var YearCalendar = new RenderYears(marginx, marginy);
            var a1 = await Task.Run(() => YearCalendar.Render());
            

            var MonthCalendar = new RenderMonths(marginx, marginy);
            var a2 = await Task.Run(() => MonthCalendar.Render());

            var DayCalendar = new RenderDays(marginx, marginy, YearCalendar.SelectedYear, MonthCalendar.SelectedMonth);
            var a3 = await Task.Run(() => DayCalendar.Render());

            Console.WriteLine(a1);
            Console.WriteLine(a2);
            Console.WriteLine(a3);

            Thread.Sleep(-1);
        }
    }
}
