using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace SofiaMetroSchedulesTrafficScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        async static Task MainAsync(string[] args)
        {
            var client = new HttpClient();
            var response = await client.GetAsync("https://schedules.sofiatraffic.bg/metro/1");
            var pageContents = await response.Content.ReadAsStringAsync();

            var pageDocument = new HtmlDocument();
            pageDocument.LoadHtml(pageContents);

            List<int> directions = new List<int>()
            {
                2666, //м.Витоша-м.Обеля-м.Летище София
                //2667, // м.Летище София-м.Обеля-м.Витоша
                //2668, //м.Витоша-м.Обеля-м.Бизнес Парк
                //2669  // м.Бизнес Парк-м.Обеля-м.Витоша
            };

            List<int> typesOfDay = new List<int>()
            {
                8451, // normal day
                6621 // holiday
            };

            var stationTimetables = new Dictionary<string, StationTimetableModel>();

            foreach (var direction in directions)
            {
                var directionSigns = pageDocument.DocumentNode.SelectNodes($"//div[@id='schedule_direction_8451_{direction}_content']//ul[@class='schedule_direction_signs']//li");
                foreach (var directionSign in directionSigns.Take(2))
                {
                    var directionId = directionSign.SelectSingleNode("a[@class='stop_link']").InnerText.Trim();
                    var stationName = directionSign.SelectSingleNode("a[@class='stop_change']").InnerText.Trim();

                    var requestUrl = $"https://schedules.sofiatraffic.bg/server/html/schedule_load/8451/{direction}/{directionId}";
                    var scheduleLoadResponse = await client.PostAsync(requestUrl, null);
                    var scheduleLoadContent = await scheduleLoadResponse.Content.ReadAsStringAsync();

                    var scheduleLoadDocument = new HtmlDocument();
                    scheduleLoadDocument.LoadHtml(scheduleLoadContent);

                    Console.WriteLine(scheduleLoadDocument.DocumentNode.InnerHtml);

                    var times = scheduleLoadDocument.DocumentNode.SelectNodes("(//div[@class='schedule_view_direction']//div[@class='schedule_times']//tbody//a)");

                    stationTimetables[stationName] = new StationTimetableModel()
                    {
                        Direction1 = new Direction()
                    };

                    foreach (var time in times)
                    {
                        if (!string.IsNullOrEmpty(time.InnerText) && time.InnerText != "&nbsp;")
                        {
                            stationTimetables[stationName].Direction1.Times.Add(time.InnerText.Trim());
                        }
                    }

                    Console.WriteLine("--------------------");
                    Thread.Sleep(3000);
                }

            }

            using (var writer = File.CreateText("timetable.json"))
            {
                writer.WriteLine(JsonConvert.SerializeObject(stationTimetables));
            }

            Console.ReadLine();
        }
    }
}
