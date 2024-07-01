using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;
using MedicalRecordsSystem.models;
using MedicalRecordsSystem.Interfaces;

namespace MedicalRecordsSystem.Services.CurrencyRetrievers
{
    internal class ArmenianCurrencyRetriever : ICurrencyRatesRetriever
    {
        private readonly HttpClient _httpClient;

        public ArmenianCurrencyRetriever(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<CurrencyRatesResponse> RetrieveDataAsync(DateTime date)
        {
            string url = "http://api.cba.am/exchangerates.asmx";
            string soapAction = "http://www.cba.am/ExchangeRatesByDate";
            var soapEnvelopeXml = CreateSoapEnvelope(date);

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(soapEnvelopeXml.ToString(), Encoding.UTF8, "text/xml")
            };
            request.Headers.Add("SOAPAction", soapAction);

            HttpResponseMessage response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            string data = await response.Content.ReadAsStringAsync();

            return DeserializeSoapResponse(data);
        }

        private static XDocument CreateSoapEnvelope(DateTime date)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");

            XNamespace soapEnv = "http://schemas.xmlsoap.org/soap/envelope/";
            XNamespace cba = "http://www.cba.am/";

            var soapEnvelope = new XDocument(
                new XElement(soapEnv + "Envelope",
                    new XAttribute(XNamespace.Xmlns + "soap", soapEnv),
                    new XAttribute(XNamespace.Xmlns + "cba", cba),
                    new XElement(soapEnv + "Body",
                        new XElement(cba + "ExchangeRatesByDate",
                            new XElement(cba + "date", formattedDate)
                        )
                    )
                )
            );

            return soapEnvelope;
        }

        private static CurrencyRatesResponse DeserializeSoapResponse(string data)
        {
            var xdoc = XDocument.Parse(data);
            XNamespace ns = "http://www.cba.am/";
            var rates = new Dictionary<string, decimal>();

            foreach (var rate in xdoc.Descendants(ns + "ExchangeRate"))
            {
                string? iso = rate.Element(ns + "ISO")?.Value;
                decimal value = decimal.Parse(rate.Element(ns + "Rate")?.Value);
                rates.Add(iso, value);
            }

            return new CurrencyRatesResponse
            {
                MainCurrency = "AMD",
                Rates = rates
            };
        }
    }
}
