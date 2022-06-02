using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Contracts.ContractHandlers;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Environment;
using XendFinanceSDK.Models;
using XendFinanceSDK.Util.Interface;
using static XendFinanceSDK.Models.Enums;


[assembly: InternalsVisibleTo("XendFinanceSDKTest")]
namespace XendFinanceSDK.Util
{
    internal sealed class Web3Client : IWeb3Client
    {
        private readonly Web3 _bscWeb3;
        private readonly Web3 _polygonWeb3;
        private Account _bscAccount;
        private Account _polygonAccount;
        private GasPriceLevel _gasPriceLevel;
        private IGasEstimatorService _gasEstimatorService;
        private string _privateKey;

        public Web3Client(string privateKey, BigInteger bscChainId, BigInteger polygonChainId, string bscNodeUrl, string polygonNodeUrl, GasPriceLevel gasPriceLevel, IGasEstimatorService gasEstimatorService)
        {
            _privateKey = privateKey;
            _bscAccount = new Account(privateKey, bscChainId);
            _polygonAccount = new Account(privateKey, polygonChainId);
            _bscWeb3 = new Web3(_bscAccount, bscNodeUrl);
            _polygonWeb3 = new Web3(_polygonAccount, polygonNodeUrl);
            _gasPriceLevel = gasPriceLevel;
            _gasEstimatorService = gasEstimatorService;
        }



        public async Task<IEnumerable<EventLog<TEventMessage>>> GetEvents<TEventMessage>(int chainId, string contractAddress, ulong startBlock, ulong endBlock) where TEventMessage : IEventDTO, new()
        {
            BlockParameter startBlockParameter = new BlockParameter(startBlock);
            BlockParameter endBlockParameter = new BlockParameter(endBlock);
            Web3 web3 = GetWeb3Instance(chainId);
            Event<TEventMessage> handler = web3.Eth.GetEvent<TEventMessage>(contractAddress);
            NewFilterInput filterInput = handler.CreateFilterInput(startBlockParameter, endBlockParameter);

            IEnumerable<EventLog<TEventMessage>> events = await handler.GetAllChangesAsync(filterInput);
            return events;
        }

        public async Task<ulong> GetLatestBlock(int chainId)
        {
            Web3 web3 = GetWeb3Instance(chainId);
            HexBigInteger blockNumberHex = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            return ulong.Parse(blockNumberHex.Value.ToString());
        }

        public async Task<ulong> GetBlockTimeStamp(int chainId, ulong blockNumber)
        {
            Web3 web3 = GetWeb3Instance(chainId);
            BlockParameter blockParameter = new BlockParameter(blockNumber);
            BlockWithTransactions blockWithTransactions = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(blockParameter);
            ulong blockTimeStamp = ulong.Parse(blockWithTransactions.Timestamp.Value.ToString());
            return blockTimeStamp;
        }
        public async Task<string> SendTransactionAsync(int chainId, string contractAddress, string abi, string functionName, GasPriceLevel? gasPriceLevel, params object[] functionInput)
        {
            Contract contract = GetContract(chainId, contractAddress, abi);
            Account account = GetAccountInstance(chainId);
            var function = contract.GetFunction(functionName);
            var gas = await function.EstimateGasAsync(account.Address, null, null, functionInput);
            HexBigInteger gasPrice = await GetGasPrice(chainId, gasPriceLevel);
            HexBigInteger gasPriceWei = new HexBigInteger(BigInteger.Parse(((int)gasPrice.Value * Math.Pow(10, 9)).ToString())); // 5 Gwei
            string transactionHash = await function.SendTransactionAsync(account.Address, gas, gasPriceWei, null, functionInput);
            return transactionHash;
        }


        public async Task<TransactionResponse> SendTransactionAndWaitForReceiptAsync(int chainId, string contractAddress, string abi, string functionName, GasPriceLevel? gasPriceLevel, CancellationTokenSource cancellationToken, params object[] functionInput)
        {
            try
            {
                Contract contract = GetContract(chainId, contractAddress, abi);
                Account account = GetAccountInstance(chainId);
                var function = contract.GetFunction(functionName);
                var gas = await function.EstimateGasAsync(account.Address, null, null, functionInput);
                HexBigInteger gasPrice = await GetGasPrice(chainId, gasPriceLevel);
                HexBigInteger gasPriceWei = new HexBigInteger(BigInteger.Parse(((int)gasPrice.Value * Math.Pow(10, 9)).ToString())); // 5 Gwei

                TransactionReceipt txReceipt = await function.SendTransactionAndWaitForReceiptAsync(account.Address, gas, gasPriceWei, null, cancellationToken, functionInput);
                bool isSuccessful = txReceipt.Status == new HexBigInteger(1);
                return new TransactionResponse
                {
                    IsSuccessful = isSuccessful,
                    TransactionHash = txReceipt.TransactionHash,
                    BlockHash = txReceipt.BlockHash
                };
            }
            catch (Exception ex)
            {

                throw ex;
            }

            
        }


        public Contract GetContract(int chainId, string contractAddress, string abi)
        {
            contractAddress = AddressValidator.ValidateAddress(contractAddress);
            Web3 web3 = GetWeb3Instance(chainId);
            string json = "";
            string path = Path.Combine(Directory.GetCurrentDirectory(), "Abi", abi);
            using (StreamReader r = new StreamReader(path))
            {
                json = r.ReadToEnd();
            }
            Contract contract = web3.Eth.GetContract(json, contractAddress);
            return contract;
        }

        private async Task<HexBigInteger> GetGasPrice(int chainId, GasPriceLevel? gasPriceLevel)
        {
            GasEstimateResponse gasEstimateResponse = await _gasEstimatorService.EstimateGas(chainId);
            if (!gasPriceLevel.HasValue)
            {
                gasPriceLevel = _gasPriceLevel;
            }

            switch (gasPriceLevel.Value)
            {
                case GasPriceLevel.Slow:
                    return new HexBigInteger(new BigInteger(gasEstimateResponse.LowGas));
                case GasPriceLevel.Average:
                    return new HexBigInteger(new BigInteger(gasEstimateResponse.AverageGas));
                case GasPriceLevel.Fast:
                    return new HexBigInteger(new BigInteger(gasEstimateResponse.FastGas));
                default:
                    throw new ArgumentOutOfRangeException("Unsupported Gas Price Level");
            }
        }


        public async Task<string> PrivateKeyToAddress()
        {
            Account account = new Account(_privateKey);
            return account.Address;
        }
        private Account GetAccountInstance(int chainId)
        {
            switch (chainId)
            {
                case (int)ChainId.BSCMainnet:
                    return _bscAccount;
                case (int)ChainId.BSCTestnet:
                    return _bscAccount;
                case (int)ChainId.PolygonMainnet:
                    return _polygonAccount;
                default:
                    throw new ArgumentOutOfRangeException("Unsupported Network Chain");
            }
        }

      
        private Web3 GetWeb3Instance(int chainId)
        {
            switch (chainId)
            {
                case (int)ChainId.BSCMainnet:
                    return _bscWeb3;
                case (int)ChainId.BSCTestnet:
                    return _bscWeb3;
                case (int)ChainId.PolygonMainnet:
                    return _polygonWeb3;
                default:
                    throw new ArgumentOutOfRangeException("Unsupported Network Chain");
            }
        }

       
    }
}
