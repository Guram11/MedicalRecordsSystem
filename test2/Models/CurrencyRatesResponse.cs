using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalRecordsSystem.models
{
    public class CurrencyRatesResponse
    {
        public string? MainCurrency { get; set; }
        public Dictionary<string, decimal>? Rates { get; set; }
    }
}
