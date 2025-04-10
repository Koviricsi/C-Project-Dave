using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Project
{
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
