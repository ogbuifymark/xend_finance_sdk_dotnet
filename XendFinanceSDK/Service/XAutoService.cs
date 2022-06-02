using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using XendFinanceSDK.Environment;
using XendFinanceSDK.Models;
using XendFinanceSDK.Service.Interfaces;
using XendFinanceSDK.Util;
using XendFinanceSDK.Util.Interface;
using static XendFinanceSDK.Models.Enums;

namespace XendFinanceSDK.Service
{
    public class XAutoService: IXAutoService
    {


        IWeb3Client _web3Client;
        string protocolName = "xAuto";
        private Assets _assets;
        public XAutoService( IWeb3Client web3Client)
        {

            _web3Client = web3Client;
            _assets = new Assets();

        }
        /// <summary>
        /// Deposit in Flexible savings
        /// </summary>
        /// <param name="depositAmount">this is the amount to be deposited</param>
        /// <param name="tokenName">this is the amount to be deposited</param>
        /// <param name="cancellationTokenSource">t</param>

        /// <returns>Returns an object(TransactionResponse) containing status and data</returns>
        public async Task<TransactionResponse> DepositAndWaitForReceiptAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                string senderAddress = await _web3Client.PrivateKeyToAddress();
                BigInteger newDepositAmount = Utility.FormatAmount(depositAmount, chainId, tokenName);

                TransactionResponse transactionResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.tokenAddress, layer2TokenInfo.tokenAbi, "approve", GasPriceLevel.Average, cancellationTokenSource, layer2TokenInfo.protocolAddress, newDepositAmount);
                if (!transactionResponse.IsSuccessful)
                    throw new Exception("Approval for xVault deposit failed");

                transactionResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi, "deposit", GasPriceLevel.Average, cancellationTokenSource, newDepositAmount);
                if (!transactionResponse.IsSuccessful)
                    throw new Exception("xVault deposit failed");

                return transactionResponse;



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
        /// <param name="tokenName">this is the amount to be deposited</param>
        /// <param name="cancellationTokenSource">t</param>

        /// <returns>Returns the transactionHash containing status and data</returns>
        public async Task<string> DepositAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                string senderAddress = await _web3Client.PrivateKeyToAddress();
                BigInteger newDepositAmount = Utility.FormatAmount(depositAmount, chainId, tokenName);

                TransactionResponse transactionResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.tokenAddress, layer2TokenInfo.tokenAbi, "approve", GasPriceLevel.Average, cancellationTokenSource, layer2TokenInfo.protocolAddress, newDepositAmount);
                if (!transactionResponse.IsSuccessful)
                    throw new Exception("Approval for xVault deposit failed");

                string transactionHash = await _web3Client.SendTransactionAsync(layer2TokenInfo.network, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi, "deposit", GasPriceLevel.Average, newDepositAmount);
                if (string.IsNullOrWhiteSpace(transactionHash))
                    throw new Exception("xVault deposit failed");

                return transactionHash;



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
        public async Task<TransactionResponse> WithdrawalAndWaitForReceiptAsync(int chainId, decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }



                string senderAddress = await _web3Client.PrivateKeyToAddress();
                Contract contract = _web3Client.GetContract(chainId, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi);
                Function ppfsFunction = contract.GetFunction(layer2TokenInfo.ppfsMethod);

                var ppfs = await ppfsFunction.CallAsync<long>();
                var divisor = Math.Pow(10, layer2TokenInfo.decimals);

                BigInteger withdrawalAmount = ((BigInteger)((double)amount * Math.Pow(10, 18)) * (BigInteger)divisor) / ppfs;


                TransactionResponse transactionResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi, "withdraw", GasPriceLevel.Average, cancellationTokenSource, withdrawalAmount, senderAddress, 0);

                return transactionResponse;

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
        public async Task<string> WithdrawalAsync(int chainId,decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }



                string senderAddress = await _web3Client.PrivateKeyToAddress();
                Contract contract = _web3Client.GetContract(chainId, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi);
                Function ppfsFunction = contract.GetFunction(layer2TokenInfo.ppfsMethod);

                BigInteger ppfs = await ppfsFunction.CallAsync<BigInteger>();
                var divisor = Math.Pow(10, layer2TokenInfo.decimals);

                BigInteger withdrawalAmount = ((BigInteger)((double)amount * Math.Pow(10, 18)) * (BigInteger)divisor) / ppfs;


                string transactionHash = await _web3Client.SendTransactionAsync(layer2TokenInfo.network, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi, "withdraw", GasPriceLevel.Average, withdrawalAmount, senderAddress, 0);

                return transactionHash;

            }
            catch (Exception ex)
            {
                //Todo: implement logger
                throw;

            }
        }



    }
}
