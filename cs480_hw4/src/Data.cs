using System.Collections;
using Microsoft.VisualBasic.FileIO;

namespace cs480_hw4;

public enum Attribute {
  Price,
  Maintenance,
  Capacity,
  HasAirbag,
}

// For simplicity in the design of the tree nodes, the data points are all stored
// as strings. This allows for easier implimentation, which is important for meeting
// the quick deadline. ;)
public class Data {
  public Dictionary<Attribute, string> Attributes = new() {
    {Attribute.Price, string.Empty},
    {Attribute.Maintenance, string.Empty},
    {Attribute.Capacity, string.Empty},
    {Attribute.HasAirbag, string.Empty},
  };

  public bool IsProfitable;

  public static List<Data> ReadFromFile(string filePath) {
    var dataList = new List<Data>();
    
    var parser = new TextFieldParser(filePath);
    parser.SetDelimiters(",");
    while (!parser.EndOfData) {
      var fields = parser.ReadFields() ?? Array.Empty<string>();

      // Quick hacky way to filter out title rows
      if (fields[0] == "price") {
        continue;
      }

      dataList.Add(new Data {
        Attributes
          = new Dictionary<Attribute, string> {
            { Attribute.Price, fields[0] },
            { Attribute.Maintenance, fields[1] },
            { Attribute.Capacity, fields[2] },
            { Attribute.HasAirbag, fields[3] },
          },
        IsProfitable = "yes".Equals(fields[4]),
      });
    }

    return dataList;
  }

  public override string ToString() {
    return "[" + Attributes[Attribute.Price]
               + ", " + Attributes[Attribute.Maintenance]
               + ", " + Attributes[Attribute.Capacity]
               + ", " + Attributes[Attribute.HasAirbag]
               + "] = " + IsProfitable;
  }
}