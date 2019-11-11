using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using API.CoinDesk;
using API.Models;
using Consumer.Tests.Utils;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace Consumer.Tests
{
    public class GetPriceIndexTests: IClassFixture<APIPact>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        public GetPriceIndexTests(APIPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions(); 
            _mockProviderServiceBaseUri = APIPact.MockProviderServiceBaseUri;
        }

        [Fact]
        public async Task WhenCryptocurrencyIsNotSupportedReturnsNotFound()
        {
            const string cryptocurrency = "unknownCryptocurrency";
            var endpointUnderTest = $"/api/priceindex/{cryptocurrency}";
            
            _mockProviderService
                .UponReceiving($"A GET request to get price indexes for '{cryptocurrency}' cryptocurrency")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = endpointUnderTest
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = (int) HttpStatusCode.NotFound
                }); 

            var consumer = new TestWebClient(_mockProviderServiceBaseUri);

            var (statusCode, coinDeskResponse) = await consumer.Get(endpointUnderTest);

            Assert.Equal(HttpStatusCode.NotFound, statusCode);
            Assert.Null(coinDeskResponse);
            
            _mockProviderService.VerifyInteractions();
        }
        
        [Fact]
        public async Task WhenCryptocurrencyIsSupportedReturnsPriceIndexes()
        {
            const string cryptocurrency = "bitcoin";
            var endpointUnderTest = $"/api/priceindex/{cryptocurrency}";
            
            _mockProviderService
                .UponReceiving($"A GET request to get price indexes for '{cryptocurrency}' cryptocurrency")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = endpointUnderTest
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Status = (int) HttpStatusCode.OK,
                    Body = new PriceIndexResponse
                    {
                        LastUpdatedIso =  "Nov 11, 2019 at 22:12 GMT",
                        PriceIndexes = new List<PriceIndex>()
                        {
                             new PriceIndex("USD","&#36;",8739.3483),
                             new PriceIndex( "GBP","&pound;",6799.1343),
                             new PriceIndex("EUR","&euro;", 7920.6637)
                        }
                    }
                }); 

            var consumer = new TestWebClient(_mockProviderServiceBaseUri);

            var (statusCode, coinDeskResponse) = await consumer.Get(endpointUnderTest);

            Assert.Equal(HttpStatusCode.OK, statusCode);
            Assert.NotNull(coinDeskResponse);
            
            _mockProviderService.VerifyInteractions();
        }
    }
}