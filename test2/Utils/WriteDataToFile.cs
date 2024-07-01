using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Utils
{
    internal class WriteDataToFile
    {
        public static void WriteData(CurrencyRatesResponse data, string filePath)
        {
            try
            {
                if (data.Rates is null)
                {
                    throw new Exception("Data does not contain currency rates!");
                }

                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    var amount = data.MainCurrency == "AMD" ? "1" : "";
                    foreach (KeyValuePair<string, decimal> kvp in data.Rates)
                    {

                        writer.WriteLine($"{amount}{kvp.Key} = {kvp.Value} {data.MainCurrency}");
                    }
                }

                Console.WriteLine("Data has been written to the file.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing data to {filePath}: {ex.Message}");
            }
        }
    }
}
