namespace cs480_hw4; 

public class Agent {
  private Tree decisionTree_ = new();
  
  public Agent() {
    
  }

  public void Train(List<Data> dataList) {
    decisionTree_.Build(dataList, Strategy.LeastValues);
    foreach (var data in dataList) {
      decisionTree_.Assign(data);
    }
    decisionTree_.Trim();
  }

  public bool Predict(Data data) {
    return false;
  }

  public string tree() {
    return decisionTree_.ToString();
  }
}
