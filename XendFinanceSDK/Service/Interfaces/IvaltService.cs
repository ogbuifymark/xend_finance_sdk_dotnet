using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Models;

namespace XendFinanceSDK.Service.Interfaces
{
    public interface IvaltService
    {
        Task<Response> Deposit(decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource = null);
        Task<Response> Withdrawal(decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource = null);
        
    }
}
