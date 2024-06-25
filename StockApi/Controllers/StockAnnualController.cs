using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using StockApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockAnnualController : ControllerBase
    {

        private readonly ISqlData db;

        public StockAnnualController(ISqlData db)
        {
            this.db = db;
        }


        [HttpGet("{id}")]
        public IEnumerable<StockAnnualFinancialModel> Get(int id)
        {

            var financialDataList = db.GetAnnualStockData(id);
            List<StockAnnualFinancialModel> output = new List<StockAnnualFinancialModel>();

            if (financialDataList.Count == 0) 
            {
                return output;
            }

            string currentFinancialAttribute = "";
            

            for (int i = 0; i < financialDataList.Count; i++)
            {

                currentFinancialAttribute = financialDataList[i].Title;
                StockAnnualFinancialModel model = new StockAnnualFinancialModel()
                {
                    Title = currentFinancialAttribute,
                    Label = financialDataList[i].Label,
                    DisplayName = financialDataList[i].DisplayName,
                };
                int j = i;

                while (j < financialDataList.Count
                    && financialDataList[j].Title == currentFinancialAttribute)
                {
                    model.FinancialFacts.Add(new FinancialFactModel()
                    {
                        CurrencyValue = financialDataList[j].CurrencyValue,
                        Date = financialDataList[j].FiscalYear,
                    });

                    j++;
                }

                output.Add(model);
                i = j - 1;
            }

            return output;
        }
    }
}
