# FioSharp (WIP)

A C# client for interacting with FIO.  
Can be used with Godot and Unity game engines, to enable FIO integration in game.

## Running Tests

The only tests that are functioning right now are in the following files:
- ApiUnitTests.cs
- FioSdkIntegrationTests.cs
- FioSdkUnitTests.cs
- FioUnitTests.cs

## Scripts

Scripts are used to generate the `FioApi.cs` and `FioTransactions.cs` files.  
They are written in python.  
To update, pull down the yaml file located [here]() and swap it out with the current one.  
Run each file: `python3 GenApi.py|GenTransaction.py`  
Copy the file contents into their counterpart in the FioSharp project.