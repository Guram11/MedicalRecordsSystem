using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Interfaces;

internal interface ICurrencyRatesRetriever
{
    Task<CurrencyRatesResponse> RetrieveDataAsync(DateTime date);
}
