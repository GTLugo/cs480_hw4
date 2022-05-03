namespace cs480_hw4;

public enum Strategy {
  LeastValues,
  MostValues,
}

public class Node {
  public Attribute? Attribute = null;
  public string Decision = string.Empty;
  public bool? Profitable = null;
  public List<Node> Children = new();
  
  public bool IsLeaf() => Profitable is not null;
  
  public override string ToString() {
    var str = Attribute + " = " + Decision;
    if (IsLeaf()) {
      str += " : " + Profitable;
    }
    return str;
  }
}

public class Tree {
  public Node Root = new();

  private Dictionary<Attribute, List<string>> valuesTally_ = new();

  private List<Attribute> availableAttributes_ = Enum.GetValues<Attribute>().ToList();

  public void Update(List<Data> dataList, Node node, Strategy strategy) {
    if (availableAttributes_.Count == 0) {
      node.Profitable = false;
      return;
    }
    
    // 1. A <- the "best" decision attribute for the next node
    var attribute = BestAttribute(dataList, strategy);
    // 3. For each value of A, create a new child (sub-tree) of the node
    foreach (var value in valuesTally_[attribute]) {
      node.Children.Add(new Node {
        Attribute = attribute, // 2. Assign A as decision attribute for node
        Decision = value
      });
      //Console.Out.WriteLine("{0}: {1}", attribute, value);
    }
    
    availableAttributes_.Remove(attribute);

    foreach (var child in node.Children) {
      Update(dataList, child, strategy);
    }
    
    availableAttributes_.Add(attribute);
  }

  // Measure the number of possible values for each attribute in the training data
  private void Measure(List<Data> dataList) {
    valuesTally_ = new Dictionary<Attribute, List<string>>();
    foreach (var attributePair in dataList.SelectMany(data => data.Attributes)) {
      // Check if the values tally already has a list available
      if (!valuesTally_.ContainsKey(attributePair.Key)) {
        valuesTally_.Add(attributePair.Key, new List<string> { attributePair.Value });
      } else {
        // Check if the list already contains the value for the attribute
        if (!valuesTally_[attributePair.Key].Contains(attributePair.Value)) {
          if (!valuesTally_.ContainsKey(attributePair.Key)) {
            valuesTally_.Add(attributePair.Key, new List<string> { attributePair.Value });
          } else {
            valuesTally_[attributePair.Key].Add(attributePair.Value);
          }
        }
      }
    }
    

    // foreach (var attribute in valuesTally_) {
    //   Console.Out.WriteLine("{0}: {1}", attribute.Key, attribute.Value.Count);
    // }
  }

  private Attribute BestAttribute(List<Data> dataList, Strategy strategy) {
    Measure(dataList);
    return (strategy switch {
              Strategy.LeastValues => valuesTally_
                                      .OrderBy(pair => pair.Value.Count)
                                      .First(pair => availableAttributes_.Contains(pair.Key)),
              Strategy.MostValues  => valuesTally_
                                      .OrderBy(pair => pair.Value.Count)
                                      .Last(pair => availableAttributes_.Contains(pair.Key)),
              _                    => throw new ArgumentOutOfRangeException(nameof(strategy), strategy, null)
            }).Key;
  }

  private string ToString_Impl(Node node, int depth) {
    var str = "";
    foreach (var child in node.Children) {
      for (var i = 0; i < depth - 1; i++) {
        str += "  ";
      }
      if (depth > 0) str += "| ";
      str += child + "\n" + ToString_Impl(child, depth + 1);
    }
    return str;
  }

  public override string ToString() {
    //return Root.Children.Aggregate("", (current, node) => current + ToString_Impl(node, 0));
    return ToString_Impl(Root, 0);
  }
}