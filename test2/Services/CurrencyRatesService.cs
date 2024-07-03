using HtmlAgilityPack;
using MedicalRecordsSystem.models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MedicalRecordsSystem.Services
{
    internal class CurrencyRatesService
    {
        private readonly ILogger<CurrencyRatesService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public CurrencyRatesService(ILogger<CurrencyRatesService> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await RetrieveDataAsync(DateTime.Now);
                await Task.Delay(TimeSpan.FromDays(1), stoppingToken); // Delay for one day
            }
        }

        public async Task<CurrencyRatesResponse> RetrieveDataAsync(DateTime date)
        {
            string url = $"https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/rss?date={date}";

            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync(url); // Replace with your URL

            response.EnsureSuccessStatusCode();
            string data = await response.Content.ReadAsStringAsync();

            return DeserializeXmlResponse(data);
        }

        public static CurrencyRatesResponse DeserializeXmlResponse(string data)
        {
            var currencyRates = new Dictionary<string, decimal>();

            var xDoc = XDocument.Parse(data);

            // Select the description element which contains the CDATA section
            var descriptionElement = xDoc?.Descendants("item")?.FirstOrDefault()?.Descendants("description").FirstOrDefault();

            if (descriptionElement != null)
            {
                var cdataContent = descriptionElement.Value;

                // Extract the content of the CDATA section
                var startIndex = cdataContent.IndexOf("<table");
                var endIndex = cdataContent.IndexOf("</table>") + "</table>".Length;

                if (startIndex >= 0 && endIndex > startIndex)
                {
                    var tableContent = cdataContent.Substring(startIndex, endIndex - startIndex);

                    // Load the table content as an XML document
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(tableContent);

                    // Iterate through each row in the table
                    foreach (var row in htmlDoc.DocumentNode.SelectNodes("//tr"))
                    {
                        var cells = row.SelectNodes("td").ToList();
                        if (cells.Count >= 3)
                        {
                            var currencyCode = cells[1].InnerText.Trim();
                            var rate = decimal.Parse(cells[2].InnerText.Trim());


                            currencyRates[currencyCode] = rate;
                        }
                    }
                }
            }

            return new CurrencyRatesResponse
            {
                MainCurrency = "GEL",
                Rates = currencyRates
            };
        }
    }
}
