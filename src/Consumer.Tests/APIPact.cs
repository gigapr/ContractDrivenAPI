using System;
using PactNet;
using PactNet.Mocks.MockHttpService;

namespace Consumer.Tests
{
    public class APIPact: IDisposable
    {
        private const int MockServerPort = 9222;
        private readonly IPactBuilder _pactBuilder;
        public static readonly string MockProviderServiceBaseUri = $"http://localhost:{MockServerPort}";
        internal IMockProviderService MockProviderService { get; }

        public APIPact()
        {
            _pactBuilder = new PactBuilder(new PactConfig
            {
                PactDir = @"..\..\..\..\pacts", 
                LogDir = @"..\..\..\..\logs"
            });

            _pactBuilder.ServiceConsumer("Consumer").HasPactWith("API");

            
            MockProviderService = _pactBuilder.MockService(MockServerPort);
        }
        public void Dispose()
        {
            _pactBuilder.Build(); //Will save the pact file once finished
        }
    }
}