using MedicalRecordsSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsSystem.Services.HospitalServices
{
    internal class PatientManager
    {
        private static Dictionary<int, Patient> patients = new Dictionary<int, Patient>();

        public static void AddPatient(Patient patient)
        {
            if (!patients.ContainsKey(patient.Id))
            {
                patients[patient.Id] = patient;
                Console.WriteLine("Patient added successfully.");
            }
            else
            {
                Console.WriteLine("Patient with this ID already exists.");
            }
        }

        public static void RemovePatient(int id)
        {
            if (patients.Remove(id))
            {
                Console.WriteLine("Patient removed successfully.");
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        public static void UpdatePatient(int id, Patient updatedPatient)
        {
            if (patients.ContainsKey(id))
            {
                patients[id] = updatedPatient;
                Console.WriteLine("Patient updated successfully.");
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        public static IEnumerable<Patient> GetAllPatients()
        {
            return patients.Values;
        }

        public static Patient? GetPatient(int id)
        {
            patients.TryGetValue(id, out var patient);

            return patient is null ? null : patient;
        }

        public static void AddMedicalRecord(int patientId, MedicalRecord record)
        {
            if (patients.TryGetValue(patientId, out var patient))
            {
                patient.MedicalRecords.Add(record);
                Console.WriteLine("Medical record added successfully.");
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }

        public static void RemoveMedicalRecord(int patientId, string serviceName)
        {
            if (patients.TryGetValue(patientId, out var patient))
            {
                var record = patient.MedicalRecords.FirstOrDefault(r => r.ServiceName == serviceName);
                if (record != null)
                {
                    patient.MedicalRecords.Remove(record);
                    Console.WriteLine("Medical record removed successfully.");
                }
                else
                {
                    Console.WriteLine("Medical record not found.");
                }
            }
            else
            {
                Console.WriteLine("Patient not found.");
            }
        }
    }
}
