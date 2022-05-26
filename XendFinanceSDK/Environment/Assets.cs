using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XendFinanceSDK.Models;

namespace XendFinanceSDK.Environment
{
    public class Assets
    {
        public List<Layer2TokenInfo> assets = new List<Layer2TokenInfo>();
        //bsc
        public const string  BUSD_BSC_XVault = "0x3de1Fe0039EC99773DBEE5608823FECDeFB8D9D0";
        public const string USDC_BSC_XVault = "0x50c9fBf77CBC8FF1b23a8ED61725C325bedC3C86";
        public const string USDT_BSC_XVault = "0x454d6F10B18f391adD499cE39aCD5bFCD424B601";
        public const string BUSD_BSC_XAuto = "0x0f28698FD6A0771CB099482305BeEd0EeCB458D5";
        public const string USDC_BSC_XAuto = "0xa3003c67C0C8fF2280b282F0A821e95fEBA47293";
        public const string USDT_BSC_XAuto = "0x9607be08acFeB47Ea7e66b494Dd5dAb88Dda59cf";
        public const string USDT_BNB_XAuto = "0x8C709c792700d73e37D8B0A4CD3bcc995d03f084";
        //matic
        public const string USDT_Matic_XAuto = "0x143afc138978Ad681f7C7571858FAAA9D426CecE";
        public const string USDC_Matic_XAuto = "0xd01a0971F03D0ddC8D621048d92A1632b2dB7356";
        public const string AAVE_Matic_XAuto = "0xDD3afc5D5476FC327812B84ae2ccf66C011e6d67";
        public const string WBTC_Matic_XAuto = "0x0b26E76D8617b20Ec9fe0811BE2dCbF3438cc27F";

        //Token Addresses
        public const string BUSD_BSC = "0xe9e7CEA3DedcA5984780Bafc599bD69ADd087D56";
        public const string USDC_BSC = "0x8AC76a51cc950d9822D68b83fE1Ad97B32Cd580d";
        public const string USDT_BSC = "0x55d398326f99059fF775485246999027B3197955";

        public const string AAVE_MATIC = "0xd6df932a45c0f255f85145f286ea0b292b21c90b";
        public const string WBTC_MATIC = "0x1bfd67037b42cf73acf2047067bd4f2c47d9bfd6";
        public const string USDT_MATIC = "0xc2132D05D31c914a87C6611C10748AEb04B58e8F";
        public const string USDC_MATIC = "0x2791bca1f2de4661ed88a30c99a7a9449aa84174";


