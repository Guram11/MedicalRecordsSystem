using MedicalRecordsSystem.Utils;
using Microsoft.Extensions.DependencyInjection;
using MedicalRecordsSystem.Services.CurrencyRetrievers;
using MedicalRecordsSystem.Services.HospitalServices;
using MedicalRecordsSystem.models;
using Microsoft.Extensions.Logging;

namespace MedicalRecordsSystem;

internal class Program
{
    private static CurrencyRatesResponse _georgianCurrencyRates = new();
    public static async Task Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
            .AddHostedService<CurrencyRatesService>()
            .AddSingleton<ILoggerProvider, FileLoggerProvider>()
            .AddSingleton<CurrencyRatesService>()
            .AddHttpClient()
            .BuildServiceProvider();

        var fetcher = serviceProvider.GetRequiredService<CurrencyRatesService>();
        var cts = new CancellationTokenSource();

        // Fetch initial currency rates
        _georgianCurrencyRates = await fetcher.FetchInitialRatesAsync();

        // Start the background task
        var fetcherTask = fetcher.StartAsync(cts.Token);

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
                        MedicalServicesOptions.IssueInvoice(_georgianCurrencyRates);
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
                        // Signal the cancellation and wait for the task to complete
                        cts.Cancel();
                        await fetcherTask;
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