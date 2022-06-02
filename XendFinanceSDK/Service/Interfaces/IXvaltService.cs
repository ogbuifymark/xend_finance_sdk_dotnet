using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Models;

namespace XendFinanceSDK.Service.Interfaces
{
    public interface IXvaltService
    {
        Task<TransactionResponse> DepositAndWaitForReceiptAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<string> DepositAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<TransactionResponse> WithdrawalAndWaitForReceiptAsync(int chainId, decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<string> WithdrawalAsync(int chainId, decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<TransactionResponse> GetPricePerFullShare(int chainId,string tokenName);
        Task<TransactionResponse> GetShareBalance(int chainId,string tokenName);
        Task<TransactionResponse> GetAPYAsync(string tokenName, int chainId);
        Task<TransactionResponse> MaxAvailableSharesAsync(string tokenName, int chainId);


    }
}
