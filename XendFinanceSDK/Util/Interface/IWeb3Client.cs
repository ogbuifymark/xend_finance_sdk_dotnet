using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace XendFinanceSDK.Util.Interface
{
    public interface IWeb3Client
    {
        Task<ulong> GetBlockTimeStamp(ulong blockNumber);
        Task<ulong> GetLatestBlock();
        //Task<string> GetTokenUri(BigInteger tokenid, string contractAddress);
        Task<IEnumerable<EventLog<TEventMessage>>> GetEvents<TEventMessage>(string contractAddress, ulong startBlock, ulong endBlock) where TEventMessage : IEventDTO, new();
        Task<T> CallContract<T>(string contractAddress, string abi, string functionName, params object[] functionInput) where T : IFunctionOutputDTO, new();
        Task<T> CallContract<T>(string contractAddress, string abi, string functionName, string from, params object[] functionInput) where T : IFunctionOutputDTO, new();
        Task<T> CallContract<T, W>(string contractAddress, W inputFunction) where W : FunctionMessage, new() where T : class, new();
        Task<Contract> CreateContract(string provider, string chainId, string Abiname, string contractAddress, string privateKey);
        Task<string> PrivateKeyToAddress(string privateKey);
    }
}