        public Assets()
        {

            Layer2TokenInfo usdc = new Layer2TokenInfo
            {
                name= "USDC",
		        logo= "",
		        tokenAddress= USDC_BSC,
		        tokenAbi= AbiConfig.Erc20,
		        protocolName= "xVault",
		        protocolAddress= USDC_BSC_XVault,
		        protocolAbi= AbiConfig.XVaultUSDCV2,
		        network= 56,
		        decimals= 18,
		        widthdrawDecimals= 36,
		        ppfsMethod= "pricePerShare",
            };
            Layer2TokenInfo usdt = new Layer2TokenInfo
            {
                name= "USDT",
		        logo= "",
		        tokenAddress= USDT_BSC,
		        tokenAbi= AbiConfig.Erc20,
		        protocolName= "xVault",
		        protocolAddress= USDT_BSC_XVault,
		        protocolAbi= AbiConfig.XVaultUSDTV2,
		        network= 56,
		        decimals= 18,
		        widthdrawDecimals= 36,
		        ppfsMethod= "pricePerShare",
            };
            Layer2TokenInfo busd_erc = new Layer2TokenInfo
            {
                name = "BUSD",
                logo = "",
                tokenAddress = BUSD_BSC,
                tokenAbi = AbiConfig.Erc20,
                protocolName = "xVault",
                protocolAddress = BUSD_BSC_XVault,
                protocolAbi = AbiConfig.XVaultBUSDV2,
                network = 56,
                decimals = 18,
                widthdrawDecimals = 36,
                ppfsMethod = "pricePerShare",
            };
			//Layer2TokenInfo busd_erc = new Layer2TokenInfo
			//{
			//	name = "BUSD",
			//	logo = "",
			//	tokenAddress = BUSD_BSC,
			//	tokenAbi = AbiConfig.Erc20,
			//	protocolName = "xVault",
			//	protocolAddress = BUSD_BSC_XVault,
			//	protocolAbi = AbiConfig.XVaultBUSDV2,
			//	network = 56,
			//	decimals = 18,
			//	widthdrawDecimals = 36,
			//	ppfsMethod = "pricePerShare",
			//};
			Layer2TokenInfo busd = new Layer2TokenInfo
            {
                name= "BUSD",
		        logo= "",
		        tokenAddress= BUSD_BSC,
		        tokenAbi= AbiConfig.Busd,
		        protocolName= "xAuto",
		        protocolAddress= BUSD_BSC_XAuto,
		        protocolAbi= AbiConfig.xvAutoBUSDV2,
		        network= 56,
		        decimals= 18,
		        widthdrawDecimals= 36,
		        ppfsMethod= "getPricePerFullShare",
            };
            Layer2TokenInfo usdt_bsc = new Layer2TokenInfo
            {
                name= "USDT",
		        logo= "",
		        tokenAddress= USDT_BSC,
		        tokenAbi= AbiConfig.Erc20,
		        protocolName= "xAuto",
		        protocolAddress= USDT_BSC_XAuto,
		        protocolAbi= AbiConfig.xvAutoUSDTV2,
		        network= 56,
		        decimals= 18,
		        widthdrawDecimals= 36,
                ppfsMethod = "getPricePerFullShare",
            };

            Layer2TokenInfo usdt_bnb = new Layer2TokenInfo
            {
                name= "BNB",
		        logo= "",
		        tokenAddress= "",
		        tokenAbi= AbiConfig.Erc20,
		        protocolName= "xAuto",
		        protocolAddress= USDT_BNB_XAuto,
		        protocolAbi= AbiConfig.xvAutoBNBV2,
		        network= 56,
		        decimals= 18,
		        widthdrawDecimals= 36,
		        ppfsMethod= "getPricePerFullShare",
            };

            Layer2TokenInfo usdt_usdc = new Layer2TokenInfo
            {
			    name= "USDC",
				logo= "",
				tokenAddress= USDC_BSC,
				tokenAbi= AbiConfig.Erc20,
				protocolName = "xAuto",
				protocolAddress= USDC_BSC_XAuto,
				protocolAbi= AbiConfig.xvAutoBUSDV2,
				network= 56,
				decimals= 18,
				widthdrawDecimals= 36,
				ppfsMethod= "getPricePerFullShare",
            };
			Layer2TokenInfo usdc_matic = new Layer2TokenInfo
			{
				name = "USDC",
				logo = "",
				tokenAddress = USDC_MATIC,
				tokenAbi = AbiConfig.Erc20,
				protocolName = "xAuto",
				protocolAddress = USDC_Matic_XAuto,
				protocolAbi = AbiConfig.xvMaticAutoUSDCV2,
				network = 137,
				decimals = 18,
				widthdrawDecimals = 24,
				ppfsMethod = "getPricePerFullShare",
			};
			Layer2TokenInfo usdt_matic = new Layer2TokenInfo
			{
				name = "USDT",
				logo = "",
				tokenAddress = USDT_MATIC,
				tokenAbi = AbiConfig.Erc20,
				protocolName = "xAuto",
				protocolAddress = USDT_Matic_XAuto,
				protocolAbi = AbiConfig.xvMaticAutoUSDTV2,
				network = 137,
				decimals = 18,
				widthdrawDecimals = 24,
				ppfsMethod = "getPricePerFullShare",
			};
			Layer2TokenInfo aave_matic = new Layer2TokenInfo
			{
				name = "AAVE",
				logo = "",
				tokenAddress = AAVE_MATIC,
				tokenAbi = AbiConfig.Erc20,
				protocolName = "xAuto",
				protocolAddress = AAVE_Matic_XAuto,
				protocolAbi = AbiConfig.xvMaticAutoAAVEV2,
				network = 137,
				decimals = 18,
				widthdrawDecimals = 36,
				ppfsMethod = "getPricePerFullShare",
			};
			Layer2TokenInfo wbtc_matic = new Layer2TokenInfo
			{
				name = "WBTC",
				logo = "",
				tokenAddress = WBTC_MATIC,
				tokenAbi = AbiConfig.Erc20,
				protocolName = "xAuto",
				protocolAddress = WBTC_Matic_XAuto,
				protocolAbi = AbiConfig.xvMaticAutoWBTCV2,
				network = 137,
				decimals = 18,
				widthdrawDecimals = 26,
				ppfsMethod = "getPricePerFullShare",
			};

            assets.Add(usdc);
            assets.Add(usdt);
			assets.Add(busd_erc);
			assets.Add(busd);
			assets.Add(usdt_bsc);
			assets.Add(usdt_bnb);
			assets.Add(usdt_usdc);
			assets.Add(usdc_matic);
			assets.Add(usdt_matic);
			assets.Add(aave_matic);
			assets.Add(wbtc_matic);
		}
		public Layer2TokenInfo FilterToken(string tokenName, int chainId, string protocolName)
		{
			return  assets.Where(prop => prop.name == tokenName && prop.network == chainId && prop.protocolName == protocolName).FirstOrDefault();
		}

	}
   
       
    
}
