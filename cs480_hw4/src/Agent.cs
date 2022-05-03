namespace cs480_hw4; 

public class Agent {
  private Tree decisionTree_ = new();
  
  public Agent() {
    
  }

  public void Train(List<Data> dataList) {
    // builds the tree according to the data and the strategy
    decisionTree_.Build(dataList, Strategy.MostValues);
    // Assigns the profitable data to the tree
    foreach (var data in dataList) {
      decisionTree_.Assign(data);
    }
    // Trims the tree to just the necessary branches (This part took the longest to figure out)
    decisionTree_.Trim();
  }

  public bool Predict(Data data) {
    return false;
  }

  public string tree() {
    return decisionTree_.ToString();
  }
}
