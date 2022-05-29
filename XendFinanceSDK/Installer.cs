using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using XendFinanceSDK.Models;
using XendFinanceSDK.Service;
using XendFinanceSDK.Service.Interfaces;
using XendFinanceSDK.Util;
using XendFinanceSDK.Util.Interface;
using static XendFinanceSDK.Models.Enums;

namespace XendFinanceSDK
{
    public static class SdkInstaller
    {
        public static void AddXendFinanceSdk(this IServiceCollection services,
                                             string privateKey,
                                             GasPriceLevel gasPriceLevel = GasPriceLevel.Average)
        {
            RegisterServices(services, GasEstimateUrls.BSCGasEstimateUrl, GasEstimateUrls.PolygonGasEstimateUrl);
            IServiceProvider serviceProvider = services.BuildServiceProvider();
            services.AddSingleton<IWeb3Client>(x => new Web3Client(privateKey, ChainIds.BSCMainnet, ChainIds.PolygonMainnet, RPCNodeUrls.BSC_MAINNET, RPCNodeUrls.POLYGON_MAINNET, gasPriceLevel, (IGasEstimatorService)serviceProvider.GetService(typeof(IGasEstimatorService)))); ;
        }

        public static void AddXendFinanceSdk(this IServiceCollection services,
                                             string privateKey,
                                             GasPriceLevel gasPriceLevel = GasPriceLevel.Average,
                                             BlockchainEnvironment environment = BlockchainEnvironment.Mainnet)
        {
            RegisterServices(services, GasEstimateUrls.BSCGasEstimateUrl, GasEstimateUrls.PolygonGasEstimateUrl);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            switch (environment)
            {
                case BlockchainEnvironment.Mainnet:
                    services.AddSingleton<IWeb3Client>(x => new Web3Client(privateKey, ChainIds.BSCMainnet, ChainIds.PolygonMainnet, RPCNodeUrls.BSC_MAINNET, RPCNodeUrls.POLYGON_MAINNET, gasPriceLevel, (IGasEstimatorService)serviceProvider.GetService(typeof(IGasEstimatorService))));
                    break;
                case BlockchainEnvironment.Testnet:
                    services.AddSingleton<IWeb3Client>(x => new Web3Client(privateKey, ChainIds.BSCTestnet, ChainIds.PolygonTestnet, RPCNodeUrls.BSC_TESTNET, RPCNodeUrls.POLYGON_TESTNET, gasPriceLevel, (IGasEstimatorService)serviceProvider.GetService(typeof(IGasEstimatorService))));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Found unsupported blochchain environment");
            }
        }


        public static void AddXendFinanceSdk(this IServiceCollection services,
                                            string privateKey,
                                            string bscNodeUrl = null,
                                            string polygonNodeUrl = null,
                                            string bscGasEstimateUrl = null,
                                            string polygonGasEstimateUrl = null,
                                            GasPriceLevel gasPriceLevel = GasPriceLevel.Average,
                                            BlockchainEnvironment environment = BlockchainEnvironment.Mainnet)
        {
            RegisterServices(services, bscGasEstimateUrl, polygonGasEstimateUrl);
            IServiceProvider serviceProvider = services.BuildServiceProvider();

            switch (environment)
            {
                case BlockchainEnvironment.Mainnet:
                    services.AddSingleton<IWeb3Client>(x => new Web3Client(privateKey, ChainIds.BSCMainnet, ChainIds.PolygonMainnet, bscNodeUrl ?? RPCNodeUrls.BSC_MAINNET, polygonNodeUrl ?? RPCNodeUrls.POLYGON_MAINNET, gasPriceLevel, (IGasEstimatorService)serviceProvider.GetService(typeof(IGasEstimatorService))));
                    break;
                case BlockchainEnvironment.Testnet:
                    services.AddSingleton<IWeb3Client>(x => new Web3Client(privateKey, ChainIds.BSCTestnet, ChainIds.PolygonTestnet, bscNodeUrl ?? RPCNodeUrls.BSC_TESTNET, polygonNodeUrl ?? RPCNodeUrls.POLYGON_TESTNET, gasPriceLevel, (IGasEstimatorService)serviceProvider.GetService(typeof(IGasEstimatorService))));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Found unsupported blochchain environment");
            }
        }

        private static void RegisterServices(IServiceCollection services, string bscGasEstimateUrl, string polygonGasEstimateUrl)
        {
            services.AddSingleton<IGasEstimatorService>(x =>
            {
                IHttpClientFactory httpClientFactory = x.GetRequiredService<IHttpClientFactory>();
                return new GasEstimatorService(bscGasEstimateUrl, polygonGasEstimateUrl, httpClientFactory);
            });
            services.AddTransient<IXvaltService, XvaltService>();
        }

    }
}

