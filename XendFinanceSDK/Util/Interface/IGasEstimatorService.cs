using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XendFinanceSDK.Models;
using static XendFinanceSDK.Models.Enums;

namespace XendFinanceSDK.Util.Interface
{
    internal interface IGasEstimatorService
    {
        Task<GasEstimateResponse> EstimateGas(int chainId);
    }
}
