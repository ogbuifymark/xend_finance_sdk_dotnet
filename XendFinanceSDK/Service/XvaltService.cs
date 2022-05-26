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
    public class XvaltService : IvaltService
    {

        string _provider, _privateKey, _chainId;
        IWeb3Client _web3Client;
        string protocolName = "xVault";
        private Assets _assets;
        public XvaltService(string provider, Assets assets, string privateKey, string chainId, IWeb3Client web3Client)
        {

            _privateKey = privateKey;
            _chainId = chainId;
            _provider = provider;
            _web3Client = web3Client;
            _assets = assets;

        }
        /// <summary>
        /// Deposit in Flexible savings
        /// </summary>
        /// <param name="depositAmount">this is the amount to be deposited</param>
        /// <param name="cancellationTokenSource">t</param>

        /// <returns>Returns an object containing status and data</returns>
        

        /// <summary>
        /// Deposit in Flexible savings
        /// </summary>
        /// <param name="depositAmount">this is the amount to be deposited</param>
        /// <param name="cancellationTokenSource">t</param>

        /// <returns>Returns an object containing status and data</returns>
        public async System.Threading.Tasks.Task<Response> Deposit(decimal depositAmount, string tokenName,CancellationTokenSource cancellationTokenSource = null)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, Convert.ToInt32(_chainId), protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                string senderAddress = await _web3Client.PrivateKeyToAddress(_privateKey);
                //Contract web3  = utility.CreateContract(_provider, AbiConfig.IndividualAbi, _addressees.PERSONAL);
                Contract depositContract = await _web3Client.CreateContract(_provider, _chainId, layer2TokenInfo.protocolAbi, layer2TokenInfo.protocolAddress, _privateKey);
                Function depositFunction = depositContract.GetFunction("deposit");


                BigInteger newDepositAmount = Utility.FormatAmount(depositAmount, Convert.ToInt32(_chainId), tokenName);

                HexBigInteger gasLimit = await depositFunction.EstimateGasAsync(senderAddress, null, null, newDepositAmount);
                HexBigInteger gasPrice = new HexBigInteger(BigInteger.Parse((5 *Math.Pow(10, 9)).ToString())); // 5 Gwei


                var depositReceipt = await depositFunction.SendTransactionAndWaitForReceiptAsync(senderAddress, gasLimit, gasPrice, null, cancellationTokenSource,  newDepositAmount);
                return new Response
                {
                    status = true,
                    data = depositReceipt
                };

            }
            catch (Exception ex)
            {
                //Todo: implement logger
                throw;
            }
        }


        /// <summary>
        /// Deposit in Flexible savings
        /// </summary>
        /// <param name="depositAmount">this is the amount to be deposited</param>
        /// <param name="cancellationTokenSource">t</param>

        /// <returns>Returns an object containing status and data</returns>
        public async System.Threading.Tasks.Task<Response> Withdrawal(decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource = null)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, Convert.ToInt32(_chainId), protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                string senderAddress = await _web3Client.PrivateKeyToAddress(_privateKey);
                Contract contract = await _web3Client.CreateContract(_provider, _chainId, layer2TokenInfo.protocolAbi, layer2TokenInfo.protocolAddress, _privateKey);
                Function contractFunction = contract.GetFunction("balanceOf");
                Function ppfsFunction = contract.GetFunction(layer2TokenInfo.ppfsMethod);
                Function withdrawFunction = contract.GetFunction("withdraw");

                var share = await contractFunction.CallAsync<dynamic>(senderAddress);
                var ppfs = await ppfsFunction.CallAsync<dynamic>();
                var divisor = Math.Pow(10, layer2TokenInfo.widthdrawDecimals);

                double totalDeposit = ((long)share * (long)ppfs) / double.Parse(divisor.ToString(), System.Globalization.NumberStyles.Float);
                double withdrawalAmount = ((long)share * (long)amount) / totalDeposit;
                
                withdrawalAmount = Math.Truncate(withdrawalAmount);
                BigInteger newAmount = (BigInteger)withdrawalAmount;




                HexBigInteger gasLimit = await contractFunction.EstimateGasAsync(senderAddress, null, null, newAmount);
                HexBigInteger gasPrice = new HexBigInteger(BigInteger.Parse((5 * Math.Pow(10, 9)).ToString())); // 5 Gwei


                var withdrawalReceipt = await withdrawFunction.SendTransactionAndWaitForReceiptAsync(senderAddress, gasLimit, gasPrice, null, cancellationTokenSource, newAmount, senderAddress,0);
                return new Response
                {
                    status = true,
                    data = withdrawalReceipt
                };

            }
            catch (Exception ex)
            {
                //Todo: implement logger
                return new Response
                {
                    status = false,
                    data = ex
                };

            }
        }

       
    }
}
