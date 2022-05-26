using System;
using System.Collections.Generic;
using System.Text;

namespace XendFinanceSDK.Models
{
    public class ChainIds
    {
        public const int ETHEREUM_MAINNET = 1;
        public const int BSC_MAINNET = 56;
        public const int BSC_TESTNET = 97;
        public const int POLYGON_MAINNET = 137;
        public const int POLYGON_TESTNET = 80001;
        public const int LOCALHOST = 0;
    }

    public class Provider
    {
        public static NETWORKS ETHEREUM_MAINNET = new NETWORKS
        {
            currency = "DAI",
            url = "https://eth-mainnet.alchemyapi.io/v2/2gdCD03uyFCNKcyEryqJiaPNtOGdsNLv",
            chain = 1
        };
        public static NETWORKS BSC_MAINNET = new NETWORKS
        {
            currency = "BUSD",
            url = "https://bsc-dataseed.binance.org/",
            chain = 56
        };
        public static NETWORKS BSC_TESTNET = new NETWORKS
        {
            currency = "BUSD",
            url = "https://data-seed-prebsc-1-s1.binance.org:8545/",
            chain = 97
        };

        public static NETWORKS POLYGON_MAINNET = new NETWORKS
        {
            currency = "MATIC",
            url = "https://rpc-mainnet.matic.network/",
            chain = 137
        };
        public static NETWORKS POYLGON_TESTNET = new NETWORKS
        {
            currency = "MATIC",
            url = "https://rpc-mumbai.matic.today/",
            chain = 8001
        };
        public static NETWORKS LOCALHOST = new NETWORKS
        {
            currency = "BUSD",
            url = "http://127.0.0.1:8545",
            chain = 0
        };
    }

    public class NETWORKS
    {
        public string currency { get; set; }
        public string url { get; set; }
        public int chain { get; set; }
    }

   

}
