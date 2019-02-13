using HtmlAgilityPack;
using System;
using System.Net.Http;
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

            var times = pageDocument.DocumentNode.SelectNodes("(//div[@class='schedule_view_direction']//div[@class='schedule_times']//tbody//a)");
            foreach (var time in times)
            {
                Console.WriteLine(time.InnerText);
            }

            Console.ReadLine();
        }
    }
}
