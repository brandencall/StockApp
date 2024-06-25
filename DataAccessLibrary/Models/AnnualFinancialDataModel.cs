using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class AnnualFinancialDataModel
    {
        public string Title { get; set; }
        public string Label { get; set; }
        public string FiscalYear { get; set; }
        public string DisplayName { get; set; }
        public decimal CurrencyValue { get; set; }
    }
}
