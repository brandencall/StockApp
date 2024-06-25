using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models.ML
{
    public class MLStockFeatureModel
    {
        public string CIK { get; set; }
        public string Ticker { get; set; }
        public string FiscalYear { get; set; }
        public float NextYearMarketCap { get; set; }
        public float NetSales { get; set; }
        public float CashAndCashEquivalentsAtCarryingValue { get; set; }
        public float NetIncomeLoss { get; set; }
        public float OperatingIncomeLoss { get; set; }
        public float AccountsReceivableNetCurrent { get; set; }
        public float AssetsCurrent { get; set; }
        public float LiabilitiesCurrent { get; set; }
        public float StockholdersEquity { get; set; }
    }
    public class PredictedMarketCap
    {
        [ColumnName("Score")]
        public float MarketCap { get; set; }
    }
}
