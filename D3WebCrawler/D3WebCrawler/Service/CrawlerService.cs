using D3WebCrawler.Model;
using D3WebCrawler.Model.Enum;
using HtmlAgilityPack;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace D3WebCrawler.Service
{
    public class CrawlerService
    {
        private readonly Uri baseUrlSite;

        public CrawlerService(string urlSite)
        {
            baseUrlSite = new Uri(urlSite);
        }

        public async Task StartCrawlerAync()
        {
            try
            {
                var html = await GetContent(baseUrlSite);

                if (string.IsNullOrEmpty(html))
                {
                    Console.WriteLine("Não foi possível ler o conteúdo do site.");

                    return;
                }

                //OBJETO COM AS INFORMAÇÕES DAS PÁGINAS
                var paginas = new List<Pagina>();

                HtmlDocument htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(html);

                //LINKS DAS PÁGINAS
                var pageList = new ConcurrentBag<string>() { baseUrlSite.ToString() };

                //CAREGA AS INFORMAÇÕES DO LINK BASE
                var pagina = GetPageDatails(htmlDoc, pageList);
                pagina.URL = baseUrlSite.ToString();
                paginas.Add(pagina);

                #region CARREGA INFORMAÕES DAS PÁGINAS
                var loadedPages = new List<string>() { baseUrlSite.ToString() };

                while (pageList.Any(x => !loadedPages.Contains(x)))
                {
                    var page = pageList.FirstOrDefault(x => !loadedPages.Contains(x));

                    try
                    {
                        var htmlPage = await GetContent(new Uri(page));

                        if (string.IsNullOrEmpty(html))
                        {
                            Console.WriteLine("Não foi possível ler o conteúdo do site.");

                            return;
                        }

                        htmlDoc = new HtmlDocument();
                        htmlDoc.LoadHtml(htmlPage);

                        pagina = GetPageDatails(htmlDoc, pageList);
                        pagina.URL = page;
                        paginas.Add(pagina);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Erro ao carregar página: " + ex.Message);
                        Console.WriteLine("Págima: " + page);
                    }

                    loadedPages.Add(page);
                }
                #endregion

                #region IMPRIMIR AS INFORMAÇÕES DA PÁGINA NO CONSOLE
                if (paginas.Any())
                {
                    Console.WriteLine("");

                    foreach (var item in paginas)
                    {
                        Console.WriteLine("========================================================================================");
                        Console.WriteLine("PÁGINA: {0} ", item.Nome);
                        Console.WriteLine("URL: {0}", item.URL);
                        Console.WriteLine("----------------------------------------------------------------------------------------");

                        if (item.Componentes.Any())
                        {
                            foreach (var componet in item.Componentes.OrderBy(x => x.Tipo))
                            {
                                Console.WriteLine("COMPONENTE: \"{0}\" | NOME: \"{1}\" | URL: \"{2}\"", componet.Tipo, componet.Nome, componet.URL);
                            }
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ocorreu um erro interno.");
                Console.WriteLine("Erro: " + ex.Message);

                if (ex.InnerException != null)
                {
                    Console.WriteLine("Erro InnerException: " + ex.InnerException.Message);
                }
            }
        }

        private Pagina GetPageDatails(HtmlDocument htmlDoc, ConcurrentBag<string> pageList)
        {
            #region CARREGA TODOS OS LINKS DA PÁGINA
            if (htmlDoc.DocumentNode.SelectNodes("//a[@href]") != null)
            {
                var hrefTags = htmlDoc.DocumentNode.SelectNodes("//a[@href]")
                    .Where(x => x.Attributes["href"].Value.Contains(baseUrlSite.ToString()) || (!x.Attributes["href"].Value.Contains(baseUrlSite.ToString()) && x.Attributes["href"].Value.StartsWith("/"))).ToList();

                if (hrefTags != null && hrefTags.Any())
                {
                    Parallel.ForEach(hrefTags, new ParallelOptions { MaxDegreeOfParallelism = 4 }, data =>
                    {
                        var url = data.Attributes["href"].Value;

                        if (url.Length <= 1)
                        {
                            return;
                        }

                        if (!url.Contains(baseUrlSite.ToString()))
                        {
                            url = new Uri(baseUrlSite, url).ToString();
                        }

                        //CASO TENHA ENCONTRADO UMA URL NOVA QUE NÃO EXISTE NA LISTA, ADICIONAMOS A MASMA PARA CARREGAR SUAS INFORMAÇÕES
                        if (!pageList.Contains(url))
                        {
                            pageList.Add(url);

                            Console.WriteLine("Link novo encontrado: " + url);
                        }
                    });
                }
            }
            #endregion

            #region CARREGA INFORMAÇÕES DA PÁGINA
            var pagina = new Pagina();

            if (htmlDoc.DocumentNode.SelectNodes("//head/title") != null)
            {
                pagina.Nome = htmlDoc.DocumentNode.SelectSingleNode("//head/title").InnerText;
            }

            #region CARREGA TODAS AS IMAGENS DA PÁGINA
            if (htmlDoc.DocumentNode.SelectNodes("//img[@src]") != null)
            {
                foreach (var img in htmlDoc.DocumentNode.SelectNodes("//img[@src]"))
                {
                    pagina.Componentes.Add(new Componente()
                    {
                        Tipo = TipoComponenteEnum.IMG,
                        URL = img.Attributes["src"].Value,
                        Nome = img.Attributes["alt"].Value ?? "Imagem sem alt"
                    });
                }
            }
            #endregion

            #region CARREGA TODAS OS CSS DA PÁGINA
            if (htmlDoc.DocumentNode.SelectNodes("//link[@rel='stylesheet']") != null)
            {
                foreach (var img in htmlDoc.DocumentNode.SelectNodes("//link[@rel='stylesheet']"))
                {
                    pagina.Componentes.Add(new Componente()
                    {
                        Tipo = TipoComponenteEnum.CSS,
                        URL = img.Attributes["href"].Value,
                        Nome = "Arquivo CSS"
                    });
                }
            }
            #endregion

            #region CARREGA TODOS OS JS DA PÁGINA
            if (htmlDoc.DocumentNode.SelectNodes("//script[@src]") != null)
            {
                foreach (var img in htmlDoc.DocumentNode.SelectNodes("//script[@src]"))
                {
                    pagina.Componentes.Add(new Componente()
                    {
                        Tipo = TipoComponenteEnum.JS,
                        URL = img.Attributes["src"].Value,
                        Nome = "Arquivo JS"
                    });
                }
            }
            #endregion

            return pagina;
            #endregion
        }

        private async Task<string> GetContent(Uri urlSite)
        {
            var httpClient = new HttpClient();
            return await httpClient.GetStringAsync(urlSite);
        }
    }
}
