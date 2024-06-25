using DataAccessLibrary.Models.ML;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MLUI.Services
{
    public class DataCollectionService
    {
        private readonly DataAccessService dataAccess;
        private readonly float trainingDataPercent = .8f;

        public DataCollectionService(DataAccessService dataAccess)
        {
            this.dataAccess = dataAccess;
        }
        public (List<MLStockFeatureModel> trainingData, List<MLStockFeatureModel> testData) GetMLData()
        {
            Console.WriteLine("Loading Data");
            List<MLStockFeatureModel> trainingData = new List<MLStockFeatureModel>();
            List<MLStockFeatureModel> testData = new List<MLStockFeatureModel>();
            List<MLStockFeatureModel> allData = new List<MLStockFeatureModel>();

            int[] years =
            {
                2009,
                2010,
                2011,
                2012,
                2013,
                2014,
                2015,
                2016,
                2017,
                2018,
                2019,
                2020,
                2021,
                2022,
                2023,
            };

            foreach (var year in years)
            {
                Console.WriteLine("Getting ML data for " + year);
                
                var yearData = dataAccess.GetListOfMLStockFeatures(year.ToString(), (year + 1).ToString());

                allData.AddRange(yearData);

            }
            var randomizedData = allData.OrderBy(item => Guid.NewGuid()).ToList();

            int splitSize = (int)(randomizedData.Count * trainingDataPercent);

            trainingData.AddRange(randomizedData.Take(splitSize).ToList());
            testData.AddRange(randomizedData.Skip(splitSize).ToList());

            return (trainingData, testData);
        }
    }
}
