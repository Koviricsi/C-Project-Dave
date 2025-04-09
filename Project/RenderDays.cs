using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Project
{
    class RenderDays
    {
        public int DefaultDay { get; set; }
        public int SelectedDay { get; set; }
        private int _marginy;
        private int _centerX;
        private int _month;
        private int _year;

        private Thread control;

        public RenderDays(int marginx, int marginy, int year, int month)
        {
            DefaultDay = DateTime.Now.Day;
            SelectedDay = -1;
            _month = month;
            _year = year;

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
                        if (key == ConsoleKey.UpArrow && DefaultDay - 1 > 0)
                        {
                            DefaultDay--;
                            Render();
                        }
                        else if (key == ConsoleKey.DownArrow && DefaultDay + 1 <= DateTime.DaysInMonth(_year, _month))
                        {
                            DefaultDay++;
                            Render();
                        }
                        else if (key == ConsoleKey.Enter)
                        {
                            SelectedDay = DefaultDay;
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
            var center = _centerX - (int)Math.Ceiling((DefaultDay.ToString().Length * 7.0 + (DefaultDay.ToString().Length + 1) * 3 + 2) / 2);

            if (DefaultDay - 1 <= 0)
            {
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay, center, 3, false);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay + 1, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay + 2, center, 3, true);
            }
            else if (DefaultDay + 1 >= DateTime.DaysInMonth(_year, _month))
            {
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay - 2, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay - 1, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay, center, 3, false);

            }
            else
            {
                ASCIIChars.RenderNum(DefaultDay - 1, center, 3, true);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay, center, 3, false);
                Console.WriteLine();
                ASCIIChars.RenderNum(DefaultDay + 1, center, 3, true);
            }

            if (!control.IsAlive)
            {
                CreateThread();
                Console.WindowHeight = 25;
                control.Start();
                control.Join();
                Console.WindowHeight = Console.LargestWindowHeight - _marginy;
                return SelectedDay;
            }
            return -1;
        }
    }
}
