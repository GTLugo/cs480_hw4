export module agent;

#pragma warning(suppress:5050)
import std.core;

import decision_tree;
import data;

export class Agent {
public:
  Agent() {
    std::cout << "Agent::Agent\n";
  }

  ~Agent() {
    std::cout << "Agent::~Agent\n";
  }

  void train(const std::vector<Data>& data) {
    std::cout << "Agent::solve\n";
  }

  std::vector<std::pair<Data, bool>> test(const std::vector<Data>& data) {
    std::cout << "Agent::solve\n";
    return {};
  }

  void displaySolution() {
    std::cout << "Agent::displaySolution\n";
  }

private:
  DecisionTree tree{};
};