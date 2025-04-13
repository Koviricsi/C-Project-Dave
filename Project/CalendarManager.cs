using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project
{
    public static class CalendarManager
    {
        public static void CreateOrUpdateDay(Dictionary<string, DailyData> data, int year, int month, int day, List<string> tasks)
        {
            string key = $"{year:D4}-{month:D2}-{day:D2}";

            data[key] = new DailyData
            {
                Year = year,
                Month = month,
                Day = day,
                ToDo = tasks
            };
        }

        public static bool DeleteDay(Dictionary<string, DailyData> data, int year, int month, int day)
        {
            string key = $"{year:D4}-{month:D2}-{day:D2}";
            return data.Remove(key);
        }

        public static void AddTask(Dictionary<string, DailyData> data, int year, int month, int day, string task)
        {
            string key = $"{year:D4}-{month:D2}-{day:D2}";

            // Task hozzadasa
            if (data.TryGetValue(key, out var dayData))
            {
                dayData.ToDo.Add(task);
            }
            else
            {
                data[key] = new DailyData
                {
                    Year = year,
                    Month = month,
                    Day = day,
                    ToDo = new List<string> { task }
                };
            }
        }

        public static bool RemoveTask(Dictionary<string, DailyData> data, int year, int month, int day, string task)
        {
            string key = $"{year:D4}-{month:D2}-{day:D2}";
            if (data.TryGetValue(key, out var dayData))
            {
                dayData.ToDo.Remove(task);
                if (dayData.ToDo.Count == 0)
                {
                    data.Remove(key);

                    return true;
                }
            }

            return false;
        }


    }
}
