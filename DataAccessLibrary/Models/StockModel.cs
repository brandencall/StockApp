using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class StockModel
    {
        public int Id { get; set; }
        public string CIK { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
    }
}
