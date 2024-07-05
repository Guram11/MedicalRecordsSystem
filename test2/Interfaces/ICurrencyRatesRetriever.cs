using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Interfaces;

internal interface ICurrencyRatesRetriever
{
    Task<CurrencyRatesResponse> FetchRatesAsync(DateTime date);
}
