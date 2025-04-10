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
            var calendar = new CalendarControl();
            string filePath = "calendar_data.json";

            // LOAD
            var calendarData = CalendarStorage.LoadAllData(filePath);
            foreach (var i in calendarData.Values) {
                calendar.AddCalendarEvent(i.Year,i.Month,i.Day);
            }

            // RICSI STUFF
            var marginx = 10;
            var marginy = 5;

            Default(marginx, marginy);

            // KEZDES

            while (true) {
                calendar.Render(true);
                if (calendar.isClosing) {
                    break;
                }

                var dateKey = CalendarStorage.GetDateKey(calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);

                if (calendarData.TryGetValue(dateKey, out DailyData dayData)) {
                    // DEV: majd ebben az esetben lehet felhozni ezen opciokat: Új event hozzadasa, Régi event torlese, Nap törlese

                    AnsiConsole.MarkupLine($"\n[yellow]Eventek: {dateKey}:[/]");
                    foreach (var task in dayData.ToDo) {
                        AnsiConsole.MarkupLine($"[bold cyan]-[/] {task}");
                    }
                    var option = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Opciok:")
                            .PageSize(10)
                            .AddChoices(new[] {
                            "Add new event", "Remove old event", "Delet Day", "Back"
                    }));

                    if (option == "Add new event") {
                        var newEvent = AnsiConsole.Prompt(
                            new TextPrompt<string>("Name your event: "));
                        CalendarManager.AddTask(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day, newEvent);
                        CalendarStorage.SaveAllData(calendarData, filePath);

                    }
                    if (option == "Remove old event") {
                        var oldEvent = AnsiConsole.Prompt(
                            new TextPrompt<string>("Name the event to delete: "));

                        bool notFound = true;

                        for (int i = 0; i < dayData.ToDo.Count; i++) {
                            if (dayData.ToDo[i] == oldEvent) {
                                CalendarManager.RemoveTask(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day, oldEvent);
                                CalendarStorage.SaveAllData(calendarData, filePath);
                                AnsiConsole.MarkupLine($"\n[grey]{oldEvent} deleted from the day.[/]");
                                notFound = false;
                                break;
                            }
                        }

                        if (notFound) {
                            AnsiConsole.MarkupLine($"\n[grey]{oldEvent} not found.[/]");
                        }
                    }
                    if (option == "Delet Day") {
                        CalendarManager.DeleteDay(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);
                        calendar.RemoveCalendarEvent(calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);
                        CalendarStorage.SaveAllData(calendarData, filePath);
                    }
                    if (option == "Back") {
                        continue;
                    }

                } else {
                    // DEV: majd ebben az esetben lehet felhozni opciot: event hozzadas
                    AnsiConsole.MarkupLine($"\n[grey]Nincs event ezen a napon {dateKey}.[/]");
                    var option = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("Opciok:")
                            .PageSize(10)
                            .AddChoices(new[] {
                            "Add event(s) to this day", "Back"
                    }));
                    if (option == "Add event(s) to this day") {
                        Console.WriteLine("Add meg az eventeket ilyen (event1;event2;event3):");
                        string inp = Console.ReadLine();
                        var elemek = inp.Split(';');

                        var list = new List<string>(elemek);

                        CalendarManager.CreateOrUpdateDay(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day, list);
                        calendar.AddCalendarEvent(calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);
                        CalendarStorage.SaveAllData(calendarData, filePath);
                    }

                    if (option == "Back") {
                        continue;
                    }
                }
            }
            




            // SAVE 2
            CalendarStorage.SaveAllData(calendarData, filePath);

            Console.ReadKey(true);
        }
    }
}