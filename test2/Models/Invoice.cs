using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsSystem.Models
{
    internal class Invoice
    {
        public int Id { get; set; } = 1;
        public int MedicalRecordId { get; set; }
        DateTime Date { get; set; }
        public string BaseCurrency { get; set; } = "GEL";
        public string? Currency { get; set; }
        public decimal BasePrice { get; set; }
        public decimal ConvertedPrice { get; set; }

        public Invoice(int medicalRecordId, DateTime date, string currency, decimal basePrice, decimal convertedPrice)
        {
            Id++;
            MedicalRecordId = medicalRecordId;
            Date = date;
            Currency = currency;
            BasePrice = basePrice;
            ConvertedPrice = convertedPrice;
        }
    }
}
