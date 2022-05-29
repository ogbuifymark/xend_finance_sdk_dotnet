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
        Task<TransactionResponse> DepositAndWaitForReceiptAsync(decimal depositAmount, string tokenName, CancellationToken cancellationTokenSource);
        Task<string> DepositAsync(decimal depositAmount, string tokenName, CancellationToken cancellationTokenSource);
        Task<TransactionResponse> WithdrawalAndWaitForReceiptAsync(decimal amount, string tokenName, CancellationToken cancellationTokenSource);
        Task<string> WithdrawalAsync(decimal amount, string tokenName, CancellationToken cancellationTokenSource);
    }
}
