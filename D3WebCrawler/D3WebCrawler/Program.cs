using D3WebCrawler.Service;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace D3WebCrawler
{
    class Program
    {
        static async Task Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Ssl3;

            var urlSite = ConfigurationManager.AppSettings["URL_SITE"].ToString();

            if (string.IsNullOrEmpty(urlSite))
            {
                Console.WriteLine("URL do site não configurado.");

                return;
            }

            var crawlerService = new CrawlerService(urlSite);
            await crawlerService.StartCrawlerAync();

            Console.WriteLine("");
            Console.WriteLine("Precione enter para fechar o programa...");
            Console.ReadKey();
        }
    }
}
