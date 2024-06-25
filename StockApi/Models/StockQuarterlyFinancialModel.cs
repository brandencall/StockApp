namespace StockApi.Models
{
    public class StockQuarterlyFinancialModel
    {
        public string Title { get; set; }
        public string Label { get; set; }
        public string DisplayName { get; set; }
        public List<FinancialFactModel> FinancialFacts { get; set; } = new List<FinancialFactModel>();
    }
}
