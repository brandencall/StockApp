using DataAccessLibrary.Models;
using System.Text.Json;

namespace DataAccessLibrary.Data
{
    public interface ISqlData
    {
        List<AnnualFinancialDataModel> GetAnnualStockData(int stockId);
        List<StockModel> GetListOfStockModels();
        List<CIKModel> GetListOfStocks();
        List<QuarterlyFinancialDataModel> GetQuarterlyStockData(int stockId);
        void InsertStock(string cik, string ticker, string name);
        void InsertStockFinacialData(string cik, StockFinancesModel stockFinancesModel, JsonElement financialValue);
        void InsertStockFinancialData(string cik, StockFinancesModel stockFinancesModel, decimal financialValue);
    }
}