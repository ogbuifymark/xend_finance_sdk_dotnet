# xend_finance_sdk_dotnet
Build applications on-top of the Xend Finance Smart Contract Protocols.

#Installation
1. In Solution Explorer, right-click References and choose Manage NuGet Packages.

2. Choose "nuget.org" as the Package source, select the Browse tab, search for XendFinanceSDK, select that package in the list, and select Install. If you want more information on the NuGet Package Manager, see Install and manage packages using Visual Studio.

3. Accept any license prompts.

4. (Visual Studio 2017 only) If prompted to select a package management format, select PackageReference in project file

5. If prompted to review changes, select OK.

#How to use the sdk

Below is a code snippet to guild you on how to use the sdk
```
using NUnit.Framework;
using System;
using XendFinanceSDK;
using XendFinanceSDK.Models;

namespace SdkClient
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
            int chainId = 56; // this is for Bsc mainnet 
            string privateKey = "XXX";
            
            Init init = new Init(chainId, privateKey);
            //xautoDeposit(init, "BUSD", 0.01m);
            //xautoWithdrawal(init, "BUSD", 0.01m);
            xvaltWithdrawal(init, "BUSD", 0.01m);
            //xvaltDeposit(init, "BUSD", 0.01m);
            

            Console.WriteLine("finished");
        }
        static void xautoDeposit(Init init, string token, decimal amount)
        {
            // for this first call approval before calling the deposit function
            Response aproveResponse = init.ApproveXAutoTransaction(token, amount).Result;
            Response xAutoDeposit = init.XAutoDepositTransaction(token, amount).Result;
  
        }

        static void xvaltDeposit(Init init, string token, decimal amount)
        {
            // for this first call approval before calling the deposit function
            Response aproveResponse = init.ApproveXValutTransaction(token, amount).Result;
            Response xValuDeposit = init.XValtDepositTransaction(token, amount).Result;

        }

        static void xautoWithdrawal(Init init, string token, decimal amount)
        {
            Response xautowithdraw = init.XAutoWithdrawTransaction(token, amount).Result;
            Assert.AreEqual(xautowithdraw.status, true);
        }

        static void xvaltWithdrawal(Init init, string token, decimal amount)
        {
            Response xvaultwithraw = init.XValtWithdraawTransaction(token, amount).Result;
            Assert.AreEqual(xvaultwithraw.status, true);
        }
    }
}
```
