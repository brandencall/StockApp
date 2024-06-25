using DataAccessLibrary.Models.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLibrary.Data
{
    public interface ISqlDataML
    {
        List<MLStockFeatureModel> GetMLStockFeatureModels(string year, string labelYear);
    }
}
