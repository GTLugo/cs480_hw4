using cs480_hw4;

var agent = new Agent();
var trainingData = Data.ReadFromFile("res/trainData.csv");
agent.Train(trainingData);