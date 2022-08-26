# FioSharp (WIP)

A C# client for interacting with FIO.  
Can be used with Godot and Unity game engines, to enable FIO integration in game.

## Running Tests

The only tests that are functioning right now are in the following files:
- ApiUnitTests.cs
- FioSdkIntegrationTests.cs
- FioSdkUnitTests.cs
- FioUnitTests.cs

To run the tests in any of these files, 
1. Open the `FioSharp.sln` file in Visual Studio.  
2. Navigate to FioSharp.Tests.  
3. Right click on any of the test files and click on `Run Test(s)`.  
4. If that option is grayed out, open the file you wish to run the tests of, find a `[Test]` tag, right click on it and click on `Run Test(s)`. This will run that individual test.  
5. Once you have done that, the `Run Test(s)` should be available by right clicking on the file.  

## Scripts

Scripts are used to generate the `FioApi.cs` and `FioTransactions.cs` files.  
They are written in python.  
To update, pull down the yaml file located [here]() and swap it out with the current one.  
Run each file: `python3 GenApi.py|GenTransaction.py`  
Copy the file contents into their counterpart in the FioSharp project.