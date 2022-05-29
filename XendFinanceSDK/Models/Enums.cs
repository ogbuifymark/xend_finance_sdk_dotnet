using System;
using System.Collections.Generic;
using System.Text;

namespace XendFinanceSDK.Models
{
    public class Enums
    {
        public enum GasPriceLevel
        {
            Slow = 1,
            Average,
            Fast
        }
        public enum Networks
        {
            BSC = 1,
            POLYGON
        }
        public enum BlockchainEnvironment
        {
            Mainnet = 1,
            Testnet
        }
        public enum ChainId
        {
            BSCMainnet = 56,
            PolygonMainnet = 137,
            BSCTestnet = 97

        }
    }
}
