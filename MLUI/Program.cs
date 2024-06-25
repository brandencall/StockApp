// See https://aka.ms/new-console-template for more information
using DataAccessLibrary.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ML;
using MLUI;
using MLUI.Services;


StartUp.OnStartUp();

var dataCollection = StartUp.serviceProvider.GetService<DataCollectionService>();
(var trainingData, var testData) = dataCollection.GetMLData();


MLContext mlContext = new MLContext();

var model = ML.Train(mlContext, trainingData);

ML.Evaluate(mlContext, model, testData);

Console.WriteLine();