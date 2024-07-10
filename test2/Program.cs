using MedicalRecordsSystem.Utils;
using Microsoft.Extensions.DependencyInjection;
using MedicalRecordsSystem.Services.CurrencyRetrievers;
using MedicalRecordsSystem.Services.HospitalServices;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace MedicalRecordsSystem
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddProvider(new FileLoggerProvider());
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddHostedService<CurrencyRatesService>()
                            .AddHttpClient();
                })
                .Start();

            PatientManager.GetAllPatients();

            // User Menu
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
                Console.WriteLine("8. Exit and stop background task");

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
                            MedicalServicesOptions.IssueInvoice();
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
                            // Stop the host to gracefully stop the background service
                            await host.StopAsync();
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
}