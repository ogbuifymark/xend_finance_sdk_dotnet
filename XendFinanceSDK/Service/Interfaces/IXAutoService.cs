using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Models;

namespace XendFinanceSDK.Service.Interfaces
{
    public interface IXAutoService
    {
        Task<TransactionResponse> DepositAndWaitForReceiptAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<TransactionResponse> DepositAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<TransactionResponse> WithdrawalAndWaitForReceiptAsync(int chainId, decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<TransactionResponse> WithdrawalAsync(int chainId, decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource);
        Task<TransactionResponse> GetPricePerFullShare(int chainId, string tokenName);
        //Task<TransactionResponse> GetShareBalance(int chainId, string tokenName);
        Task<TransactionResponse> GetAPYAsync(int chainId, string tokenName);
    }
}
