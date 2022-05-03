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
var accuracy = agent.Test(testingData);
sw.Stop();
var testTime = sw.Elapsed.TotalSeconds;

Console.Out.WriteLine("Decision Tree:\n{0}", agent.Tree());
Console.Out.WriteLine("Prediction Accuracy: {0}%\n", accuracy * 100);
Console.Out.WriteLine("Elapsed time:\n| Train Time: {0} s\n| Test Time: {1} s", trainTime, testTime);