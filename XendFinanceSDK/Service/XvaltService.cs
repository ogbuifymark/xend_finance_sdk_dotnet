using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
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
    public class XvaltService : IXvaltService
    {

        IWeb3Client _web3Client;
        string protocolName = "xVault";
        private Assets _assets;
        public XvaltService( IWeb3Client web3Client)
        {

            _web3Client = web3Client;
            _assets = new Assets();

        }


        /// <summary>
        /// DepositAndWaitForReceiptAsync
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="depositAmount">this is the amount to be deposited</param>
        /// <param name="tokenName">token name</param>
        /// <param name="cancellationTokenSource">this is the cancellation token</param>
        /// <returns>Returns an object(TransactionResponse)</returns>
        public async Task<TransactionResponse> DepositAndWaitForReceiptAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource = null)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                BigInteger newDepositAmount = Utility.FormatAmount(depositAmount, chainId, tokenName);

                TransactionResponse transactionAproveResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.tokenAddress, layer2TokenInfo.tokenAbi, "approve", GasPriceLevel.Average, cancellationTokenSource, layer2TokenInfo.protocolAddress, newDepositAmount);
                if (!transactionAproveResponse.IsSuccessful)
                    throw new Exception("Approval for xVault deposit failed");

                TransactionResponse transactionResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi, "deposit", GasPriceLevel.Average, cancellationTokenSource, newDepositAmount);
                if (!transactionResponse.IsSuccessful)
                    throw new Exception("xVault deposit failed");

                return transactionResponse;



            }
            catch (Exception ex)
            {
                return new TransactionResponse
                {
                    IsSuccessful = false,
                    data = ex,
                    message = ex.Message
                };
            }
        }

        /// <summary>
        /// DepositAsync 
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="depositAmount">this is the amount to be deposited</param>
        /// <param name="tokenName">token name</param>
        /// <param name="cancellationTokenSource">this is the cancellation token</param>
        /// <returns>Returns the transactionHash containing status and data</returns>
        public async Task<TransactionResponse> DepositAsync(int chainId, decimal depositAmount, string tokenName, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                BigInteger newDepositAmount = Utility.FormatAmount(depositAmount, chainId, tokenName);

                TransactionResponse transactionResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.tokenAddress, layer2TokenInfo.tokenAbi, "approve", GasPriceLevel.Average, cancellationTokenSource, layer2TokenInfo.protocolAddress, newDepositAmount);
                if (!transactionResponse.IsSuccessful)
                    throw new Exception("Approval for xVault deposit failed");

                string transactionHash = await _web3Client.SendTransactionAsync(layer2TokenInfo.network, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi, "deposit", GasPriceLevel.Average,  newDepositAmount);
                if (string.IsNullOrWhiteSpace(transactionHash))
                    throw new Exception("xVault deposit failed");

                return new TransactionResponse
                {
                    IsSuccessful = true,
                    TransactionHash = transactionHash,
                };




            }
            catch (Exception ex)
            {
                return new TransactionResponse
                {
                    IsSuccessful = false,
                    data = ex,
                    message = ex.Message
                };
            }
        }


        /// <summary>
        /// WithdrawalAndWaitForReceiptAsync
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="amount">this is the amount to be deposited</param>
        /// <param name="tokenName">token name</param>
        /// <param name="cancellationTokenSource">this is the cancellation token</param>
        /// <returns>Returns an object(TransactionResponse)</returns>
        public async Task<TransactionResponse> WithdrawalAndWaitForReceiptAsync(int chainId, decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName,chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }

                string senderAddress = await _web3Client.PrivateKeyToAddress();


                Contract contract = _web3Client.GetContract(chainId, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi);
                Function ppfsFunction = contract.GetFunction(layer2TokenInfo.ppfsMethod);

                var ppfs = await ppfsFunction.CallAsync<long>();
                var divisor = Math.Pow(10, layer2TokenInfo.decimals);

                BigInteger withdrawalAmount = ((BigInteger)((double)amount * Math.Pow(10,18))* (BigInteger)divisor) / ppfs;


                TransactionResponse transactionResponse = await _web3Client.SendTransactionAndWaitForReceiptAsync(layer2TokenInfo.network, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi, "withdraw", GasPriceLevel.Average, cancellationTokenSource, withdrawalAmount, senderAddress, 0);

                return transactionResponse;

            }
            catch (Exception ex)
            {
                return new TransactionResponse
                {
                    IsSuccessful = false,
                    data = ex,
                    message = ex.Message
                };

            }
        }

        /// <summary>
        /// WithdrawalAsync
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="amount">this is the amount to be deposited</param>
        /// <param name="tokenName">token name</param>
        /// <param name="cancellationTokenSource">this is the cancellation token</param>
        /// <returns>Returns the transactionHash containing status and data</returns>
        public async Task<TransactionResponse> WithdrawalAsync(int chainId, decimal amount, string tokenName, CancellationTokenSource cancellationTokenSource)
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

                return new TransactionResponse
                {
                    IsSuccessful = true,
                    TransactionHash = transactionHash,
                };

            }
            catch (Exception ex)
            {
                return new TransactionResponse
                {
                    IsSuccessful = false,
                    data = ex,
                    message = ex.Message
                };

            }
        }
        /// <summary>
        /// GetPricePerFullShare
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="tokenName">token name</param>
        /// <returns>Returns an object(TransactionResponse) containing status and data</returns>

        public async System.Threading.Tasks.Task<TransactionResponse> GetPricePerFullShare(int chainId,string tokenName)
        {
            try
            {
                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                Contract contract = _web3Client.GetContract(chainId, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi);
                Function ppfsFunction = contract.GetFunction(layer2TokenInfo.ppfsMethod);
                BigInteger ppfs = await ppfsFunction.CallAsync<BigInteger>();

                return new TransactionResponse
                {
                    IsSuccessful = true,
                    data = ppfs
                };

            }
            catch (Exception ex)
            {
                return new TransactionResponse
                {
                    IsSuccessful = false,
                    data = ex,
                    message = ex.Message
                };
            }

        }
        /// <summary>
        /// GetShareBalance
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="tokenName">token name</param>
        /// <returns>Returns an object(TransactionResponse) containing status and data</returns>
        public async System.Threading.Tasks.Task<TransactionResponse> GetShareBalance(int chainId, string tokenName)
        {
            try
            {
                string senderAddress = await _web3Client.PrivateKeyToAddress();

                Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
                if (layer2TokenInfo == null)
                {
                    throw new Exception("No token found");
                }
                Contract contract = _web3Client.GetContract(chainId, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi);
                Function contractFunction = contract.GetFunction("balanceOf");
                BigInteger share = await contractFunction.CallAsync<BigInteger>(senderAddress);


                return new TransactionResponse
                {
                    IsSuccessful = true,
                    data = share
                };

            }
            catch (Exception ex)
            {

                return new TransactionResponse
                {
                    IsSuccessful = false,
                    data = ex,
                    message = ex.Message
                };
            }

        }

        /// <summary>
        /// GetAPYAsync
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="tokenName">token name</param>
        /// <returns>Returns an object(TransactionResponse) </returns>
        public async Task<TransactionResponse> GetAPYAsync(int chainId, string tokenName)
        {
            Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
            if (layer2TokenInfo == null)
            {
                throw new Exception("No token found");
            }
            Contract contract = _web3Client.GetContract(chainId, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi);
            Function contractFunction = contract.GetFunction("getApy");
            BigInteger apy = await contractFunction.CallAsync<BigInteger>();

            return new TransactionResponse
            {
                IsSuccessful = true,
                data = apy
            };
        }


        /// <summary>
        /// MaxAvailableSharesAsync
        /// </summary>
        /// <param name="chainId">this is the chain id of the network</param>
        /// <param name="tokenName">token name</param>
        /// <returns>Returns an object(TransactionResponse) </returns>

        public async Task<TransactionResponse> MaxAvailableSharesAsync(int chainId, string tokenName)
        {
            Layer2TokenInfo layer2TokenInfo = _assets.FilterToken(tokenName, chainId, protocolName);
            if (layer2TokenInfo == null)
            {
                throw new Exception("No token found");
            }
            Contract contract = _web3Client.GetContract(chainId, layer2TokenInfo.protocolAddress, layer2TokenInfo.protocolAbi);
            Function contractFunction = contract.GetFunction("maxAvailableShares");
            BigInteger balance = await contractFunction.CallAsync<BigInteger>();
            return new TransactionResponse
            {
                IsSuccessful = true,
                data = balance
            };
        }

    }
}
