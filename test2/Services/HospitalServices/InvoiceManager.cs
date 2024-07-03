using MedicalRecordsSystem.Models;
using MedicalRecordsSystem.Services.CurrencyRetrievers;
using MedicalRecordsSystem.models;
using MedicalRecordsSystem.Utils;

namespace MedicalRecordsSystem.Services.HospitalServices;

internal class InvoiceManager(HttpClient httpClient)
{
    private readonly HttpClient _httpClient = httpClient;

    public async Task<Invoice> IssueInvoice(IEnumerable<MedicalRecord> records, string currency)
    {
        CurrencyRatesResponse currencyRates = await new GeorgianCurrencyRetriever(_httpClient).RetrieveDataAsync(DateTime.Now);
        WriteDataToFile.WriteCurrenciesToFile(currencyRates, "rates.txt");

        decimal sumPrice = 0.0m;

        foreach (var record in records)
        {
            var servicePrice = HospitalServices.Services.FirstOrDefault(p => p.Key == record.ServiceName).Value.Price;
            sumPrice += servicePrice;
            record.IsPaid = true;
        }

        string currencyNameInGeorgian = String.Empty;
        if(currency == "USD")
        {
            currencyNameInGeorgian = "1 აშშ დოლარი";
        }
        else if (currency == "EUR")
        {
            currencyNameInGeorgian = "1 ევრო";
        }
        else
        {
            currencyNameInGeorgian = "GEL";
        }

        decimal rate = 1.0m;

        if (currencyRates.Rates is null)
        {
            throw new Exception("An error occurred while fetching currency rates. Please try again.");
        }

        foreach (KeyValuePair<string, decimal> kvp in currencyRates.Rates)
        {
            if (kvp.Key == currencyNameInGeorgian)
            {
                rate = kvp.Value;
            }
        }

        var convertedPrice = Math.Round(sumPrice / rate, 2);
        PatientManager.WritePatientDataToFile();

        return new Invoice(DateTime.Now, currency, sumPrice, convertedPrice);
    }
}
