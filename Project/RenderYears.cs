using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project
{
    class RenderYears
    {
        public int DefaultYear { get; set; }
        public int SelectedYear { get; set; }
        private int _marginy;
        private int _centerX;

        private Thread control;

        public RenderYears(int marginx, int marginy)
        {
            DefaultYear = DateTime.Now.Year;
            SelectedYear = -1;

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
                        if (key == ConsoleKey.UpArrow && DefaultYear - 1 > 0)
                        {
                            DefaultYear--;
                            Render();
                        }
                        else if (key == ConsoleKey.DownArrow && DefaultYear + 1 <= 9999)
                        {
                            DefaultYear++;
                            Render();
                        }
                        else if (key == ConsoleKey.Enter)
                        {
                            SelectedYear = DefaultYear;
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
            var center = _centerX - (int)Math.Ceiling((DefaultYear.ToString().Length * 7.0 + (DefaultYear.ToString().Length + 1) * 3 + 2) / 2);

            if (DefaultYear - 1 <= 0)
            {
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear, center, 3, false);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear + 1, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear + 2, center, 3, true);
            }
            else if (DefaultYear + 1 >= 9999)
            {
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear - 2, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear - 1, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear, center, 3, false);

            }
            else
            {
                ASCIIChars.RenderNum(DefaultYear - 1, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear, center, 3, false);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultYear + 1, center, 3, true);
            }

            if (!control.IsAlive)
            {
                CreateThread();
                Console.WindowHeight = 25;
                control.Start();
                control.Join();
                Console.WindowHeight = Console.LargestWindowHeight - _marginy;
                return SelectedYear;
            }
            return -1;
        }
    }
}
