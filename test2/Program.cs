using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using MedicalRecordsSystem.Interfaces;
using MedicalRecordsSystem.models;
using MedicalRecordsSystem.Utils;
using Microsoft.Extensions.DependencyInjection;
using MedicalRecordsSystem.Models;
using MedicalRecordsSystem.Services.CurrencyRetrievers;
using MedicalRecordsSystem.Services.HospitalServices;

namespace MedicalRecordsSystem
{
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


            while (true)
            {
                Console.WriteLine("Choose an operation:");
                Console.WriteLine("1. Add patient");
                Console.WriteLine("2. Add medical record for patient");
                Console.WriteLine("3. Choose a service");
                Console.WriteLine("4. Issue an invoice");
                Console.WriteLine("5. List all patients");
                Console.WriteLine("6. Exit");


                var choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            UserMenuOptions.AddPatient();
                            break;
                        case "2":
                            UserMenuOptions.AddMedicalRecord();
                            break;
                        case "3":
                            UserMenuOptions.ListServices();
                            break;
                        case "4":
                            UserMenuOptions.IssueInvoice(invoiceManager);
                            break;
                        case "5":
                            UserMenuOptions.ListAllPatients();
                            break;
                        case "6":
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

        




        //DateTime date = DateTime.Now;

        //List<ICurrencyRatesRetriever> currencyRatesRetrievers = new List<ICurrencyRatesRetriever>()
        //{
        //    new GeorgianCurrencyRetriever(client),
        //    new AzerbaijanCurrencyRetriever(client),
        //    new ArmenianCurrencyRetriever(client),
        //};

        //List<string> filePaths = new List<string>()
        //{
        //    "georgianCurrency.txt",
        //    "azerbaijanCurrency.txt",
        //    "armeniaCurrency.txt",
        //};

        //    while (true)
        //    {
        //        DateTime date;
        //        Console.WriteLine("View currency rates for the following countries: 1. Georgia 2. Azerbaijan 3. Armenia 4. Exit");
        //        string? action = Console.ReadLine();

        //        switch (action)
        //        {
        //            case "1":
        //                date = GetDateInput.GetDateFromUser("yyyy-MM-dd");
        //                if (date == DateTime.MinValue) continue;
        //                var data1 = await currencyRatesRetrievers[0].RetrieveDataAsync(date);
        //                if (data1 != null) WriteDataToFile.WriteData(data1, filePaths[0]);
        //                break;

        //            case "2":
        //                date = GetDateInput.GetDateFromUser("dd.MM.yyyy");
        //                if (date == DateTime.MinValue) continue;
        //                var data2 = await currencyRatesRetrievers[1].RetrieveDataAsync(date);
        //                if (data2 != null) WriteDataToFile.WriteData(data2, filePaths[1]);
        //                break;


        //            case "3":
        //                date = GetDateInput.GetDateFromUser("yyyy-MM-dd");
        //                if (date == DateTime.MinValue) continue;
        //                var data3 = await currencyRatesRetrievers[2].RetrieveDataAsync(date);
        //                if (data3 != null) WriteDataToFile.WriteData(data3, filePaths[2]);
        //                break;

        //            case "4":
        //                Console.WriteLine("Exiting the application.");
        //                return;

        //            default:
        //                Console.WriteLine("Invalid option. Please try again.");
        //                continue;
        //        }
        //    }
        //}
    
    }
}