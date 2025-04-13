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
using Spectre.Console.Rendering;
using System.Windows;


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

                    var rootdate = new Tree("[gold1 bold]"+dayData.ToString()+"[/]");
                    rootdate.Style("lightgoldenrod2_1");
                    rootdate.AddNodes(dayData.ToDo.Select(text => new Markup("[orange1]"+text+"[/]")));
                    AnsiConsole.Write(rootdate);
                    Console.WriteLine();

                    var option = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[dodgerblue2]Opciók:[/]")
                            .PageSize(10)
                            .AddChoices(new[] {
                            "Esemény hozzáadása", "Esemény törlése", "Összes esemény törlése", "Vissza"
                    }));

                    if (option == "Esemény hozzáadása") {
                        var newEvent = AnsiConsole.Prompt(
                            new TextPrompt<string>("Nevezd el az eseményt: "));
                        CalendarManager.AddTask(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day, newEvent);
                        CalendarStorage.SaveAllData(calendarData, filePath);

                    }
                    if (option == "Esemény törlése") {
                        var oldEvent = AnsiConsole.Prompt(
                            new TextPrompt<string>("Törölni kívánt esemény neve: "));

                        bool notFound = true;

                        for (int i = 0; i < dayData.ToDo.Count; i++) {
                            if (dayData.ToDo[i] == oldEvent) {
                                if (CalendarManager.RemoveTask(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day, oldEvent)) 
                                {
                                    calendar.RemoveCalendarEvent(calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);
                                }
                                CalendarStorage.SaveAllData(calendarData, filePath);
                                AnsiConsole.MarkupLine($"\n[grey]{oldEvent} törlésre került.[/]");
                                notFound = false;
                                break;
                            }
                        }

                        if (notFound) {
                            AnsiConsole.MarkupLine($"\n[grey]{oldEvent} nem található.[/]");
                        }
                    }
                    if (option == "Összes esemény törlése") {
                        CalendarManager.DeleteDay(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);
                        calendar.RemoveCalendarEvent(calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);
                        CalendarStorage.SaveAllData(calendarData, filePath);
                    }
                    if (option == "Vissza") {
                        continue;
                    }

                } else {
                    // DEV: majd ebben az esetben lehet felhozni opciot: event hozzadas
                    AnsiConsole.MarkupLine($"\n[grey]Nincs esemény ezen a napon: {dateKey}.[/]\n");
                    var option = AnsiConsole.Prompt(
                        new SelectionPrompt<string>()
                            .Title("[dodgerblue2]Opciók:[/]")
                            .PageSize(10)
                            .AddChoices(new[] {
                            "Esemény(ek) hozzáadása ehhez a naphoz", "Vissza"
                    }));
                    if (option == "Esemény(ek) hozzáadása ehhez a naphoz") {
                        AnsiConsole.MarkupLine("[aqua]Adja meg az [yellow]esemény(ek)et[/] a megadott formátumban [yellow](esemény1;esemény2;esemény3):[/][/]");
                        string inp = Console.ReadLine();
                        var elemek = inp.Split(';');

                        var list = new List<string>(elemek);

                        CalendarManager.CreateOrUpdateDay(calendarData, calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day, list);
                        calendar.AddCalendarEvent(calendar.SelectedDate.Year, calendar.SelectedDate.Month, calendar.SelectedDate.Day);
                        CalendarStorage.SaveAllData(calendarData, filePath);
                    }

                    if (option == "Vissza") {
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