namespace MedicalRecordsSystem.Models;

internal class Invoice(DateTime date, string currency, decimal basePrice, decimal convertedPrice) : BaseEntity
{
    public DateTime Date { get; set; } = date;
    public string BaseCurrency { get; set; } = "GEL";
    public string Currency { get; set; } = currency;
    public decimal BasePrice { get; set; } = basePrice;
    public decimal ConvertedPrice { get; set; } = convertedPrice;
}
