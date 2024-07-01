using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsSystem.Models
{
    internal class MedicalRecord
    {
        public int Id { get; set; } = 1;
        public DateTime Date {  get; set; }
        public int PatientId { get; set; }
        public string? ServiceName { get; set; }
        bool IsPaid { get; set; }
        public MedicalRecord(DateTime date, int patientId, string serviceName)
        {
            Id++;
            Date = date;
            PatientId = patientId;
            ServiceName = serviceName;
            IsPaid = false;
        }
    }
}
