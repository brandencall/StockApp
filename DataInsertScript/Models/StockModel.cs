using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsertScript.Models
{
    public class StockModel
    {
        [JsonProperty("cik_str")]
        public string CIK { get; set; }
        public string Ticker { get; set; }
        public string Title { get; set; }
    }
}
