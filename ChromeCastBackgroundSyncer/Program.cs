using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace ChromeCastBackgroundSyncer
{
    static class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var settings = new Settings();
            configuration.Bind(settings);

            var backgrounds = await GetBackgroundsInformation(settings);
            Directory.CreateDirectory(settings.Folder);
            DownloadBackgrounds(backgrounds, settings);
            Console.ReadLine();
        }

        private static async Task<IEnumerable<Background>> GetBackgroundsInformation(Settings settings)
        {
            IEnumerable<Background> backgrounds;
            using (var httpClient = new HttpClient())
            {
                var json = await httpClient.GetStringAsync(settings.JsonUrl);

                backgrounds = JsonConvert.DeserializeObject<List<Background>>(json);
                Console.WriteLine("JSON downloaded");
            }
            return backgrounds;
        }

        private static void DownloadBackgrounds(IEnumerable<Background> backgrounds, Settings settings)
        {
            foreach (var background in backgrounds)
            {
                var uri = new Uri(background.Url);

                string filename = Path.GetFileName(uri.Segments.Last());
                string filePath = Path.Combine(settings.Folder, filename);

                if (File.Exists(filePath))
                {
                    Console.WriteLine($"{filePath} already exists.");
                    continue;
                }

                using (var webClient = new WebClient())
                {
                    webClient.DownloadFileAsync(uri, filePath);
                    webClient.DownloadFileCompleted += (sender, eventArgs) => Console.WriteLine($"{filePath} downloaded.");
                }
            }
        }
    }

    public class Settings
    {
        public string JsonUrl { get; set; }

        public string Folder { get; set; }
    }
}
