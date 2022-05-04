namespace cs480_hw4;

public enum Strategy {
  LeastValues,
  MostValues,
}

public class Node {
  public Attribute? Attribute;
  public string Decision = string.Empty;
  public Result Profitable = Result.None;
  public readonly List<Node> Children = new();
  
  public bool IsLeaf() => Profitable != Result.None;
  
  public override string ToString() {
    var str = Attribute + " = " + Decision;
    if (IsLeaf()) {
      str += " : " + Profitable;
    }
    return str;
  }
}

public class Tree {
  private readonly Node root_ = new();

  private Dictionary<Attribute, List<string>> valuesTally_ = new();

  private readonly List<Attribute> availableAttributes_ = Enum.GetValues<Attribute>().ToList();

  public void Build(List<Data> dataList, Strategy strategy) {
    Build_Impl(dataList, root_, strategy);
  }
  
  private void Build_Impl(List<Data> dataList, Node node, Strategy strategy) {
    if (availableAttributes_.Count == 0) {
      node.Profitable = Result.No;
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
      Build_Impl(dataList, child, strategy);
    }
    
    availableAttributes_.Add(attribute);
  }
  
  public void Assign(Data data) {
    Assign_Impl(data, root_);
  }
  
  private static void Assign_Impl(Data data, Node node) {
    foreach (var child in node.Children.Where(
               child => child.Attribute != null 
                        && child.Decision == data.Attributes[child.Attribute.Value])
            ) {
      Assign_Impl(data, child);
      return;
    }
    node.Profitable = data.IsProfitable;
  }
  
  public void Trim() {
    Trim_Impl(root_);
  }
  
  private static void Trim_Impl(Node node) {
    var tempList = new List<Result>();
    foreach (var child in node.Children) {
      Trim_Impl(child);
      tempList.Add(child.Profitable);
    }
    
    if (tempList.Distinct().Count() == 1 && !tempList.Contains(Result.None)) {
      //Console.Out.WriteLine("Trimming: {0} {1} {2}", node.Attribute, node.Decision, node.Profitable);
      node.Profitable = tempList[0];
      node.Children.Clear();
    }
  }
  
  public bool Test(Data data) {
    return Test_Impl(data, root_);
  }
  
  private static bool Test_Impl(Data data, Node node) {
    foreach (var child in node.Children.Where(
               child => child.Attribute != null 
                        && child.Decision == data.Attributes[child.Attribute.Value])
            ) {
      return Test_Impl(data, child);
    }
    
    return data.IsProfitable == node.Profitable;
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

  public override string ToString() {
    //return Root.Children.Aggregate("", (current, node) => current + ToString_Impl(node, 0));
    return ToString_Impl(root_, 0);
  }

  private static string ToString_Impl(Node node, int depth) {
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
}