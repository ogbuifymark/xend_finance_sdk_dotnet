using System;
using System.Collections.Generic;
using System.Text;

namespace XendFinanceSDK.Models
{
  

    public class Layer2TokenInfo
    {
       
        public string name { get; set; }
        public string logo { get; set; }
        public string tokenAddress { get; set; }
        public string tokenAbi { get; set; }
        public string protocolName { get; set; }
        public string protocolAddress { get; set; }
        public string protocolAbi { get; set; }
        public int network { get; set; }
        public int decimals { get; set; }
        public int widthdrawDecimals { get; set; }
        public string ppfsMethod { get; set; }
    }

}
