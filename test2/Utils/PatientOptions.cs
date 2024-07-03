using MedicalRecordsSystem.Models;
using MedicalRecordsSystem.Services.HospitalServices;

namespace MedicalRecordsSystem.Utils;

internal class PatientOptions
{
    public static void AddPatient()
    {
        string firstName;
        string lastName;
        DateTime dob;
        int id;
        string nationality;

        while (true)
        {
            Console.WriteLine("Enter first name:");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                firstName = input;
                break;
            }
            Console.WriteLine("First name cannot be empty. Please try again.");
        }

        while (true)
        {
            Console.WriteLine("Enter last name:");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                lastName = input;
                break;
            }
            Console.WriteLine("Last name cannot be empty. Please try again.");
        }

        while (true)
        {
            Console.WriteLine("Enter date of birth (yyyy-MM-dd):");
            var input = Console.ReadLine();
            if (DateTime.TryParseExact(input, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dob))
                break;
            Console.WriteLine("Invalid date format. Please enter the date in yyyy-MM-dd format.");
        }

        while (true)
        {
            Console.WriteLine("Enter ID number:");
            var input = Console.ReadLine();
            if (int.TryParse(input, out id))
                break;
            Console.WriteLine("Invalid ID number. Please enter a valid integer.");
        }

        while (true)
        {
            Console.WriteLine("Enter nationality:");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                nationality = input;
                break;
            }
            Console.WriteLine("Nationality cannot be empty. Please try again.");
        }

        try
        {
            var patient = new Patient() { FirstName = firstName, LastName = lastName, Nationality = nationality, DateOfBirth = dob, Id = id};
            PatientManager.AddPatient(patient);
            PatientManager.WritePatientDataToFile();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while adding the patient: {ex.Message}");
        }
    }

    public static void DeletePatient()
    {
        try
        {
            int id;

            while (true)
            {
                Console.WriteLine("Enter ID number:");
                var input = Console.ReadLine();
                if (int.TryParse(input, out id))
                    break;
                Console.WriteLine("Invalid ID number. Please enter a valid integer.");
            }

            PatientManager.RemovePatient(id);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the patient: {ex.Message}");
        }
    }

    public static void ListAllPatients()
    {
        var patients = PatientManager.GetAllPatients();

        if (!patients.Any())
        {
            Console.WriteLine("No patients found!");
            return;
        }

        foreach (var patient in patients)
        {
            Console.WriteLine($"First Name: {patient.FirstName}");
            Console.WriteLine($"Last Name: {patient.LastName}");
            Console.WriteLine($"Date of Birth: {patient.DateOfBirth:yyyy-MM-dd}");
            Console.WriteLine($"ID Number: {patient.Id}");
            Console.WriteLine($"Nationality: {patient.Nationality}");
            Console.WriteLine($"Age: {patient.Age}");
            Console.WriteLine("Medical Services:");

            foreach (var record in patient.MedicalRecords)
            {
                Console.WriteLine($"  - Record Date: {record.Date:yyyy-MM-dd}");
                Console.WriteLine($"  - Service Name: {record.ServiceName}");
                Console.WriteLine($"  - Is Paid: {record.IsPaid}");
                Console.WriteLine($"  - ID: {record.Id}                       ");
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
