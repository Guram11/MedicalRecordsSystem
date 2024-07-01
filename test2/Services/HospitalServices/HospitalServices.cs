using MedicalRecordsSystem.Models;

namespace MedicalRecordsSystem.Services.HospitalServices
{
    internal class HospitalServices
    {
        public static Dictionary<string, HospitalService> Services = new Dictionary<string, HospitalService>
        {
            { "BloodSample", new HospitalService("Blood Sample", "Collection and analysis of a blood sample", 50.00m) },
            { "Surgery", new HospitalService("Surgery", "Various surgical procedures", 1500.00m) },
            { "XRay", new HospitalService("X-Ray", "X-ray imaging for diagnostic purposes", 200.00m) },
            { "MRI", new HospitalService("MRI Scan", "Magnetic Resonance Imaging", 1000.00m) },
            { "CTScan", new HospitalService("CT Scan", "Computed Tomography scan", 850.00m) },
        };
    }
}