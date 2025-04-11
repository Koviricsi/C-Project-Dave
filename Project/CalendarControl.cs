using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Spectre.Console;

namespace Project
{
    class CalendarControl
    {
        public bool isClosing;
        private int _year;
        private int _month;
        private int _day;
        private Calendar _calendar;
        private CalendarEvent[] _events;
        private Thread _monthSelect;
        private DateTime _selectedDate;

        public CalendarControl()
        {
            isClosing = false;
            var now = DateTime.Now;
            _calendar = new Calendar(now.Year, now.Month, now.Day);
            _calendar.Culture("hu-HU");
            _calendar.BorderStyle(Style.Parse("yellow"));
            _calendar.HeaderStyle(Style.Parse("dodgerblue1"));
            _calendar.Alignment(Justify.Center);
            _calendar.AddCalendarEvent("Mai nap",new DateTime(now.Year, now.Month, now.Day), Style.Parse("blue"));

            _events = _calendar.CalendarEvents.ToArray();

            _year = now.Year;
            _month = now.Month;
            _day = now.Day;
            _selectedDate = new DateTime(now.Year, now.Month, now.Day);
        }

        private void CreateThread()
        {
            _monthSelect = new Thread(() =>
            {
                while (true)
                {
                    if (Console.KeyAvailable)
                    {
                        var key = Console.ReadKey(true).Key;
                        if (key == ConsoleKey.LeftArrow && Month - 1 > 0)
                        {
                            Month--;
                            Render();
                        }
                        else if (key == ConsoleKey.RightArrow && Month + 1 <= 12)
                        {
                            Month++;
                            Render();
                        }
                        else if (key == ConsoleKey.LeftArrow && Month - 1 == 0)
                        {
                            Month = 12;
                            Year--;
                            Render();
                        }
                        else if (key == ConsoleKey.RightArrow && Month + 1 == 13)
                        {
                            Month = 1;
                            Year++;
                            Render();
                        }
                        else if (key == ConsoleKey.Enter)
                        {
                            SetSelectedDate();
                        }
                        else if (key == ConsoleKey.Escape) {
                            isClosing = true;
                            Clear();
                        }
                    }
                }
            });
        }

        public void Render(bool isMainRender = false)
        {
            AnsiConsole.Clear();
            Console.WriteLine();
            AnsiConsole.Write(_calendar);

            if (_monthSelect == null || !_monthSelect.IsAlive)
            {
                CreateThread();
                _monthSelect.Start();
            }
            Console.WriteLine();
            if (isMainRender)
            {
             _monthSelect.Join();
            }
        }

        public void Clear()
        {            
            _monthSelect.Abort(); 
            AnsiConsole.Clear();
        }

        public void AddCalendarEvent(int year, int month, int day)
        {
            var now = DateTime.Now;
            if (year == now.Year
                && month == now.Month
                && day == now.Day)
            {
                _calendar.AddCalendarEvent(new DateTime(year,month,day));
            }
            _calendar.AddCalendarEvent(new DateTime(year, month, day), Style.Parse("aqua"));
            _events = _calendar.CalendarEvents.ToArray();
        }

        public void RemoveCalendarEvent(int year, int month, int day)
        {
            var now = DateTime.Now;
            foreach (var item in _events)
            {
                if (item.Year == year 
                    && item.Month == month
                    && item.Day == day)
                {
                    if (_events[0] != item)
                    {
                        _calendar.CalendarEvents.Remove(item);
                        _events = _calendar.CalendarEvents.ToArray();
                    }
                }
            }
        }

        public void SetSelectedDate()
        {
            AnsiConsole.Markup("[aqua]Adjon meg egy [yellow]dátumot (egész számot)[/] a fenti táblázatból az aznapi [yellow]események megtekintéséhez, módosításához:[/] [/]");
            string inputBuffer = "";
            int daysInMonth = DateTime.DaysInMonth(_year, _month);
            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var keyInfo = Console.ReadKey(true);
                    if (char.IsDigit(keyInfo.KeyChar))
                    {
                        inputBuffer += keyInfo.KeyChar;
                        Console.Write(keyInfo.KeyChar);
                    }
                    else if (keyInfo.Key == ConsoleKey.Backspace && inputBuffer.Length > 0)
                    {
                        inputBuffer = inputBuffer.Substring(0, inputBuffer.Length - 1);
                        Console.Write("\b \b");
                    }
                    else if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        if (int.TryParse(inputBuffer, out int day) && day > 0 && day <= daysInMonth)
                        {
                            _selectedDate = new DateTime(_year, _month, day);
                            break;
                        }
                        else
                        {
                            var errorMessage = "[red]Érvénytelen dátum! Kérlek próbáld újra.[/]";
                            Console.CursorLeft = 0;
                            AnsiConsole.MarkupLine(errorMessage + new string(' ', Console.WindowWidth - errorMessage.Length));
                            inputBuffer = "";
                            AnsiConsole.Markup("[aqua]Adjon meg egy [yellow]dátumot (egész számot)[/] a fenti táblázatból az aznapi [yellow]események megtekintéséhez, módosításához:[/] [/]");
                        }
                    }
                    else if (keyInfo.Key == ConsoleKey.Escape)
                    {
                        Render();
                        return;
                    }
                }
            }
            Clear();
        }

        public int Year
        {
            get { return _year; }
            set
            {
                _year = value;
                _calendar.Year = value;
                Render();
            }
        }

        public int Month
        {
            get { return _month; }
            set
            {
                _month = value;
                _calendar.Month = value;
                Render();
            }
        }

        public int Day
        {
            get { return _day; }
            set
            {
                _day = value;
                _calendar.Day = value;
                Render();
            }
        }

        public DateTime SelectedDate { get { return _selectedDate; } }
    }
}
