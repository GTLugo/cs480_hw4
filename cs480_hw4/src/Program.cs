using System.Diagnostics;
using cs480_hw4;

var agent = new Agent();
var trainingData = Data.ReadFromFile("res/trainData.csv");
var testingData = Data.ReadFromFile("res/testData.csv");

var sw = Stopwatch.StartNew();
agent.Train(trainingData);
sw.Stop();
var trainTime = sw.Elapsed.TotalSeconds;

sw = Stopwatch.StartNew();
var prediction1 = agent.Predict(testingData[0]);
var prediction2 = agent.Predict(testingData[1]);
sw.Stop();
var testTime = sw.Elapsed.TotalSeconds;

Console.Out.Write("Decision Tree:\n{0}", agent.tree());
Console.Out.WriteLine("Predictions:\n| Prediction 1: {0} \n| Prediction 2: {1}",
                      (prediction1) ? "Correct" : "Incorrect",
                      (prediction2) ? "Correct" : "Incorrect");
Console.Out.WriteLine("Elapsed time:\n| Train Time: {0} s\n| Test Time: {1} s", trainTime, testTime);