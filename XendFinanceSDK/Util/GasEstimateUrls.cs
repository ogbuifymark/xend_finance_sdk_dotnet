using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XendFinanceSDK.Util
{
    internal static class GasEstimateUrls
    {
        public static string BSCGasEstimateUrl { get { return "https://api.bscscan.com/api?module=gastracker&action=gasoracle&apikey=QR9K3AJJB6QTIDEFNZBQPS7S1N6NURZBTV"; } }
        public static string PolygonGasEstimateUrl { get { return "https://api.polygonscan.com/api?module=gastracker&action=gasoracle&apikey=BVD47FZXG7QDH9UKEYBV6DPF7FAZY92P6J"; } }
    }
}
