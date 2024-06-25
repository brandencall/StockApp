using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DataInsertScript.Models;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using DataAccessLibrary;
using static Dapper.SqlMapper;

namespace DataInsertScript.Services
{
    public class EdgarApiService
    {
        private readonly HttpClient client;
        private readonly DataAccessService dataAccess;
        private string cik = string.Empty;
        private int requestLimit = 10;
        private int requestCount = 0;
        private DateTime nextResetTime;

        public EdgarApiService(DataAccessService dataAccess)
        {
            client = new HttpClient();
            var headers = new
            {
                UserAgent = "BrandenCall (mailto:brandencall@live.com)",
                Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8",
                Referer = "https://www.sec.gov/edgar/searchedgar/companysearch.html"
            };
            client.DefaultRequestHeaders.Add("User-Agent", headers.UserAgent);
            client.DefaultRequestHeaders.Add("Accept", headers.Accept);
            client.DefaultRequestHeaders.Add("Referer", headers.Referer);
            this.dataAccess = dataAccess;

            nextResetTime = DateTime.Now.AddSeconds(1);
        }
        public async Task PopulateStocksTable()
        {
            var url = Startup.config.GetValue<string>("ApiSettings:CompanyListApi");
            var response = await client.GetAsync(url);

            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex) 
            {
                Console.WriteLine(ex.Message);
            }

            var content = await response.Content.ReadAsStringAsync();
            var stocks = JsonConvert.DeserializeObject<Dictionary<string, StockModel>>(content);

            dataAccess.InsertStockData(stocks.Values.ToList());
        }

        public async Task PopulateStockFinancials(string cik)
        {
            this.cik = cik;
            var url = Startup.config.GetValue<string>("ApiSettings:CompanyFactsApi");
            url += cik + ".json";

            RateLimit();
            var response = await client.GetAsync(url);
            requestCount++;

            try
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                JsonDocument document = JsonDocument.Parse(content);
                ParseFinancialJson(document);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine(ex.Message);
            }
        
        }

        private void RateLimit()
        {
            if(requestCount >= requestLimit)
            {
                var delayTime = nextResetTime - DateTime.Now;

                if (delayTime > TimeSpan.Zero)
                {
                    Task.Delay(delayTime).Wait();
                }

                requestCount = 0;
                nextResetTime = DateTime.Now.AddSeconds(1);
            }
        }

        private void ParseFinancialJson(JsonDocument document)
        {
            
            JsonElement root = document.RootElement;

            if(root.TryGetProperty("facts", out JsonElement facts) == true)
            {
                Dictionary<Enums.FactsType, JsonElement> factList = GetListOfFactProperties(facts);

                foreach (var property in factList)
                {
                    ConstructFinancialModel(property.Value);
                }
            }
      
        }

        private Dictionary<Enums.FactsType, JsonElement> GetListOfFactProperties(JsonElement facts)
        {
            Dictionary<Enums.FactsType, JsonElement> output = new Dictionary<Enums.FactsType, JsonElement>();

            foreach (var fact in Mapping.FactTypesToString)
            {
                facts.TryGetProperty(fact.Value, out JsonElement value);
                if(value.ValueKind != JsonValueKind.Undefined)
                {
                    output[fact.Key] = value;
                }   
            }

            return output;
        }

        private void ConstructFinancialModel(JsonElement fact)
        {
            foreach (var financialAttribute in fact.EnumerateObject())
            {
                DataAccessLibrary.Models.StockFinancesModel financialModel = new DataAccessLibrary.Models.StockFinancesModel();

                financialModel.FinancialAttributeTitle = financialAttribute.Name;
                financialModel.FinancialAttributeLabel = financialAttribute.Value.GetProperty("label").ToString();
                financialModel.FinancialAttributeDescription = financialAttribute.Value.GetProperty("description").ToString();

                JsonElement units = financialAttribute.Value.GetProperty("units");
                UpdateFinancialModelsDataPoints(financialModel, units);
            }
        }

        private void UpdateFinancialModelsDataPoints(DataAccessLibrary.Models.StockFinancesModel financialModel,
                                                     JsonElement units)
        {
            foreach (var unit in Mapping.UnitTypesToString)
            {
                units.TryGetProperty(unit.Value, out JsonElement financialData);

                if (financialData.ValueKind != JsonValueKind.Undefined)
                {
                    financialModel.UnitType = unit.Key;
                    foreach (var dataPoint in financialData.EnumerateArray())
                    {
                        if (dataPoint.TryGetProperty("val", out var financialValue))
                        {
                            UpdateDataPoints(financialModel, dataPoint);
                            dataAccess.InsertFinancialData(cik, financialModel, financialValue);
                            ClearDataPoints(financialModel);
                        }
                        
                    }
                    return;
                }
            }
        }

        private void UpdateDataPoints(DataAccessLibrary.Models.StockFinancesModel financialModel, JsonElement dataPoint)
        {
            if(dataPoint.TryGetProperty("start", out var start))
            {
                financialModel.StartDate = start.GetDateTime().Date;
            }
            if (dataPoint.TryGetProperty("end", out var end))
            {
                financialModel.EndDate = end.GetDateTime().Date;
            }
            if (dataPoint.TryGetProperty("fy", out var fiscalYear))
            {
                financialModel.FiscalYear = fiscalYear.ToString();
            }
            if (dataPoint.TryGetProperty("fp", out var fiscalPeriod))
            {
                financialModel.FiscalPeriod = fiscalPeriod.ToString();
            }
            if (dataPoint.TryGetProperty("form", out var form))
            {
                financialModel.Form = form.ToString();
            }
            if (dataPoint.TryGetProperty("filed", out var filed))
            {
                financialModel.Filed = filed.ToString();
            }
            if(dataPoint.TryGetProperty("frame", out var frame))
            {
                financialModel.Frame = frame.ToString();
            }
        }

        private void ClearDataPoints(DataAccessLibrary.Models.StockFinancesModel financialModel)
        {
            financialModel.StartDate = null;
            financialModel.EndDate = null;
            financialModel.FiscalYear = null;
            financialModel.FiscalPeriod = null;
            financialModel.Form = null;
            financialModel.Filed = null;
            financialModel.Frame = null;

        }
    }
}
