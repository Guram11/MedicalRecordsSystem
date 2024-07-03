namespace MedicalRecordsSystem.Models;

internal class MedicalRecord(DateTime date, int patientId, string serviceName) : BaseEntity()
{
    public DateTime Date { get; set; } = date;

    public int PatientId { get; set; } = patientId;

    public string ServiceName { get; set; } = serviceName;

    public bool IsPaid { get; set; } = false;
}
