﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project
{
    class RenderMonths
    {
        public int DefaultMonth { get; set; }
        public int SelectedMonth { get; set; }
        private int _marginy;

        private Thread control;

        public RenderMonths(int marginx, int marginy)
        {
            DefaultMonth = DateTime.Now.Month;
            SelectedMonth = -1;

            var width = (Console.LargestWindowWidth - marginx % 2 == 0) ? Console.LargestWindowWidth - marginx + 1 : Console.LargestWindowWidth - marginx;
            var centerX = (int)Math.Ceiling(width / 2.0);

            CreateThread();
        }

        void CreateThread()
        {
            control = new Thread(() =>
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.UpArrow && DefaultMonth-1 > 0)
                        {
                            DefaultMonth--;
                            Render();
                        }
                        else if (key == ConsoleKey.DownArrow && DefaultMonth + 1 <= 12)
                        {
                            DefaultMonth++;
                            Render();
                        }
                        else if (key == ConsoleKey.Enter)
                        {
                            SelectedMonth = DefaultMonth;
                            Console.Clear();
                            control.Abort();
                        }
                    }
                }
            });
        }

        public int Render()
        {
            Console.Clear();

            if (DefaultMonth-1 <= 0)
            {
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth, false);
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth+1, true);
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth+2, true);
            }
            else if(DefaultMonth+1 >= 13)
            {
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth-2, true);
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth-1, true);
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth, false);

            }
            else
            {
                ASCIIChars.RenderText(DefaultMonth-1, true);
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth, false);
                Console.WriteLine();
                ASCIIChars.RenderText(DefaultMonth+1, true);
            }

            if (!control.IsAlive)
            {
                CreateThread();
                Console.WindowHeight = 25;
                control.Start();
                control.Join();
                Console.WindowHeight = Console.LargestWindowHeight - _marginy;
                return SelectedMonth;
            }
            return -1;
        }
    }
}
