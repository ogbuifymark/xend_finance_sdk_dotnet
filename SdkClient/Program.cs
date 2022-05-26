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
            int chainId = 56;
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
            Response aproveResponse = init.ApproveXAutoTransaction(token, amount).Result;
            Response xAutoDeposit = init.XAutoDepositTransaction(token, amount).Result;
            Assert.AreEqual(aproveResponse.status,true );
            Assert.AreEqual(xAutoDeposit.status, true);
        }

        static void xvaltDeposit(Init init, string token, decimal amount)
        {
            Response aproveResponse = init.ApproveXValutTransaction(token, amount).Result;
            Response xValuDeposit = init.XValtDepositTransaction(token, amount).Result;
            Assert.AreEqual(aproveResponse.status, true);
            Assert.AreEqual(xValuDeposit.status, true);
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
