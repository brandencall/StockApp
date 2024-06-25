using DataAccessLibrary.Data;
using DataAccessLibrary.Models;
using DataInsertScript.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataInsertScript.Services
{
    public class DataAccessService
    {
        private readonly ISqlData db;

        public DataAccessService(ISqlData db)
        {
            this.db = db;
        }

        public void InsertStockData(List<Models.StockModel> stocks)
        {
            foreach (Models.StockModel stock in stocks)
            {
                db.InsertStock(AddZerosToCIK(stock.CIK), stock.Ticker, stock.Title);
            }
        }
        
        public void InsertFinancialData(string cik, 
                                           StockFinancesModel financialModel,
                                           JsonElement financialValue)
        {
            db.InsertStockFinacialData(cik, financialModel, financialValue);
        }

        public void InsertFinancialData(string cik,
                                        StockFinancesModel financialModel,
                                        decimal financialValue)
        {
            db.InsertStockFinancialData(cik, financialModel, financialValue);
        }

        private string AddZerosToCIK(string cik)
        {
            return cik.PadLeft(10, '0');
        }

        public List<CIKModel> GetAllCIKs()
        {
            return db.GetListOfStocks();
        }
        public List<DataAccessLibrary.Models.StockModel> GetStockModels()
        {
            return db.GetListOfStockModels();
        }

        
    }
}
