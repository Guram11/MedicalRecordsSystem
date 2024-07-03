using HtmlAgilityPack;
using System.Xml.Linq;
using MedicalRecordsSystem.Interfaces;
using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Services.CurrencyRetrievers;

public class GeorgianCurrencyRetriever(HttpClient httpClient) : ICurrencyRatesRetriever
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<CurrencyRatesResponse> RetrieveDataAsync(DateTime date)
    {
        string url = $"https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/rss?date={date}";

        HttpResponseMessage response = await _httpClient.GetAsync(url);
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
                        var currencyCode = cells[0].InnerText.Trim();
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
