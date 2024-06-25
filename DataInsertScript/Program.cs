using DataAccessLibrary.Models;
using DataInsertScript;
using DataInsertScript.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;



class Program
{
    static async Task Main(string[] args)
    {
        Startup.OnStartUp();
        var edgarAPI = Startup.serviceProvider.GetService<EdgarApiService>();
        var fmpAPI = Startup.serviceProvider.GetService<FMPService>();

        int option = UserInterface.MenuOption();

        if (option == 1)
        {
            await edgarAPI.PopulateStocksTable();
        }
        else if (option == 2)
        {
            var dataAccess = Startup.serviceProvider.GetService<DataAccessService>();
            List<CIKModel> ciks = dataAccess.GetAllCIKs();

            for (int i = 6436; i < ciks.Count; i++)
            {
                Console.WriteLine("Populating: " + ciks[i].CIK);
                await edgarAPI.PopulateStockFinancials(ciks[i].CIK);
            }
        }
        else if(option == 3)
        {
            var dataAccess = Startup.serviceProvider.GetService<DataAccessService>();
            List<StockModel> stocks = dataAccess.GetStockModels();


            for (int i = 0; i < stocks.Count; i++)
            {
                Console.WriteLine("Populating: " + stocks[i].CIK + ", " + stocks[i].Ticker);
                await fmpAPI.PopulateMarketCap(stocks[i].Ticker, stocks[i].CIK);
            }

        }
    }
}