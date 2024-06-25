using DataAccessLibrary.Models.ML;
using Microsoft.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLUI
{
    public static class ML
    {

        public static ITransformer Train(MLContext mlContext, List<MLStockFeatureModel> trainingData)
        {
            Console.WriteLine("Training Data");
            IDataView dataView = mlContext.Data.LoadFromEnumerable(trainingData);

            var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "NextYearMarketCap")
                .Append(mlContext.Transforms.Concatenate("Features", "NetSales", "NetIncomeLoss", "OperatingIncomeLoss", 
                "AccountsReceivableNetCurrent", "AssetsCurrent", "LiabilitiesCurrent","StockholdersEquity", 
                "CashAndCashEquivalentsAtCarryingValue"))
                .Append(mlContext.Transforms.NormalizeMinMax("Features"))
                .Append(mlContext.Regression.Trainers.FastTree());

            //var pipeline = mlContext.Transforms.CopyColumns(outputColumnName: "Label", inputColumnName: "NextYearMarketCap")
            //    .Append(mlContext.Transforms.Concatenate("Features", "Assets", "NetIncomeLoss"))
            //    .Append(mlContext.Regression.Trainers.FastTree());

            var model = pipeline.Fit(dataView);

            Console.WriteLine("Done Training");

            return model;

        }

        public static void Evaluate(MLContext mlContext, ITransformer model, List<MLStockFeatureModel> testData)
        {
            IDataView dataView = mlContext.Data.LoadFromEnumerable(testData);
            var predictions = model.Transform(dataView);


            var predictionEngine = mlContext.Model.CreatePredictionEngine<MLStockFeatureModel, PredictedMarketCap>(model);
            var predictionData = testData.Select(td =>
            {
                var prediction = predictionEngine.Predict(td);
                return new
                {
                    td.Ticker,
                    ActualMarketCap = td.NextYearMarketCap,
                    PredictedMarketCap = prediction.MarketCap
                };
            });

            Console.WriteLine("Actual vs. Predicted Market Cap:");
            Console.WriteLine("--------------------------------");
            foreach (var item in predictionData)
            {
                Console.WriteLine(item.Ticker);
                Console.WriteLine($"Actual: {item.ActualMarketCap}, Predicted: {item.PredictedMarketCap}");
            }


            var metrics = mlContext.Regression.Evaluate(predictions, "Label", "Score");

            Console.WriteLine();
            Console.WriteLine($"RSquered Score: {metrics.RSquared:0.##}");
            Console.WriteLine($"Root Mean Squered error: {metrics.RootMeanSquaredError:#.##}");
        }
    }
}
