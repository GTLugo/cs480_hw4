# CS 480 - Assignment 2 - Sudoku Solving AI

## Usage
Built in Rider with .NET Core 6.0, C# 9.0.

Run the following command syntax. Binaries are built into the build directory under the folder for the target platform.
```
./sudoku_ai <difficulty> [smart]
```

+ difficulty: can be of the values: "easy," "medium," "hard," or "evil"
+ smart (optional): flag that indicates whether to use a minimum remaining value (MRV) strategy.

Examples:
```
./sudoku_ai easy
./sudoku_ai hard smart
```
## Analysis

The smart search algorithm far outperforms the simple algorithm. There are orders of magnitude in difference between the run time of the simple versus smart algorithms. 

Additionally, the variable assignment count of the smart algorithm was astronomically lower.

Overall, the smart algorithm's run time and variable assignments did not explode nearly as quickly as the simple algorithm's did, indicating much better performance.

Sample output:
![Output of simple algorithm](https://github.com/GTLugo/cs480_hw2_cpp/blob/master/output.png)
![Output of smart algorithm](https://github.com/GTLugo/cs480_hw2_cpp/blob/master/output_smart.png)