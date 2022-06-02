using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Models;
using static XendFinanceSDK.Models.Enums;

namespace XendFinanceSDK.Util.Interface
{
    public interface IWeb3Client
    {
        Task<IEnumerable<EventLog<TEventMessage>>> GetEvents<TEventMessage>(int chainId, string contractAddress, ulong startBlock, ulong endBlock) where TEventMessage : IEventDTO, new();
        Task<ulong> GetLatestBlock(int chainId);
        Contract GetContract(int chainId, string contractAddress, string abi);
        Task<ulong> GetBlockTimeStamp(int chainId, ulong blockNumber);
        Task<TransactionResponse> SendTransactionAndWaitForReceiptAsync(int chainId, string contractAddress, string abi, string functionName, GasPriceLevel? gasPriceLevel, CancellationTokenSource cancellationToken, params object[] functionInput);
        Task<string> SendTransactionAsync(int chainId, string contractAddress, string abi, string functionName, GasPriceLevel? gasPriceLevel, params object[] functionInput);
         Task<string> PrivateKeyToAddress();
    }
}
