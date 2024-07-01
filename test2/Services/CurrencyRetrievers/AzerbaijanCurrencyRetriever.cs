using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using MedicalRecordsSystem.Interfaces;
using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Services.CurrencyRetrievers
{
    [XmlRoot("ValCurs")]
    public class ValCurs
    {
        [XmlElement("ValType")]
        public List<ValType>? ValTypes { get; set; }
    }

    public class ValType
    {
        [XmlElement("Valute")]
        public List<Valute>? Valutes { get; set; }
    }

    public class Valute
    {
        [XmlElement("Name")]
        public string? Name { get; set; }

        [XmlElement("Value")]
        public decimal Value { get; set; }
    }

    internal class AzerbaijanCurrencyRetriever : ICurrencyRatesRetriever
    {
        private readonly HttpClient _httpClient;

        public AzerbaijanCurrencyRetriever(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CurrencyRatesResponse> RetrieveDataAsync(DateTime date)
        {
            string url = $"https://www.cbar.az/currencies/{date.ToString("dd.MM.yyyy")}.xml";

            HttpResponseMessage response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string data = await response.Content.ReadAsStringAsync();

            return DeserializeXmlResponse(data);
        }

        private static CurrencyRatesResponse DeserializeXmlResponse(string data)
        {
            ValCurs valCurs;
            XmlSerializer serializer = new XmlSerializer(typeof(ValCurs));

            using (StringReader reader = new StringReader(data))
            {
                valCurs = (ValCurs)serializer.Deserialize(reader);
            }

            // Create dictionary and populate with currency names and rates
            Dictionary<string, decimal> currencyRates = new Dictionary<string, decimal>();
            foreach (var valType in valCurs.ValTypes)
            {
                foreach (var valute in valType.Valutes)
                {
                    currencyRates[valute.Name] = valute.Value;
                }
            }

            return new CurrencyRatesResponse
            {
                MainCurrency = "AZB",
                Rates = currencyRates
            };
        }
    }
}
