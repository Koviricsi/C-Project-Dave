using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project
{
    class RenderYears
    {
        private List<string[]> Numbers = new List<string[]>();

        public int DefaultYear { get; set; }
        private int _height;
        private int _width;
        private int _centerX;
        private int _centerY;

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
            Numbers.Add(new string[] { " █████ ", "██   ██", "██   ██", "██   ██", "██   ██", "██   ██", " █████ " });
            Numbers.Add(new string[] { "  ███  ", " ████  ", "█ ███  ", "  ███  ", "  ███  ", "  ███  ", "███████" });
            Numbers.Add(new string[] { " █████ ", "█   ███", "    ███", "   ███ ", "  ███  ", " ███   ", "███████" });
            Numbers.Add(new string[] { "", "", "", "", "", "", "" });
        }

        public void RenderLine(string text)
        {
            Console.Write("|".PadLeft(_centerX - (text.Length / 2) - 10, ' '));
            Console.Write(text.PadLeft(text.Length + 10 - 1, ' '));
            Console.Write("|\n".PadLeft(10, ' '));
        }

        public void Render()
        {
            foreach (var text in Numbers[0])
                RenderLine(text);
            Console.WriteLine();
            foreach (var text in Numbers[1])
                RenderLine(text);
            Console.WriteLine();
            foreach (var text in Numbers[2])
                RenderLine(text);

        }
        }
}
