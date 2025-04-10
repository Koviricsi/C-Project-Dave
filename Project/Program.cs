using Project;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;
using Spectre.Console;
using Spectre.Console.Rendering;
using System.Windows;


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

            Default(marginx, marginy);

            var calendar = new CalendarControl();

            var a = new Calendar(new DateTime(2025, 04, 10));
            a.Border = TableBorder.DoubleEdge;
            AnsiConsole.Write(a);

            Console.InputEncoding = Console.OutputEncoding = Encoding.UTF8;
            AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots10)
                .Start("Loading...", ctx =>
                {
                    while (true)
                    {
                        int i = 0;
                        i++;
                    }
                });
        }
    }
}