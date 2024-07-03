namespace MedicalRecordsSystem.Models;

internal class Patient
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public int Age => CalculateAge(DateOfBirth);

    public string Nationality { get; set; }

    public List<MedicalRecord> MedicalRecords { get; set; } = [];

    public Patient()
    {
        MedicalRecords = [];
    }

    public Patient(string firstName, string lastName, DateTime dob, int idNumber, string nationality)
    {
        Id = idNumber;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dob;
        Nationality = nationality;
    }

    private static int CalculateAge(DateTime dob)
    {
        var today = DateTime.Today;
        int years = today.Year - dob.Year;

        if (dob.Date > today.AddYears(-years)) years--;

        return years;
    }
}
