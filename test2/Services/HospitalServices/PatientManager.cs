using MedicalRecordsSystem.Models;
using System.Text.Json;

namespace MedicalRecordsSystem.Services.HospitalServices;

internal class PatientManager
{
    private readonly static Dictionary<int, Patient> patients = [];

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
            return;
        }
    }

    public static void RemovePatient(int id)
    {
        if (patients.Remove(id))
        {
            WritePatientDataToFile();
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
        string jsonString = File.ReadAllText("patients.json");
        var options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };

        using (FileStream fs = new("patients.json", FileMode.Open, FileAccess.Read))
        {
            var patientsList = JsonSerializer.Deserialize<List<Patient>>(fs, options) ?? [];
            foreach (var p in patientsList)
            {
                patients[p.Id] = p;
            }
        }

        return patients.Values;
    }

    public static Patient? GetPatient(int id)
    {
        patients.TryGetValue(id, out var patient);

        return patient is null ? null : patient;
    }

    public static void WritePatientDataToFile()
    {
        try
        {
            var options = new JsonSerializerOptions
            {
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            };

            var data = patients.Values.ToList();

            using FileStream fs = new("patients.json", FileMode.Create, FileAccess.Write);
            using Utf8JsonWriter writer = new(fs, new JsonWriterOptions { Indented = true });

            JsonSerializer.Serialize(writer, data, options);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    public static void AddMedicalRecord(int patientId, MedicalRecord record)
    {
        if (patients.TryGetValue(patientId, out var patient))
        {
            patient.MedicalRecords.Add(record);
            WritePatientDataToFile();
            Console.WriteLine("Medical record added successfully.");
        }
        else
        {
            Console.WriteLine("Patient not found.");
        }
    }

    public static MedicalRecord? GetMedicalRecord(int patientId, Guid recordId)
    {
        var patient = GetPatient(patientId);

        if (patient is null) return null;

        var record = patient.MedicalRecords.Where(x => x.Id == recordId).First();

        if (record is null) return null;

        return record;
    }

    public static void RemoveMedicalRecord(int patientId, Guid serviceId)
    {
        var patient = GetPatient(patientId);
        var record = GetMedicalRecord(patientId, serviceId);

        if (record != null && patient != null)
        {
            patient.MedicalRecords.Remove(record);
            WritePatientDataToFile();
            Console.WriteLine("Medical record removed successfully.");
        }
        else
        {
            Console.WriteLine("Medical record not found.");
        }
    }
}
