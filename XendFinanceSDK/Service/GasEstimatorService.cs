using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Threading.Tasks;
using XendFinanceSDK.Models;
using XendFinanceSDK.Util.Interface;
using static XendFinanceSDK.Models.Enums;

namespace XendFinanceSDK.Service
{
    internal class GasEstimatorService : IGasEstimatorService
    {
        private readonly HttpClient _bscHttpClient;
        private readonly HttpClient _polygonHttpClient;
        private readonly string _bscGasEstimateUrl;
        private readonly string _polygonGasEstimateUrl;

        private ConcurrentDictionary<int, GasEstimateResponse> gasEstimates = new ConcurrentDictionary<int, GasEstimateResponse>();
        private const int TTL_IN_MINUTES = 5;

        public GasEstimatorService(string bscGasEstimateUrl, string polygonGasEstimateUrl, IHttpClientFactory httpClientFactory)
        {
            _bscGasEstimateUrl = bscGasEstimateUrl;
            _polygonGasEstimateUrl = polygonGasEstimateUrl;
            _bscHttpClient = httpClientFactory.CreateClient("xendfinance-dotnet-sdk-gas-client-bsc");
            _polygonHttpClient = httpClientFactory.CreateClient("xendfinance-dotnet-sdk-gas-client-polygon");

        }



        /// <summary>
        /// Get's the estimated gas cost for a blockchain network
        /// The gas estimate gotten is cached and API calls to the Gas estimate API's ar eonly made once an entry is not found in the cache
        /// </summary>
        /// <param name="network"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task<GasEstimateResponse> EstimateGas(int chainId)
        {

            GasEstimateResponse gasEstimate = GetFromInMemory(chainId);
            if (gasEstimate != null)
            {
                return gasEstimate;
            }

            switch (chainId)
            {
                case (int)ChainId.BSCMainnet:
                    gasEstimate = await EstimateGasBSC();
                    break;
                case (int)ChainId.BSCTestnet:
                    gasEstimate = await EstimateGasBSC();
                    break;
                case (int)ChainId.PolygonMainnet:
                    gasEstimate = await EstimateGasPolygon();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unsupported Network Chain");
            }
            AddToInMemory(chainId, gasEstimate);
            return gasEstimate;
        }

        private async Task<GasEstimateResponse> EstimateGasBSC()
        {
            HttpResponseMessage httpResponseMessage = await _bscHttpClient.GetAsync(_bscGasEstimateUrl);
            httpResponseMessage.EnsureSuccessStatusCode();
            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            BSCGasEstimateResponse response = JsonConvert.DeserializeObject<BSCGasEstimateResponse>(contentString);
            return new GasEstimateResponse
            {
                AverageGas = response.Result.ProposeGasPrice,
                FastGas = response.Result.FastGasPrice,
                LowGas = response.Result.SafeGasPrice
            };
        }

        private async Task<GasEstimateResponse> EstimateGasPolygon()
        {
            HttpResponseMessage httpResponseMessage = await _polygonHttpClient.GetAsync(_polygonGasEstimateUrl);
            httpResponseMessage.EnsureSuccessStatusCode();
            string contentString = await httpResponseMessage.Content.ReadAsStringAsync();
            PolygonGasEstimateResponse response = JsonConvert.DeserializeObject<PolygonGasEstimateResponse>(contentString);
            return new GasEstimateResponse
            {
                AverageGas = response.Result.ProposeGasPrice,
                FastGas = response.Result.FastGasPrice,
                LowGas = response.Result.SafeGasPrice
            };
        }

        private void AddToInMemory(int chainId, GasEstimateResponse response)
        {
            response.UpdatedDate = DateTime.UtcNow;
            gasEstimates[chainId] = response;
        }

        private GasEstimateResponse GetFromInMemory(int chainId)
        {
            if (!gasEstimates.ContainsKey(chainId))
            {
                return null;
            }

            double minutesElapsed = (DateTime.UtcNow - gasEstimates[chainId].UpdatedDate).TotalMinutes;
            if (minutesElapsed > TTL_IN_MINUTES)
            {
                gasEstimates[chainId] = null;
            }

            return gasEstimates[chainId];
        }
    }
}
