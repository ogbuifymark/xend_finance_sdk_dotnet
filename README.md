# xend_finance_sdk_dotnet
Build applications on-top of the Xend Finance Smart Contract Protocols.

#Installation
1. In Solution Explorer, right-click References and choose Manage NuGet Packages.

2. Choose "nuget.org" as the Package source, select the Browse tab, search for XendFinanceSDK, select that package in the list, and select Install. If you want more information on the NuGet Package Manager, see Install and manage packages using Visual Studio.

3. Accept any license prompts.

4. (Visual Studio 2017 only) If prompted to select a package management format, select PackageReference in project file

5. If prompted to review changes, select OK.

#How to use the sdk

 On your Startup.cs class, you have to add the sdk as a service by either doing this 

 ```
    services.AddXendFinanceSdk("your_privateKey");
 ```
 
 Or 
```
    // Environment in this case is an Enum BlockchainEnvironment which has values Mainnet = 1 and Testnet=2
    // GasPriceLevel is an optional field
    services.AddXendFinanceSdk("your_privateKey", environment, GasPriceLevel);
 ```
 Or

 ```
    // Environment in this case is an Enum BlockchainEnvironment which has values Mainnet = 1 and Testnet=2
    // GasPriceLevel is an optional field
    services.AddXendFinanceSdk("your_privateKey", bscNodeUrl, polygonNodeUrl,bscGasEstimateUrl, polygonGasEstimateUrl,environment,GasPriceLevel );
 ```

Then after registring the sdk in your Startup.cs class, 
Then you can go ahead to use the sdk in you controllers or services. you can inject `IXvaltService` and `IXAutoService` in your costructor. and then call the function you want to call. e.g

```string transactionHash = xautoServer.DepositAsync((int)ChainIds.BSCMainnet, 0.01m, "BUSD", cancellationToken).Result;
```
Or
```
TransactionResponse transactionResponse = xvaltService.GetAPYAsync("BUSD", (int)ChainIds.BSCMainnet).Result;
```