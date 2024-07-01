using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MedicalRecordsSystem.models;

namespace MedicalRecordsSystem.Interfaces
{
    internal interface ICurrencyRatesRetriever
    {
        Task<CurrencyRatesResponse> RetrieveDataAsync(DateTime date);
    }
}
