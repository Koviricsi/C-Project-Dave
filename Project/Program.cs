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

            var marginx = 10;
            var marginy = 5;

            Default(marginx, marginy);

            string filePath = "calendar_data.json";

            var calendarData = CalendarStorage.LoadAllData(filePath); //-> Ez egy Dictionary HA letezik akkor visszaadja az elmentettet ha nem akkor egy ujat ad vissza

            //var asd = new Calendar(2025, 4, 9);
            //asd.Culture("hu-HU");
            //asd.AddCalendarEvent(new DateTime(2025, 4, 9), Style.Parse("yellow"));
            //AnsiConsole.Write(asd);

            var test = new CalendarControl();
            test.Render();


            // SAVE
            CalendarStorage.SaveAllData(calendarData, filePath);
        }
    }
    public class DailyData
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Day { get; set; }
        public List<string> ToDo { get; set; }
    }

    public static class CalendarStorage
    {
        public static void SaveAllData(Dictionary<string, DailyData> allData, string filePath)
        {
            var json = JsonSerializer.Serialize(allData,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                }
            );
            File.WriteAllText(filePath, json);
        }

        public static Dictionary<string, DailyData> LoadAllData(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return new Dictionary<string, DailyData>();
            }

            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<Dictionary<string, DailyData>>(json);
        }

        public static string GetDateKey(int year, int month, int day)
        {
            return $"{year:D4}-{month:D2}-{day:D2}";
        }
    }
}