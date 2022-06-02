using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Service.Interfaces;

namespace SdkClientWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SDKController : ControllerBase
    {
        IXvaltService _xvaltService;
        public SDKController(IXvaltService xvaltService)
        {
            _xvaltService = xvaltService;
        }
        public IActionResult Deposit([FromBody] DepositRequest depositRequest )
        {
            var response = _xvaltService.DepositAndWaitForReceiptAsync(depositRequest.chainId, depositRequest.amount, depositRequest.tokenName, CancellationTokenSource.CreateLinkedTokenSource());
            return Ok(response);
        }
    }
}
