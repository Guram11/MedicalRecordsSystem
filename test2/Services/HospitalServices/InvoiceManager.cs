using MedicalRecordsSystem.Models;
using MedicalRecordsSystem.Services.CurrencyRetrievers;
using MedicalRecordsSystem.models;
using MedicalRecordsSystem.Utils;
using System.Linq;

namespace MedicalRecordsSystem.Services.HospitalServices
{
    internal class InvoiceManager
    {
        private readonly HttpClient _httpClient;
        public InvoiceManager(HttpClient httpClient) {
           _httpClient = httpClient;
        }

        public async Task<Invoice> IssueInvoice(List<MedicalRecord> records, string currency)
        {
            CurrencyRatesResponse currencyRates = await new GeorgianCurrencyRetriever(_httpClient).RetrieveDataAsync(DateTime.Now);

            WriteDataToFile.WriteData(currencyRates, "rates.txt");

            decimal sumPrice = 0.0m;

            foreach (var record in records)
            {
                var servicePrice = HospitalServices.Services.FirstOrDefault(p => p.Key == record.ServiceName).Value.Price;
                sumPrice += servicePrice;
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
                Console.WriteLine("Something");
            }

            decimal rate = 1.0m;
            foreach (KeyValuePair<string, decimal> kvp in currencyRates.Rates)
            {
                if (kvp.Key == currencyNameInGeorgian)
                {
                    rate = kvp.Value;
                }
            }

            var convertedPrice = sumPrice / rate;

            return new Invoice(records.FirstOrDefault().PatientId, DateTime.Now, currency, sumPrice, convertedPrice);
        }
    }
}
