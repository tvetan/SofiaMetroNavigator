using System.Threading.Tasks;

namespace SofiaMetroStationsScraper
{
    class Program
    {
        static void Main(string[] args)
        {
            MainAsync(args).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        async static Task MainAsync(string[] args)
        {
            var logPath = System.IO.Path.GetTempFileName();
        }
    }
}
