using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using API;
using API.CoinDesk;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using PactNet;
using Producer.Tests.XUnitHelpers;
using Xunit;
using Xunit.Abstractions;

namespace Producer.Tests
{
    public class CoinDeskServiceStub : ICoinDeskService
    {
        public CoinDeskServiceStub(HttpClient client)
        {
            
        }
        
        public Task<CoinDeskResponse> GetBitcoinPriceIndex()
        {
            return Task.FromResult(new CoinDeskResponse
            {
                time = new CoinDeskTime
                {
                    updatedISO = "Nov 11, 2019 at 22:12 GMT"
                },
                bpi = new BitcoinPriceIndexes
                {
                    EUR = new BitcoinPriceIndex {code = "EUR", symbol = "&euro;", rate_float = 7920.6637},
                    GBP = new BitcoinPriceIndex {code = "GBP", symbol = "&pound;", rate_float = 6799.1343},
                    USD = new BitcoinPriceIndex {code = "USD", symbol = "&#36;", rate_float = 8739.3483},
                } 
            });
        }
    }
    
    public class GetPriceIndexTests: IDisposable
    {
        private bool _disposedValue; // To detect redundant calls
        private const string ProviderUri = "http://0.0.0.0:8080";
        private const string PactServiceUri = "http://localhost:9001";
        private readonly IWebHost _pactWebHost;
        private readonly IWebHost _apiWebHost;
        private readonly ITestOutputHelper _outputHelper;

        public GetPriceIndexTests(ITestOutputHelper output)
        {
            _outputHelper = output;
            
            _pactWebHost = WebHost.CreateDefaultBuilder()
                .UseUrls(PactServiceUri)
                .UseStartup<TestStartup>()
                .Build();
            _pactWebHost.Start();

            _apiWebHost = WebHost.CreateDefaultBuilder()
                .ConfigureServices(collection =>
                {
                    collection.AddHttpClient<ICoinDeskService, CoinDeskServiceStub>();
                })
                .UseUrls(ProviderUri)
                .UseStartup<Startup>()
                .Build();
            _apiWebHost.Start();
        }

        [Fact]
        public void EnsureProviderApiHonoursPactWithConsumer()
        {
            var config = new PactVerifierConfig
            {
                Outputters = new List<PactNet.Infrastructure.Outputters.IOutput>{new XUnitOutput(_outputHelper)},
                Verbose = true
            };

            var pactVerifier = new PactVerifier(config);
            pactVerifier.ProviderState($"{PactServiceUri}/provider-states")
                .ServiceProvider("Provider", ProviderUri)
                .HonoursPactWith("Consumer")
                .PactUri(@"..\..\..\..\pacts\consumer-api.json")
                .Verify();
        }


        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue) return;
            
            if (disposing)
            {
                _pactWebHost.StopAsync().GetAwaiter().GetResult();
                _pactWebHost.Dispose();

                _apiWebHost.StopAsync().GetAwaiter().GetResult();
                _apiWebHost.Dispose();
            }

            _disposedValue = true;
        }

        public void Dispose()
        {
            Dispose(true);
        }
    }
}