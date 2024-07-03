namespace MedicalRecordsSystem.models;

public class CurrencyRatesResponse
{
    public string? MainCurrency { get; set; }
    public Dictionary<string, decimal>? Rates { get; set; }
}
