using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsSystem.Models
{
    internal class HospitalService
    {
        public int Id { get; set; } = 1;
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public HospitalService(string name, string description, decimal price)
        {
            Id++;
            Name = name;
            Description = description;
            Price = price;
        }
    }
}
