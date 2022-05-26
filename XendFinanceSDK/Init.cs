using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XendFinanceSDK.Environment;
using XendFinanceSDK.Models;
using XendFinanceSDK.Service;
using XendFinanceSDK.Service.Interfaces;
using XendFinanceSDK.Util;
using XendFinanceSDK.Util.Interface;

namespace XendFinanceSDK
{
    public class Init
    {
        public readonly IvaltService _xvaltService;
        public readonly IvaltService _xautotService;
        int _chainId;
        string _privateKey;
        NETWORKS _network;
        Assets _assets;
        string _protocols;
        IWeb3Client _web3Client;
        IGeneralService _generalService;

        public Init(int chainId, string privateKey)
        {
            _chainId = chainId;
            _privateKey = privateKey;
            _network = checkChainId(chainId);
            _web3Client = new Web3Client(_network.url);
            _assets = new Assets();
            _generalService = new GeneralService(_network.url, _assets, privateKey, chainId.ToString(), _web3Client);
            _xvaltService = new XvaltService(_network.url, _assets,privateKey, chainId.ToString(), _web3Client);
            _xautotService = new XAutoService(_network.url, _assets, privateKey, chainId.ToString(), _web3Client);

        }
        public async Task<Response> ApproveXAutoTransaction( string token, decimal amount)
        {
            return await _generalService.Approve(amount, token, "xAuto");
        }
        public async Task<Response> ApproveXValutTransaction(string token, decimal amount )
        {
            return await _generalService.Approve(amount, token, "xVault");
        }
        public async Task<Response> XVaultPricePerFullShare(string token)
        {
            return await _generalService.GetPricePerFullShare( token, "xVault");
        }
        public async Task<Response> XAutoPricePerFullShare(string token)
        {
            return await _generalService.GetPricePerFullShare(token, "xAuto");
        }
        public async Task<Response> XAutoShareBalance(string token)
        {
            return await _generalService.GetShareBalance(token, "xAuto");
        }
        public async Task<Response> XValtShareBalance(string token)
        {
            return await _generalService.GetPricePerFullShare(token, "xVault");
        }
        public async Task<Response> XAutoDepositTransaction(string token, decimal amount)
        {
            //await _generalService.ApproveSederAddress(amount, token, "xAuto");
            return await _xautotService.Deposit(amount, token);
        }

        public async Task<Response> XValtDepositTransaction(string token, decimal amount)
        {
            return await _xvaltService.Deposit(amount, token);
        }
        public async Task<Response> XAutoWithdrawTransaction(string token, decimal amount)
        {
            return await _xautotService.Withdrawal(amount, token);
        }
        public async Task<Response> XValtWithdraawTransaction(string token, decimal amount)
        {
            return await _xvaltService.Withdrawal(amount, token);
        }
        private NETWORKS checkChainId(int chainId)
        {

            switch (chainId)
            {
                case ChainIds.ETHEREUM_MAINNET:
                    return Provider.ETHEREUM_MAINNET;

                case ChainIds.BSC_TESTNET:
                    return Provider.BSC_TESTNET;

                case ChainIds.BSC_MAINNET:
                    return Provider.BSC_MAINNET;

                case ChainIds.LOCALHOST:
                    return Provider.LOCALHOST;

                default: return Provider.LOCALHOST;
            }

        }


    }

}
