 using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Models;
using XendFinanceSDK.Service.Interfaces;

namespace SdkClientWeb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SDKController : ControllerBase
    {
        IXvaltService _xvaltService;
        IXAutoService _xAutoService;
        public SDKController(IXvaltService xvaltService, IXAutoService xAutoService)
        {
            _xvaltService = xvaltService;
            _xAutoService = xAutoService;
        }

        //XValt
        [HttpPost("xaultDeposit")]
        public IActionResult XVaultDeposit([FromBody] RequestDto depositRequest )
        {
            TransactionResponse transactionResponse = _xvaltService.DepositAndWaitForReceiptAsync(depositRequest.chainId, depositRequest.amount.Value, depositRequest.tokenName, CancellationTokenSource.CreateLinkedTokenSource()).Result;

            return Ok(transactionResponse);
        }
        [HttpPost("xVaultWitdraw")]
        public IActionResult XVaultWitdraw([FromBody] RequestDto request)
        {
            TransactionResponse transactionResponse = _xvaltService.WithdrawalAndWaitForReceiptAsync(request.chainId, request.amount.Value, request.tokenName, CancellationTokenSource.CreateLinkedTokenSource()).Result;

            return Ok(transactionResponse);
        }
        [HttpPost("xVaultPpfs")]
        public IActionResult XVaultPpfs([FromBody] RequestDto request)
        {
            TransactionResponse transactionResponse = _xvaltService.GetPricePerFullShare(request.chainId, "BUSD").Result;

            return Ok(transactionResponse);
        }
        [HttpPost("xVaultShareBalance")]
        public IActionResult XVaultShareBalance([FromBody] RequestDto request)
        {
            TransactionResponse transactionResponse = _xvaltService.GetShareBalance(request.chainId, "BUSD").Result;

            return Ok(transactionResponse);
        }
        [HttpPost("xVaultAPy")]
        public IActionResult XVaultAPy([FromBody] RequestDto request)
        {
            TransactionResponse transactionResponse = _xvaltService.GetAPYAsync(request.chainId, "BUSD").Result;

            return Ok(transactionResponse);
        }

        //XAuto
        [HttpPost("xAutoDeposit")]
        public IActionResult XAutoDeposit([FromBody] RequestDto depositRequest)
        {
            TransactionResponse transactionResponse = _xAutoService.DepositAndWaitForReceiptAsync(depositRequest.chainId, depositRequest.amount.Value, depositRequest.tokenName, CancellationTokenSource.CreateLinkedTokenSource()).Result;

            return Ok(transactionResponse);
        }
        [HttpPost("xAutoWitdraw")]
        public IActionResult XAutoWitdraw([FromBody] RequestDto request)
        {
            TransactionResponse transactionResponse = _xAutoService.WithdrawalAndWaitForReceiptAsync(request.chainId, request.amount.Value, request.tokenName, CancellationTokenSource.CreateLinkedTokenSource()).Result;

            return Ok(transactionResponse);
        }
        [HttpPost("xAutoPpfs")]
        public IActionResult XAutoPpfs([FromBody] RequestDto request)
        {
            TransactionResponse transactionResponse = _xAutoService.GetPricePerFullShare(request.chainId, "BUSD").Result;

            return Ok(transactionResponse);
        }

        [HttpPost("xAutoAPy")]
        public IActionResult XAutoAPy([FromBody] RequestDto request)
        {
            TransactionResponse transactionResponse = _xAutoService.GetAPYAsync(request.chainId, "BUSD").Result;

            return Ok(transactionResponse);
        }
    }
}
