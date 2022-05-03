module;
#include "argagg/argagg.hpp"

export module main;

#pragma warning(suppress:5050)
import std.core;

import agent;
import data;
import wait;
import time;

export int main() {
  Agent agent{};
  
  agent.train(readData(std::ifstream{ "res/trainData.csv" }));
  agent.displaySolution();

  // Pause until ready to continue
  std::cout << "Press any key to continue...";
  //wait();
  std::cout << '\n';
}