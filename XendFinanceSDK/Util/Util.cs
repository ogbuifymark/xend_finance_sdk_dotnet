using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace XendFinanceSDK.Util
{
    public class Utility
    {
        public static BigInteger FormatAmount( decimal amount, int chainId, string assetName)
        {
            if (chainId == 137)
            {

                if (assetName == "WBTC")
                {
                    return (BigInteger)((double)amount * Math.Pow(10, 8));
                }
                if (assetName == "AAVE")
                {
                    return Web3.Convert.ToWei(amount, Nethereum.Util.UnitConversion.EthUnit.Ether);
                }
                return Web3.Convert.ToWei(amount, Nethereum.Util.UnitConversion.EthUnit.Mwei);
            }
            else return Web3.Convert.ToWei(amount, Nethereum.Util.UnitConversion.EthUnit.Ether);
        }
        
    }

}
