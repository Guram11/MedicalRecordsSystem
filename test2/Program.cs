using MedicalRecordsSystem.Utils;
using Microsoft.Extensions.DependencyInjection;
using MedicalRecordsSystem.Services.CurrencyRetrievers;
using MedicalRecordsSystem.Services.HospitalServices;
using MedicalRecordsSystem.models;
using MedicalRecordsSystem.Interfaces;

namespace MedicalRecordsSystem;

internal class Program
{
    private static CurrencyRatesResponse _georgianCurrencyRates = new();
    public static void Main(string[] args)
    {
        var serviceProvider = new ServiceCollection()
       .AddHttpClient()
       .BuildServiceProvider();

        // Retrieve the IHttpClientFactory and ILogger
        var httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

        // Use the factory to create an HttpClient
        var client = httpClientFactory.CreateClient();

        List<ICurrencyRatesRetriever> currencyRatesRetrievers =
        [
            new GeorgianCurrencyRetriever(client),
            new AzerbaijanCurrencyRetriever(client),
            new ArmenianCurrencyRetriever(client),
        ];

        List<string> filePaths =
        [
            "georgianCurrency.txt",
            "azerbaijanCurrency.txt",
            "armeniaCurrency.txt",
        ];

        List<Thread> threads = [];

        for (int i = 0; i < currencyRatesRetrievers.Count; i++)
        {
            int index = i;
            Thread thread = new(() => FetchCurrencyRates(currencyRatesRetrievers[index], filePaths[index]));
            threads.Add(thread);
            thread.Start();
        }

        // Wait for all threads to complete
        foreach (var thread in threads)
        {
            thread.Join();
        }

        Console.WriteLine("All currency rates have been fetched.");
        Console.WriteLine();

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

    public static void FetchCurrencyRates(ICurrencyRatesRetriever retriever, string filePath)
    {
        Console.WriteLine($"Thread started for {retriever.GetType().Name}");

        try
        {
            CurrencyRatesResponse data = retriever.RetrieveDataAsync(DateTime.Now).Result;
            if (data != null )
            {
                WriteDataToFile.WriteCurrenciesToFile(data, filePath);
                Console.WriteLine($"Thread for {retriever.GetType().Name} fetched data successfully.");

                if (retriever is GeorgianCurrencyRetriever)
                {
                    _georgianCurrencyRates = data;
                }
            }
            else
            {
                Console.WriteLine($"Thread for {retriever.GetType().Name} did not fetch any data.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred in thread for {retriever.GetType().Name}: {ex.Message}");
        }


        Console.WriteLine($"Thread ended for {retriever.GetType().Name}");
        Console.WriteLine();
    }
}