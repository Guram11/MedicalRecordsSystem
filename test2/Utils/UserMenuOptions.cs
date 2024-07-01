using MedicalRecordsSystem.Models;
using MedicalRecordsSystem.Services.HospitalServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsSystem.Utils
{
    internal class UserMenuOptions
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
                firstName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(firstName))
                    break;
                Console.WriteLine("First name cannot be empty. Please try again.");
            }

            while (true)
            {
                Console.WriteLine("Enter last name:");
                lastName = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(lastName))
                    break;
                Console.WriteLine("Last name cannot be empty. Please try again.");
            }

            while (true)
            {
                Console.WriteLine("Enter date of birth (yyyy-MM-dd):");
                string? dobInput = Console.ReadLine();
                if (DateTime.TryParseExact(dobInput, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dob))
                    break;
                Console.WriteLine("Invalid date format. Please enter the date in yyyy-MM-dd format.");
            }

            while (true)
            {
                Console.WriteLine("Enter ID number:");
                string? idInput = Console.ReadLine();
                if (int.TryParse(idInput, out id))
                    break;
                Console.WriteLine("Invalid ID number. Please enter a valid integer.");
            }

            while (true)
            {
                Console.WriteLine("Enter nationality:");
                nationality = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(nationality))
                    break;
                Console.WriteLine("Nationality cannot be empty. Please try again.");
            }

            try
            {
                var patient = new Patient(firstName, lastName, dob, id, nationality);
                PatientManager.AddPatient(patient);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the patient: {ex.Message}");
            }
        }
    

        public static void ListAllPatients()
        {
            var patients = PatientManager.GetAllPatients();

            if (patients.Count() == 0)
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
                Console.WriteLine("Medical Records:");

                foreach (var record in patient.MedicalRecords)
                {
                    Console.WriteLine($"  - Record Date: {record.Date:yyyy-MM-dd}");
                    Console.WriteLine($"  - Service ID: {record.ServiceName}");
                }

                Console.WriteLine();
            }
        }

        public static void AddMedicalRecord()
        {
            int patientId;
            string? serviceName;

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
                serviceName = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(serviceName) && HospitalServices.Services.ContainsKey(serviceName))
                {
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

        public static async void IssueInvoice(InvoiceManager invoiceManager)
        {
            int patientId;

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

            Console.WriteLine("Enter currency (e.g., USD, EUR):");
            var currency = Console.ReadLine();

            var patient = PatientManager.GetPatient(patientId);
            var records = patient?.MedicalRecords;

            if (records != null)
            {
                var invoice = await invoiceManager.IssueInvoice(records, currency);
                Console.WriteLine($"Invoice issued for {patient.FirstName} {patient.LastName}");
                Console.WriteLine($"Base Price (GEL): {invoice.BasePrice}");
                Console.WriteLine($"Currency Price ({invoice.Currency}): {invoice.ConvertedPrice}");
            }
            else
            {
                Console.WriteLine("Medical record not found.");
            }
        }
    }
}
