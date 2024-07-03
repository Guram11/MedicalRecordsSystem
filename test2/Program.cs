using MedicalRecordsSystem.Utils;
using Microsoft.Extensions.DependencyInjection;
using MedicalRecordsSystem.Services.CurrencyRetrievers;
using MedicalRecordsSystem.Services.HospitalServices;

namespace MedicalRecordsSystem;

internal class Program
{
    public static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
       .AddHttpClient()
       .AddTransient<GeorgianCurrencyRetriever>()
       .BuildServiceProvider();

        // Retrieve the IHttpClientFactory
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        var client = httpClientFactory.CreateClient();
        // Use the factory to create an HttpClient

        //var patientManager = new PatientManager();
        var invoiceManager = new InvoiceManager(client);

        PatientManager.GetAllPatients();

        while (true)
        {
            Console.WriteLine("Choose an operation:");
            Console.WriteLine("1. Add patient");
            Console.WriteLine("2. Add medical record for patient");
            Console.WriteLine("3. List of services");
            Console.WriteLine("4. Issue an invoice");
            Console.WriteLine("5. List all patients");
            Console.WriteLine("6. Delete a patient");
            Console.WriteLine("7. Delete a medical record");
            Console.WriteLine("8. Exit");

            var choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        PatientOptions.AddPatient();
                        break;
                    case "2":
                        MedicalServicesOptions.AddMedicalRecord();
                        break;
                    case "3":
                        MedicalServicesOptions.ListServices();
                        break;
                    case "4":
                        MedicalServicesOptions.IssueInvoice(invoiceManager);
                        break;
                    case "5":
                        PatientOptions.ListAllPatients();
                        break;
                    case "6":
                        PatientOptions.DeletePatient();
                        break;
                    case "7":
                        MedicalServicesOptions.DeleteMedicalRecord();
                        break;
                    case "8":
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }  
}