using MedicalRecordsSystem.Models;
using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Services.HospitalServices;

internal class InvoiceManager()
{
    public static Invoice IssueInvoice(IEnumerable<MedicalRecord> records, string currency, CurrencyRatesResponse currencyRates)
    {
        decimal sumPrice = 0.0m;

        foreach (var record in records)
        {
            var servicePrice = HospitalServices.Services.FirstOrDefault(p => p.Key == record.ServiceName).Value.Price;
            sumPrice += servicePrice;
            record.IsPaid = true;
        }

        decimal rate = 1.0m;

        if (currencyRates.Rates is null)
        {
            throw new Exception("An error occurred while fetching currency rates. Please try again.");
        }

        foreach (KeyValuePair<string, decimal> kvp in currencyRates.Rates)
        {
            if (kvp.Key == currency)
            {
                rate = kvp.Value;
            }
        }

        var convertedPrice = Math.Round(sumPrice / rate, 2);
        PatientManager.WritePatientDataToFile();

        return new Invoice(DateTime.Now, currency, sumPrice, convertedPrice);
    }
}
