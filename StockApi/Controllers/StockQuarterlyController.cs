using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using StockApi.Models;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace StockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockQuarterlyController : Controller
    {

        private readonly ISqlData db;

        public StockQuarterlyController(ISqlData db)
        {
            this.db = db;
        }

        [HttpGet("{id}")]
        public IEnumerable<StockQuarterlyFinancialModel> Get(int id)
        {
            var financialDataList = db.GetQuarterlyStockData(id);
            List<StockQuarterlyFinancialModel> output = new List<StockQuarterlyFinancialModel>();

            if (financialDataList.Count == 0)
            {
                return output;
            }

            string currentFinancialAttribute = "";

            for (int i = 0; i < financialDataList.Count; i++)
            {
                currentFinancialAttribute = financialDataList[i].Title;
                StockQuarterlyFinancialModel model = new StockQuarterlyFinancialModel()
                {
                    Title = currentFinancialAttribute,
                    Label = financialDataList[i].Label,
                    DisplayName = financialDataList[i].DisplayName,
                };

                int j = i;

                while (j < financialDataList.Count
                    && financialDataList[j].Title == currentFinancialAttribute)
                {
                    if (financialDataList[j].Frame.Length > 6)
                    {
                        model.FinancialFacts.Add(new FinancialFactModel()
                        {
                            CurrencyValue = financialDataList[j].CurrencyValue,
                            Date = financialDataList[j].Frame.Substring(2,6),
                        });
                    }

                    j++;

                }

                output.Add(model);
                i = j - 1;
            }

            
            return output;
        }

    }
}
