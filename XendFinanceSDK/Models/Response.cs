using System;
using System.Collections.Generic;
using System.Text;

namespace XendFinanceSDK.Models
{
    public class Response
    {
        public bool status { get; set; }
        public dynamic data { get; set; }
    }
    public class TransactionResponse
    {
        public bool IsSuccessful { get; set; }
        public string BlockHash { get; set; }
        public string TransactionHash { get; set; }
        public string message { get; set; }
        public dynamic data { get; set; }
    }
}
