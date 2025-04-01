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
        private int _height;
        private int _width;
        private int _centerX;
        private int _centerY;

        private Thread control;

        public RenderYears(int year, int marginx, int marginy)
        {
            DefaultYear = year;
            var width = (Console.LargestWindowWidth - marginx % 2 == 0) ? Console.LargestWindowWidth - marginx + 1 : Console.LargestWindowWidth - marginx;
            var height = (Console.LargestWindowHeight - marginy % 2 == 0) ? Console.LargestWindowHeight - marginy + 1 : Console.LargestWindowHeight - marginy;
            var centerX = (int)Math.Ceiling(width / 2.0);
            var centerY = (int)Math.Ceiling(height / 2.0);

            _width = width;
            _height = height;
            _centerX = centerX;
            _centerY = centerY;

            control = new Thread(() =>
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.UpArrow)
                        {
                            DefaultYear--;
                            Console.Clear();
                            Render();
                        }
                        else if (key == ConsoleKey.DownArrow)
                        {
                            DefaultYear++;
                            Console.Clear();
                            Render();
                        }
                    }
                }
            });
            control.Start();
        }

        public void Render()
        {
            var center = _centerX - (int)Math.Ceiling((DefaultYear.ToString().Length * 7.0 + (DefaultYear.ToString().Length + 1) * 3 + 2) / 2);
            Console.WriteLine();
            Numbers.Render(DefaultYear - 1, center, 3, true);
            Console.WriteLine();
            Numbers.Render(DefaultYear, center, 3, false);
            Console.WriteLine();
            Numbers.Render(DefaultYear + 1, center, 3, true);
        }
    }
}
