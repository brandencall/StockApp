using DataAccessLibrary.Databases;
using DataAccessLibrary.Models.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Data
{
    public class SqlDataML : ISqlDataML
    {
        private readonly ISqlDataAccess db;
        private const string connectionStringName = "SqlDb";

        public SqlDataML(ISqlDataAccess db)
        {
            this.db = db;
        }

        public List<MLStockFeatureModel> GetMLStockFeatureModels(string year, string labelYear)
        {
            return db.LoadData<MLStockFeatureModel, dynamic>("dbo.spFinancialData_GetStocksWithFeatureList",
                                                             new { year, labelYear },
                                                             connectionStringName,
                                                             true).ToList();
        }
    }
}
