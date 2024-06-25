namespace StockApi.Models
{
    public class StockInfoModel
    {
        public int Id { get; set; }
        public string CIK { get; set; }
        public string Ticker { get; set; }
        public string Name { get; set; }
    }
}
