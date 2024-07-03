namespace MedicalRecordsSystem.Models;

internal class HospitalService(string name, string description, decimal price) : BaseEntity
{
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public decimal Price { get; set; } = price;
}
