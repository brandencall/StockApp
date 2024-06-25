using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class QuarterlyFinancialDataModel
    {
        public string Title { get; set; }
        public string Label { get; set; }
        public string Frame { get; set; }
        public string DisplayName { get; set; }
        public decimal CurrencyValue { get; set; }
    }
}
