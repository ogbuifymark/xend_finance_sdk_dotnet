using Microsoft.Extensions.DependencyInjection;
using Moq;
using Nethereum.ABI.FunctionEncoding;
using NUnit.Framework;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK;
using XendFinanceSDK.Models;
using XendFinanceSDK.Service.Interfaces;
using static XendFinanceSDK.Models.Enums;

namespace XendFinanceSDKTest
{
    public class XVaultServiceTest
    {
        IXvaltService xvaltService;

        [SetUp]
        public void Setup()
        {
            string privateKey = "";
            var serviceCollection = new ServiceCollection();
            BlockchainEnvironment blockchainEnvironment = BlockchainEnvironment.Mainnet;
            //serviceCollection.AddXendFinanceSdk(privateKey);
            //serviceCollection.AddXendFinanceSdk(privateKey, blockchainEnvironment);
            serviceCollection.AddXendFinanceSdk(privateKey, "https://bsc-dataseed.binance.org/", "https://polygon-rpc.com/", "https://api.bscscan.com/api?module=gastracker&action=gasoracle&apikey=QR9K3AJJB6QTIDEFNZBQPS7S1N6NURZBTV", "https://api.polygonscan.com/api?module=gastracker&action=gasoracle&apikey=BVD47FZXG7QDH9UKEYBV6DPF7FAZY92P6J", blockchainEnvironment);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            
            xvaltService = serviceProvider.GetService<IXvaltService>();
        }


        [Test]
        public void TestDeposit_Should_Return_Deposit_Amount_Greater_Than_Zero()
        {
            Task.Delay(1000);
            CancellationTokenSource cancellationToken = null;
            var ex = Assert.ThrowsAsync<SmartContractRevertException>(async () => await xvaltService.DepositAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0m, "BUSD", cancellationToken));
            Assert.That(ex.Message, Is.EqualTo("Smart contract error: deposit amount should be bigger than zero"));
        }


        [Test, Order(2)]
        public void TestDeposit_Should_Return_TransactionHash()
        {
            Task.Delay(10000);
            CancellationTokenSource cancellationToken = null;
            string transactionHash = xvaltService.DepositAsync((int)ChainIds.BSCMainnet, 0.01m, "BUSD", cancellationToken).Result;
            Assert.IsNotNull(transactionHash);
        }


        [Test, Order(1)]
        public void TestDeposit_Should_Return_Success()
        {
            Task.Delay(10000);
            CancellationTokenSource cancellationToken = null;
            TransactionResponse transactionResponse = xvaltService.DepositAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0.01m, "BUSD", cancellationToken).Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

        [Test, Order(3)]
        public void TestWithdrawl_Should_Return_Success()
        {
            Task.Delay(10000);
            CancellationTokenSource cancellationTokenSource = null;
            TransactionResponse transactionResponse = xvaltService.WithdrawalAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0.01m, "BUSD", cancellationTokenSource).Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

        [Test, Order(4)]
        public void TestWithdrawl_Should_Also_Return_Success()
        {
            Task.Delay(1000);
            CancellationTokenSource cancellationTokenSource = null;
            TransactionResponse transactionResponse = xvaltService.WithdrawalAndWaitForReceiptAsync((int)ChainIds.BSCMainnet, 0, "BUSD", cancellationTokenSource).Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

        [Test]
        public void Test_Ppfs_Should_Return_Success()
        {
            Task.Delay(10000);
            TransactionResponse transactionResponse = xvaltService.GetPricePerFullShare((int)ChainIds.BSCMainnet, "BUSD").Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

        [Test]
        public void Test_Share_Balance_Should_Return_Success()
        {
            Task.Delay(10000);
            TransactionResponse transactionResponse = xvaltService.GetShareBalance((int)ChainIds.BSCMainnet, "BUSD").Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

        [Test]
        public void Test_MaxAvailableSharesAsync_Should_Return_Success()
        {
            Task.Delay(10000);
            TransactionResponse transactionResponse = xvaltService.MaxAvailableSharesAsync((int)ChainIds.BSCMainnet, "BUSD").Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }

        [Test]
        public void Test_Apy_Should_Return_Success()
        {
            Task.Delay(10000);
            TransactionResponse transactionResponse = xvaltService.GetAPYAsync((int)ChainIds.BSCMainnet, "BUSD").Result;
            Assert.IsTrue(transactionResponse.IsSuccessful);
        }
    }
}