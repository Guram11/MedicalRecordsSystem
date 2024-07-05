using HtmlAgilityPack;
using MedicalRecordsSystem.Interfaces;
using MedicalRecordsSystem.models;
using MedicalRecordsSystem.Utils;
using Microsoft.Extensions.Logging;
using System.Xml.Linq;

namespace MedicalRecordsSystem.Services.CurrencyRetrievers;

internal class CurrencyRatesService(ILogger<CurrencyRatesService> logger, IHttpClientFactory httpClientFactory) : ICurrencyRatesRetriever
{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient();
    private readonly ILogger<CurrencyRatesService> _logger = logger;
    private CurrencyRatesResponse? _currentRates;
    private DateTime _lastFetchTime;

    public async Task<CurrencyRatesResponse> FetchInitialRatesAsync()
    {
        _logger.LogInformation("Fetching initial currency rates.");
        _currentRates = await FetchRatesAsync(DateTime.Now);

        _lastFetchTime = DateTime.UtcNow;
        return _currentRates;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            try
            {
                var now = DateTime.UtcNow;

                // If a minute has passed since the last fetch
                if (_currentRates != null && _lastFetchTime < now.AddMinutes(-1))
                {
                    WriteDataToFile.WriteCurrenciesToFile(_currentRates, _lastFetchTime);
                }

                _currentRates = await FetchRatesAsync(DateTime.Now);
                _lastFetchTime = now;
                _logger.LogInformation("Fetched new currency rates at {Time}.", _lastFetchTime);

                // Wait for the next minute
                var delay = TimeSpan.FromMinutes(1);

                _logger.LogInformation("Waiting for {Delay} until the next fetch.", delay);
                await Task.Delay(delay, cancellationToken);
            }
            catch (TaskCanceledException)
            {
                _logger.LogInformation("Task was canceled.");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching currency rates.");
            }
        }
    }

    public async Task<CurrencyRatesResponse> FetchRatesAsync(DateTime date)
    {
        string url = $"https://nbg.gov.ge/gw/api/ct/monetarypolicy/currencies/ka/rss?date={date}";

        var response = await _httpClient.GetAsync(url);

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
