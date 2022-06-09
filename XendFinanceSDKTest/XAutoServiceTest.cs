using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using XendFinanceSDK;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Service.Interfaces;
using Nethereum.ABI.FunctionEncoding;
using XendFinanceSDK.Models;
using static XendFinanceSDK.Models.Enums;

namespace XendFinanceSDKTest
{
    class XAutoServiceTest
    {
        IXAutoService xautoServer;

        [SetUp]
        public void Setup()
        {
            string privateKey = "";
            var serviceCollection = new ServiceCollection();
            BlockchainEnvironment blockchainEnvironment = BlockchainEnvironment.Mainnet;
            serviceCollection.AddXendFinanceSdk(privateKey);
            //serviceCollection.AddXendFinanceSdk(privateKey, blockchainEnvironment);
            //serviceCollection.AddXendFinanceSdk(privateKey, "https://bsc-dataseed.binance.org/", "https://polygon-rpc.com/", "https://api.bscscan.com/api?module=gastracker&action=gasoracle&apikey=QR9K3AJJB6QTIDEFNZBQPS7S1N6NURZBTV", "https://api.polygonscan.com/api?module=gastracker&action=gasoracle&apikey=BVD47FZXG7QDH9UKEYBV6DPF7FAZY92P6J", blockchainEnvironment);

            var serviceProvider = serviceCollection.BuildServiceProvider();
            xautoServer = serviceProvider.GetService<IXAutoService>();
        }


        [Test, Order(3)]
        public void TestDeposit_Should_Return_Deposit_Amount_Greater_Than_Zero()
        {
            Task.Delay(1000);
            CancellationTokenSource cancellationToken = null;
            var ex = Assert.ThrowsAsync<SmartContractRevertException>(async () => await xautoServer.DepositAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0m, "BUSD", cancellationToken));
            Assert.That(ex.Message, Is.EqualTo("Smart contract error: deposit must be greater than 0"));
        }


        [Test, Order(2)]
        public void TestDeposit_Should_Return_TransactionHash()
        {
            Task.Delay(10000);
            CancellationTokenSource cancellationToken = null;
            string transactionHash = xautoServer.DepositAsync((int)ChainIds.BSCMainnet, 0.01m, "BUSD", cancellationToken).Result;
            Assert.IsNotNull(transactionHash);
        }


        [Test, Order(1)]
        public void TestDeposit_Should_Return_Success()
        {
            Task.Delay(1000);
            CancellationTokenSource cancellationToken = null;
            TransactionResponse transactionResponse = xautoServer.DepositAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0.01m, "BUSD", cancellationToken).Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }
        [Test, Order(3)]
        public void TestWithdrawl_Should_Return_Success()
        {
            Task.Delay(10000);
            CancellationTokenSource cancellationTokenSource = null;
            TransactionResponse transactionResponse = xautoServer.WithdrawalAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0.01m, "BUSD", cancellationTokenSource).Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

        [Test, Order(4)]
        public void TestWithdrawl_Should_Also_Return_Success()
        {
            Task.Delay(1000);
            CancellationTokenSource cancellationTokenSource = null;
            TransactionResponse transactionResponse = xautoServer.WithdrawalAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0, "BUSD", cancellationTokenSource).Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }
        [Test]
        public void Test_Ppfs_Should_Return_Success()
        {
            Task.Delay(10000);
            TransactionResponse transactionResponse = xautoServer.GetPricePerFullShare((int)ChainIds.BSCMainnet, "BUSD").Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

      

        [Test]
        public void Test_Apy_Should_Return_Success()
        {
            Task.Delay(10000);
            TransactionResponse transactionResponse = xautoServer.GetAPYAsync((int)ChainIds.BSCMainnet, "BUSD").Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }
    }
}
