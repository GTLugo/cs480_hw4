namespace cs480_hw4; 

public class Agent {
  private Tree decisionTree_ = new();
  
  public Agent() {
    
  }

  public void Train(List<Data> dataList) {
    decisionTree_.Update(dataList, decisionTree_.Root, Strategy.LeastValues);
    Console.Out.WriteLine(decisionTree_);
  }
}
