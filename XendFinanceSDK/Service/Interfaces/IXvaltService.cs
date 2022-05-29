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
        Task<TransactionResponse> DepositAndWaitForReceiptAsync(decimal depositAmount, string tokenName, CancellationToken cancellationTokenSource);
        Task<string> DepositAsync(decimal depositAmount, string tokenName, CancellationToken cancellationTokenSource);
        Task<TransactionResponse> WithdrawalAndWaitForReceiptAsync(decimal amount, string tokenName, CancellationToken cancellationTokenSource);
        Task<string> WithdrawalAsync(decimal amount, string tokenName, CancellationToken cancellationTokenSource);
        Task<Response> GetPricePerFullShare(string tokenName, string protocolName);
        Task<Response> GetShareBalance(string tokenName, string protocolName);
        Task<Response> GetAPYAsync(string tokenName, int chainId);
        Task<Response> MaxAvailableSharesAsync(string tokenName, int chainId);


    }
}
