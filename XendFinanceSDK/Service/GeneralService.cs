using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using XendFinanceSDK.Environment;
using XendFinanceSDK.Models;
using XendFinanceSDK.Service.Interfaces;
using XendFinanceSDK.Util;
using XendFinanceSDK.Util.Interface;

namespace XendFinanceSDK.Service
{
    public class GeneralService: IGeneralService
    {
        string _provider, _privateKey, _chainId;
        IWeb3Client _web3Client;
        private Assets _assets;
        public GeneralService(string provider, Assets assets, string privateKey, string chainId, IWeb3Client web3Client)
        {

            _privateKey = privateKey;
            _chainId = chainId;
            _provider = provider;
            _web3Client = web3Client;
            _assets = assets;

        }
        public async System.Threading.Tasks.Task<Response> Approve(decimal depositAmount, string tokenName, string protocolName, CancellationTokenSource cancellationTokenSource = null)
        {
            try
            {
                string senderAddress = await _web3Client.PrivateKeyToAddress(_privateKey);
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, Convert.ToInt32(_chainId), protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                //Contract web3  = utility.CreateContract(_provider, AbiConfig.IndividualAbi, _addressees.PERSONAL);
                Contract tokenContract = await _web3Client.CreateContract(_provider, _chainId, layer2TokenInfo.tokenAbi, layer2TokenInfo.tokenAddress, _privateKey);
                Function approveFunction = tokenContract.GetFunction("approve");


                BigInteger newDepositAmount = Utility.FormatAmount(depositAmount, Convert.ToInt32(_chainId), tokenName);

                HexBigInteger gasLimit = await approveFunction.EstimateGasAsync(senderAddress, null, null, layer2TokenInfo.protocolAddress, newDepositAmount);
                HexBigInteger gasPrice = new HexBigInteger(BigInteger.Parse((5 * Math.Pow(10, 9)).ToString())); // 5 Gwei


                var approvalReceipt = await approveFunction.SendTransactionAndWaitForReceiptAsync(senderAddress, gasLimit, gasPrice, null, cancellationTokenSource, layer2TokenInfo.protocolAddress, newDepositAmount);

                if ((int)approvalReceipt.Status.Value == 1)
                {
                    return new Response
                    {
                        status = true,
                        data = approvalReceipt.Status.Value
                    };
                }
                else
                {
                    return new Response
                    {
                        status = false,
                        data = approvalReceipt.Status.Value
                    };
                }

            }
            catch (Exception ex)
            {
                //Todo: implement logger
                throw;
            }
        }


        public async System.Threading.Tasks.Task<Response> ApproveSederAddress(decimal depositAmount, string tokenName, string protocolName, CancellationTokenSource cancellationTokenSource = null)
        {
            try
            {
                string senderAddress = await _web3Client.PrivateKeyToAddress(_privateKey);
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, Convert.ToInt32(_chainId), protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                //Contract web3  = utility.CreateContract(_provider, AbiConfig.IndividualAbi, _addressees.PERSONAL);
                Contract tokenContract = await _web3Client.CreateContract(_provider, _chainId, layer2TokenInfo.tokenAbi, layer2TokenInfo.tokenAddress, _privateKey);
                Function approveFunction = tokenContract.GetFunction("approve");


                BigInteger newDepositAmount = Utility.FormatAmount(depositAmount, Convert.ToInt32(_chainId), tokenName);
                HexBigInteger gasLimit = await approveFunction.EstimateGasAsync(senderAddress, null, null, senderAddress, newDepositAmount);
                HexBigInteger gasPrice = new HexBigInteger(BigInteger.Parse(Math.Pow(5, 9).ToString())); // 5 Gwei


                var approvalReceipt = await approveFunction.SendTransactionAndWaitForReceiptAsync(senderAddress, gasLimit, gasPrice, cancellationTokenSource, senderAddress, newDepositAmount);
                if ((int)approvalReceipt.Status.Value == 1)
                {
                    return new Response
                    {
                        status = true,
                        data = approvalReceipt.Status.Value
                    };
                }
                else
                {
                    return new Response
                    {
                        status = false,
                        data = approvalReceipt.Status.Value
                    };
                }

            }
            catch (Exception ex)
            {
                //Todo: implement logger
                throw;
            }
        }
        public async System.Threading.Tasks.Task<Response> GetPricePerFullShare(string tokenName, string protocolName)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, Convert.ToInt32(_chainId), protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                Contract contract = await _web3Client.CreateContract(_provider, _chainId, layer2TokenInfo.protocolAbi, layer2TokenInfo.protocolAddress, _privateKey);
                Function ppfsFunction = contract.GetFunction(layer2TokenInfo.ppfsMethod);
                var ppfs = await ppfsFunction.CallAsync<dynamic>();

                return new Response
                {
                    status = true,
                    data = ppfs
                };

            }
            catch (Exception)
            {

                throw;
            }

        }

        public async System.Threading.Tasks.Task<Response> GetShareBalance(string tokenName, string protocolName)
        {
            try
            {
                string senderAddress = await _web3Client.PrivateKeyToAddress(_privateKey);

                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, Convert.ToInt32(_chainId), protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                Contract contract = await _web3Client.CreateContract(_provider, _chainId, layer2TokenInfo.protocolAbi, layer2TokenInfo.protocolAddress, _privateKey);
                Function contractFunction = contract.GetFunction("balanceOf");
                var share = await contractFunction.CallAsync<dynamic>(senderAddress);

                return new Response
                {
                    status = true,
                    data = share
                };

            }
            catch (Exception)
            {

                throw;
            }

        }


    }
}
