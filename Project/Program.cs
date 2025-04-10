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

        }
    }
}