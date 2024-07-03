namespace MedicalRecordsSystem.Models;

internal class Patient
{
    public required int Id { get; set; }

    public required string FirstName { get; set; }

    public required string LastName { get; set; }

    public required DateTime DateOfBirth { get; set; }

    public int Age => CalculateAge(DateOfBirth);

    public required string Nationality { get; set; }

    public List<MedicalRecord> MedicalRecords { get; set; } = [];

    private static int CalculateAge(DateTime dob)
    {
        var today = DateTime.Today;
        int years = today.Year - dob.Year;

        if (dob.Date > today.AddYears(-years)) years--;

        return years;
    }
}
