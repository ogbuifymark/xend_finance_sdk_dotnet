using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Models;

namespace XendFinanceSDK.Service.Interfaces
{
    public interface IGeneralService
    {
        Task<Response> Approve(decimal depositAmount, string tokenName, string protocolName, CancellationTokenSource cancellationTokenSource = null);
        Task<Response> ApproveSederAddress(decimal depositAmount, string tokenName, string protocolName, CancellationTokenSource cancellationTokenSource = null);
        Task<Response> GetPricePerFullShare(string tokenName, string protocolName);
        Task<Response> GetShareBalance(string tokenName, string protocolName);

    }
}
