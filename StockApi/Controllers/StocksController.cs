using DataAccessLibrary.Data;
using Microsoft.AspNetCore.Mvc;
using StockApi.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StockApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StocksController : ControllerBase
    {
        private readonly ISqlData db;

        public StocksController(ISqlData db)
        {
            this.db = db;
        }

        // GET: api/<StockController>
        [HttpGet]
        public IEnumerable<StockInfoModel> Get()
        {
            var listOfStocks = db.GetListOfStockModels();
            List<StockInfoModel> output = new List<StockInfoModel>();

            foreach(var stock in listOfStocks)
            {
                StockInfoModel model = new StockInfoModel()
                {
                    Id = stock.Id,
                    CIK = stock.CIK,
                    Ticker = stock.Ticker,
                    Name = stock.Name,
                };
                output.Add(model);
            }

            return output;
        }

    }
}
