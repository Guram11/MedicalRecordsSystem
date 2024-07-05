using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Utils;

internal class WriteDataToFile
{
    public static void WriteCurrenciesToFile(CurrencyRatesResponse data, DateTime date)
    {
        try
        {
            if (data.Rates is null)
            {
                throw new Exception("Data does not contain currency rates!");
            }

            var filePath = $"currency-rates-{date:yyyy-MM-dd-h-m-s}.txt";

            using StreamWriter writer = new(filePath);

            var amount = data.MainCurrency == "AMD" ? "1" : "";
            foreach (KeyValuePair<string, decimal> kvp in data.Rates)
            {

                writer.WriteLine($"{amount}{kvp.Key} = {kvp.Value} {data.MainCurrency}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error writing data to file: {ex.Message}");
        }
    }
}
