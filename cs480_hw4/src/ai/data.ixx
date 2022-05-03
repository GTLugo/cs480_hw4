export module data;

#pragma warning(suppress:5050)
import std.core;

export enum class Level {
  Low,
  Medium,
  High,
};

export std::ostream& operator<<(std::ostream& out, const Level& level) {
  switch (level) {
  case Level::Low:    return out << "Low";
  case Level::Medium: return out << "Medium";
  case Level::High:   return out << "High";
  default: return out;
  }
}

export struct Data {
  Level price{Level::Low};
  Level maintenance{Level::Low};
  int capacity{0};
  bool airbag{false};
};

export std::vector<Data> readData(const std::ifstream& file) {
  if (file.is_open()) {
    std::cout << "File open!\n";
  } else {
    std::cout << "File NOT open!\n";
  }
  return {};
}