using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class StockFinancesModel
    {
        public string? FinancialAttributeTitle { get; set; }
        public string? FinancialAttributeLabel { get; set; }
        public string? FinancialAttributeDescription { get; set; }
        public Enums.UnitType UnitType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? FiscalYear { get; set; }
        public string? FiscalPeriod { get; set; }
        public string? Form { get; set; }
        public string? Filed { get; set; }
        public string? Frame { get; set; }
    }
}
