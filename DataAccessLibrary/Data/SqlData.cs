using DataAccessLibrary.Databases;
using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLibrary.Data
{
    public class SqlData : ISqlData
    {

        private readonly ISqlDataAccess db;
        private const string connectionStringName = "SqlDb";

        public SqlData(ISqlDataAccess db)
        {
            this.db = db;
        }

        public void InsertStock(string cik, string ticker, string name)
        {
            db.SaveData("dbo.spStocks_Insert",
                        new { cik, ticker, name },
                        connectionStringName,
                        true);
        }

        public void InsertStockFinacialData(string cik,
                                               StockFinancesModel stockFinancesModel,
                                               JsonElement financialValue)
        {
            int stockId = GetStockId(cik);

            UnitModel unitModel = GetUnit(stockFinancesModel.UnitType);
            int financialAttributeId = InsertFinancialAttribute(stockFinancesModel.FinancialAttributeTitle,
                                                                stockFinancesModel.FinancialAttributeLabel,
                                                                stockFinancesModel.FinancialAttributeDescription);

            int financialFactId = InsertFinancialFact(unitModel.Id,
                                                      stockFinancesModel.StartDate,
                                                      stockFinancesModel.EndDate,
                                                      stockFinancesModel.FiscalYear,
                                                      stockFinancesModel.FiscalPeriod,
                                                      stockFinancesModel.Form,
                                                      stockFinancesModel.Filed,
                                                      stockFinancesModel.Frame);

            bool insertSuccessful = InsertFinancialValue(stockFinancesModel.UnitType, financialFactId, financialValue);

            if (insertSuccessful == true)
            {
                InsertStockFinances(stockId, financialAttributeId, financialFactId);
            }
        }

        public List<CIKModel> GetListOfStocks()
        {
            return db.LoadData<CIKModel, dynamic>("dbo.spStocks_GetAllCIKs", new { }, connectionStringName, true).ToList();
        }

        // Used in React stock-app: Gets a list of stock models.
        public List<StockModel> GetListOfStockModels()
        {
            return db.LoadData<StockModel, dynamic>("dbo.spStocks_GetAllStocks", new { }, connectionStringName, true).ToList();
        }

        // Used in React stock-app: Get annual stock data by id.
        public List<AnnualFinancialDataModel> GetAnnualStockData(int stockId)
        {
            return db.LoadData<AnnualFinancialDataModel, dynamic>("dbo.spFinancialDataApi_GetAnnual", new { stockId }, connectionStringName, true).ToList();
        }

        // Used in React stock-app: Get annual stock data by id.
        public List<QuarterlyFinancialDataModel> GetQuarterlyStockData(int stockId)
        {
            return db.LoadData<QuarterlyFinancialDataModel, dynamic>("dbo.spFinancialDataApi_GetQuarterly", new { stockId }, connectionStringName, true).ToList();
        }

        public void InsertStockFinancialData(string cik, StockFinancesModel stockFinancesModel, decimal financialValue)
        {
            JsonElement financialValueElement = JsonSerializer.Deserialize<JsonElement>(JsonSerializer.Serialize(financialValue));
            InsertStockFinacialData(cik, stockFinancesModel, financialValueElement);
        }

        
        private int GetStockId(string cik)
        {
            return db.LoadData<IdModel, dynamic>("dbo.spStocks_GetbyCIK",
                                                                new { cik },
                                                                connectionStringName,
                                                                true).ToList().First().Id;

        }

        private UnitModel GetUnit(Enums.UnitType unitType)
        {
            string unitTypeString = Mapping.UnitTypesToString[unitType];

            return db.LoadData<UnitModel, dynamic>("dbo.spUnits_GetByType",
                                                                new { unitTypeString },
                                                                connectionStringName,
                                                                true).ToList().First();
        }


        private int InsertFinancialAttribute(string title,
                                             string label,
                                             string description)
        {
            return db.LoadData<IdModel, dynamic>("dbo.spFinancialAttributes_Insert",
                                                 new { title, label, description },
                                                 connectionStringName,
                                                 true).ToList().First().Id;
        }

        private int InsertFinancialFact(int unitId,
                                        DateTime? startDate,
                                        DateTime? endDate,
                                        string fiscalYear,
                                        string fiscalPeriod,
                                        string form,
                                        string filed,
                                        string frame)
        {
            
            return db.LoadData<IdModel, dynamic>("dbo.spFinancialFacts_Insert",
                                                 new
                                                 {
                                                     unitId,
                                                     startDate,
                                                     endDate,
                                                     fiscalYear,
                                                     fiscalPeriod,
                                                     form,
                                                     filed,
                                                     frame
                                                 },
                                                 connectionStringName,
                                                 true).ToList().First().Id;
        }

        private bool InsertFinancialValue(Enums.UnitType unitType, int financialFactId, JsonElement financialValue)
        {
            var financialType = Mapping.UnitsToFinancialValues[unitType];

            if (financialType == Enums.FinancialValueType.CurrencyValue)
            {
                if (financialValue.TryGetDecimal(out decimal value))
                {
                    try
                    {
                        InsertCurrencyValue(financialFactId, value);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RemoveFinancialFact(financialFactId);
                        return false;
                    }
                    
                }
                else
                {
                    RemoveFinancialFact(financialFactId);
                    return false;
                }
            }
            else if (financialType == Enums.FinancialValueType.CurrencyPerShareValue)
            {
                if (financialValue.TryGetDecimal(out decimal value))
                {
                    try
                    {
                        InsertCurrencyPerSharesValue(financialFactId, value);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        RemoveFinancialFact(financialFactId);
                        return false;
                    }
                    
                }
                else
                {
                    RemoveFinancialFact(financialFactId);
                    return false;
                }
            }
            else if (financialType == Enums.FinancialValueType.ShareValue)
            {
                if (financialValue.TryGetInt64(out long value))
                {
                    try
                    {
                        InsertSharesValue(financialFactId, value);
                        return true;
                    }
                    catch(Exception ex)
                    {
                        RemoveFinancialFact(financialFactId);
                        return false;
                    }
                }
                else
                {
                    RemoveFinancialFact(financialFactId);
                    return false;
                }               
            }
            return false;
        }

        private void RemoveFinancialFact(int financialFactId)
        {
            db.SaveData<dynamic>("dbo.spFinancialFact_Delete",
                                 new { financialFactId },
                                 connectionStringName,
                                 true);
        }

        private void InsertCurrencyValue(int financialFactId, decimal value)
        {
            try
            {
                db.SaveData<dynamic>("dbo.spCurrencyValues_Insert",
                                 new { financialFactId, value },
                                 connectionStringName,
                                 true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
        }
        private void InsertCurrencyPerSharesValue(int financialFactId, decimal value)
        {
            try
            {
                db.SaveData<dynamic>("dbo.spCurrencyPerShareValues_Insert",
                                 new { financialFactId, value },
                                 connectionStringName,
                                 true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertSharesValue(int financialFactId, long value)
        {
            try
            {
                db.SaveData<dynamic>("dbo.spShareValues_Insert",
                                 new { financialFactId, value },
                                 connectionStringName,
                                 true);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void InsertStockFinances(int stockId,
                                         int financialAttributeId,
                                         int financialFactsId)
        {
            db.SaveData<dynamic>("dbo.spStockFinances_Insert",
                                 new { stockId, financialAttributeId, financialFactsId },
                                 connectionStringName,
                                 true);
        }

    }
}
