using MedicalRecordsSystem.models;
using MedicalRecordsSystem.Models;
using MedicalRecordsSystem.Services.HospitalServices;

namespace MedicalRecordsSystem.Utils;

internal static class MedicalServicesOptions
{
    public static void AddMedicalRecord()
    {
        int patientId;
        string serviceName;

        while (true)
        {
            Console.WriteLine("Enter patient ID:");
            string? patientIdInput = Console.ReadLine();

            if (int.TryParse(patientIdInput, out patientId))
            {
                if (PatientManager.GetPatient(patientId) != null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Patient ID does not exist. Please enter a valid patient ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID number. Please enter a valid integer.");
            }
        }

        while (true)
        {
            Console.WriteLine("Enter service name:");
            var input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input) && HospitalServices.Services.ContainsKey(input))
            {
                serviceName = input;
                break;
            }
            else
            {
                Console.WriteLine("Invalid service name. Please enter a valid service name.");
            }
        }

        try
        {
            var record = new MedicalRecord(DateTime.Now, patientId, serviceName);

            PatientManager.AddMedicalRecord(patientId, record);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while adding the medical record: {ex.Message}");
        }
    }

    public static void DeleteMedicalRecord()
    {
        int patientId;
        Guid serviceId;

        while (true)
        {
            Console.WriteLine("Enter patient ID number:");
            var input = Console.ReadLine();
            if (int.TryParse(input, out patientId))
                break;
            Console.WriteLine("Invalid ID number. Please enter a valid integer.");
        }

        while (true)
        {
            Console.WriteLine("Enter service ID number:");
            var input = Console.ReadLine();
            if (Guid.TryParse(input, out serviceId))
                break;
            Console.WriteLine("Invalid ID number. Please enter a valid integer.");
        }

        try
        {
            PatientManager.RemoveMedicalRecord(patientId, serviceId);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while deleting the medical record: {ex.Message}");
        }
    }

    public static void ListServices()
    {
        foreach (var service in HospitalServices.Services)
        {
            Console.WriteLine($"Service ID: {service.Key}");
            Console.WriteLine($"Name: {service.Value.Name}");
            Console.WriteLine($"Description: {service.Value.Description}");
            Console.WriteLine($"Price (GEL): {service.Value.Price}");
            Console.WriteLine();
        }
    }

    public static void IssueInvoice(CurrencyRatesResponse currencyRates)
    {
        int patientId;
        string currency;

        while (true)
        {
            Console.WriteLine("Enter patient ID:");
            string? patientIdInput = Console.ReadLine();

            if (int.TryParse(patientIdInput, out patientId))
            {
                if (PatientManager.GetPatient(patientId) != null)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Patient ID does not exist. Please enter a valid patient ID.");
                }
            }
            else
            {
                Console.WriteLine("Invalid ID number. Please enter a valid integer.");
            }
        }

        while (true)
        {
            Console.WriteLine("Enter currency (e.g., USD, EUR):");
            var input = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(input))
            {
                currency = input;
                break;
            }
            Console.WriteLine("Currency cannot be empty. Please try again.");
        }

        var patient = PatientManager.GetPatient(patientId);
        if (patient is null) return;

        var records = patient.MedicalRecords.Where(x => x.IsPaid == false);

        if (!records.Any())
        {
            Console.WriteLine("Every service has been paid for.");
            Console.WriteLine();
            return;
        }

        if (records != null)
        {
            var invoice = InvoiceManager.IssueInvoice(records, currency, currencyRates);
            Console.WriteLine($"Invoice issued for {patient.FirstName} {patient.LastName}");
            Console.WriteLine($"Base Price (GEL): {invoice.BasePrice}");
            Console.WriteLine($"Converted Price ({invoice.Currency}): {invoice.ConvertedPrice}");
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("Medical record not found.");
        }
    }
}
