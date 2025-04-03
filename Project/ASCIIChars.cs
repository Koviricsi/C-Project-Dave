using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    class ASCIIChars
    {
        private static List<string[]> numbers = new List<string[]>
            {
                new string[] { " █████ ", "██   ██", "██   ██", "██   ██", "██   ██", "██   ██", " █████ " },
                new string[] { "  ███  ", " ████  ", "█ ███  ", "  ███  ", "  ███  ", "  ███  ", "███████" },
                new string[] { " █████ ", "█   ███", "    ███", "   ███ ", "  ███  ", " ███   ", "███████" },
                new string[] { "██████ ", "     ██", "     ██", " █████ ", "     ██", "     ██", "██████ " },
                new string[] { "   ███ ", " █████ ", " ██ ██ ", "██  ██ ", "███████", "    ██ ", "    ██ " },
                new string[] { "███████", "██     ", "██     ", "██████ ", "     ██", "     ██", "██████ " },
                new string[] { " █████ ", "██     ", "██     ", "██████ ", "██   ██", "██   ██", " █████ " },
                new string[] { "███████", "     ██", "     ██", "   ██  ", " ██    ", " ██    ", " ██    " },
                new string[] { " █████ ", "██   ██", "██   ██", " █████ ", "██   ██", "██   ██", " █████ " },
                new string[] { " █████ ", "██   ██", "██   ██", " ██████", "     ██", "     ██", " █████ " }
            };

        private static List<string[]> months = new List<string[]>
            {
                new string[] { "|      ███    ███    ███  ██  ██   ██    ███ █  █████    |",
                               "|      ███    ███    ███  ██  ██   ██    ███ █  ██   ██  |",
                               "|      ███   ██ ██   ██ █ ██  ██   ██   ██ ██   ██   ██  |",
                               "|      ███   ██ ██   ██ █ ██  ██   ██   ██ ██   ██   ██  |",
                               "|      ███  ███████  ██ █ ██  ██   ██  ███████  █████    |",
                               "|  ██  ███  ██   ██  ██  ███  ██   ██  ██   ██  ██   ██  |",
                               "|   █████   ██   ██  ██  ███   █████   ██   ██  ██   ██  |" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" },
                new string[] { "", "", "", "", "", "", "" }
            };

        private static void Padding(int length, string character)
        {
            for (int i = 0; i < length; i++)
            {
                Console.Write(character);
            }
        }

        private static void RenderLine(string text, int pl, int pr)
        {
            if (pl <= 0 && pr <= 0)
            {
                Console.Write(text);
            }
            else if (pl <= 0 && pr > 0)
            {
                Console.Write(text);
                Padding(pr, " ");
                Console.Write("|");
            }
            else if (pl > 0 && pr <= 0)
            {
                Padding(pl, " ");
                Console.Write("|");
                Padding(3, " ");
                Console.Write(text);
            }
            else if (pl > 0 && pr > 0)
            {
                Padding(pl, " ");
                Console.Write("|");
                Padding(pr, " ");
                Console.Write(text);
                Padding(pr, " ");
                Console.Write("|");
            }

        }

        public static void RenderNum(int number, int pl, int pr, bool trans)
        {
            char chars = trans ? '▒' : '█';

            if (number.ToString().Length > 1)
            {
                for (int i = 0; i < 7; i++)
                {

                    for (int j = 0; j < number.ToString().Length; j++)
                    {

                        if (j == 0)
                        {
                            RenderLine(numbers[int.Parse(number.ToString()[j].ToString())][i].Replace('█', chars), pl, 0);
                            Console.Write("   ");
                        }
                        else if (j == number.ToString().Length - 1)
                        {
                            RenderLine(numbers[int.Parse(number.ToString()[j].ToString())][i].Replace('█', chars), 0, pr);
                        }
                        else
                        {
                            RenderLine(numbers[int.Parse(number.ToString()[j].ToString())][i].Replace('█', chars), 0, 0);
                            Console.Write("   ");
                        }

                    }

                    Console.Write("\n");
                }
            }

            else
            {
                foreach (var text in numbers[number])
                {
                    RenderLine(text.Replace('█', chars), pl, pr);
                    Console.Write("\n");
                }
            }
        }

        public static void RenderText(int number, int pl, bool trans)
        {
            char chars = trans ? '▒' : '█';

            foreach (var text in months[number-1])
            {
                Console.Write(text.Replace('█', chars));
                Console.Write("\n");
            }

        }
    }
}
