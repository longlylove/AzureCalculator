using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureCalculatorAPI.Models
{
    public class CalculationFormula
    {
        public decimal LeftNumber { get; set; }
        public decimal RightNumber { get; set; }
        public string Operator { get; set; }
    }
}
