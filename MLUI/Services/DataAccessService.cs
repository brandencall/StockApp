using DataAccessLibrary.Data;
using DataAccessLibrary.Models.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLUI.Services
{
    public class DataAccessService
    {
        private readonly ISqlDataML db;

        public DataAccessService(ISqlDataML db)
        {
            this.db = db;
        }

        public List<MLStockFeatureModel> GetListOfMLStockFeatures(string year, string nextYear)
        {
            return db.GetMLStockFeatureModels(year, nextYear);
        }

    }
}
