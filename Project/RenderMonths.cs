using System;
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
        private int _centerX;

        private Thread control;

        public RenderMonths(int marginx, int marginy)
        {
            DefaultMonth = DateTime.Now.Month;
            SelectedMonth = -1;

            var width = (Console.LargestWindowWidth - marginx % 2 == 0) ? Console.LargestWindowWidth - marginx + 1 : Console.LargestWindowWidth - marginx;
            var centerX = (int)Math.Ceiling(width / 2.0);

            _centerX = centerX;

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
            var center = _centerX - (int)Math.Ceiling((DefaultMonth.ToString().Length * 7.0 + (DefaultMonth.ToString().Length + 1) * 3 + 2) / 2);

            Console.WriteLine();

            if (DefaultMonth - 1 > 0)
                ASCIIChars.RenderText(DefaultMonth - 1, center, true);
                Console.WriteLine();

            ASCIIChars.RenderText(DefaultMonth, center, false);
            Console.WriteLine();

            if (DefaultMonth + 1 <= 12)
                ASCIIChars.RenderText(DefaultMonth + 1, center, true);

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
