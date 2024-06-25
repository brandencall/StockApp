using DataAccessLibrary.Models;
using DataInsertScript.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DataAccessLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInsertScript.Services
{
    public class FMPService
    {

        private readonly HttpClient client;
        private List<string> queryDates = new List<string>();
        private readonly DataAccessService dataAccess;
        private int requestLimit = 300;
        private int requestCount = 0;
        private DateTime nextResetTime;

        public FMPService(DataAccessService dataAccess)
        {
            client = new HttpClient();
            this.dataAccess = dataAccess;
            nextResetTime = DateTime.Now.AddSeconds(60);

            queryDates.Add("from=1994-01-01&to=1999-01-01");
            queryDates.Add("from=1999-01-01&to=2004-01-01");
            queryDates.Add("from=2004-01-01&to=2009-01-01");
            queryDates.Add("from=2009-01-01&to=2014-01-01");
            queryDates.Add("from=2014-01-01&to=2019-01-01");
            queryDates.Add("from=2019-01-01&to=2024-01-01");
            queryDates.Add("from=2024-01-01&to=2025-01-01");

        }

        public async Task PopulateMarketCap(string ticker, string cik)
        {
            var url = Startup.config.GetValue<string>("ApiSettings:HistoricMarketCap");
            var apiKey = Startup.config.GetValue<string>("ApiKeys:FPMKey");

            foreach (var date in queryDates)
            {
                string updateUrl = url + ticker + "?" + date + "&apikey=" + apiKey;

                RateLimit();
                var response = await client.GetAsync(updateUrl);
                requestCount++;

                try
                {
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();

                    try
                    {
                        List<MarketCapModel> marketDataList = JsonConvert.DeserializeObject<List<MarketCapModel>>(content);
                        ParseMarketCapData(marketDataList, cik);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine("Problem deserializing json");
                    }

                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }   
        }

        private void RateLimit()
        {
            if (requestCount >= requestLimit)
            {
                var delayTime = nextResetTime - DateTime.Now;

                if (delayTime > TimeSpan.Zero)
                {
                    Task.Delay(delayTime).Wait();
                }

                requestCount = 0;
                nextResetTime = DateTime.Now.AddSeconds(60);
            }
        }

        private void ParseMarketCapData(List<MarketCapModel> marketDataList, string cik)
        {
            var monthlyMarketCaps = marketDataList
                        .GroupBy(data => new { data.Date.Year, data.Date.Month })
                        .Select(group => group.OrderBy(data => data.Date).First())
                        .ToList();

            StockFinancesModel financesModel = new StockFinancesModel()
            {
                FinancialAttributeTitle = "MarketCapitalization",
                FinancialAttributeLabel = "Market Capitalization",
                FinancialAttributeDescription = "The total value of a publicly traded company's outstanding common shares owned by stockholders. Market capitalization is equal to the market price per common share multiplied by the number of common shares outstanding.",
                UnitType = Enums.UnitType.USD,
            };

            foreach (var marketCap in monthlyMarketCaps)
            {
                financesModel.StartDate = marketCap.Date;
                financesModel.EndDate = marketCap.Date;
                financesModel.FiscalYear = marketCap.Date.Year.ToString();

                dataAccess.InsertFinancialData(cik, financesModel, marketCap.MarketCap);
            }
        }

    }
}
