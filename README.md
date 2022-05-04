# CS 480 - Assignment 4 - Decision Tree AI

## Usage
Built in Rider with .NET Core 6.0, C# 9.0.

To change strategies, you must modify the parameter sent to the Train() method in Program.cs.

## Analysis

The methods I have chosen may not have been very good at representing a decision tree. 
As a whole, the program didn't perform too well against the test data. Both the least values and most values strategies failed to predict the results of the test data correctly. 
This was proven to be purely the algorithm's fault as the results of testing against the training data show the decision tree does work for predicting certain data sets.
Both algorithms performed within margin of error of each other, as one can see in the results below. Additionally, these two algorithms resulted in similar numbers of nodes. 
Had I chosen different algorithms, perhaps I would have gotten different results. This is worth looking into further in the future. 

Sample output:
### Least Values
![Output of least values](https://github.com/GTLugo/cs480_hw4/blob/master/least_values.png)
### Most Values
![Output of most values](https://github.com/GTLugo/cs480_hw4/blob/master/most_values.png)