using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using API.CoinDesk;
using Newtonsoft.Json;

namespace Consumer.Tests.Utils
{
    internal class TestWebClient
    {
        public (HttpStatusCode, CoinDeskResponse) Result(HttpStatusCode httpStatusCode, CoinDeskResponse coinDeskResponse)
        {
            return (httpStatusCode, coinDeskResponse);
        }
        
        private readonly string _baseUrl;

        internal TestWebClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<Tuple<HttpStatusCode, CoinDeskResponse>> Get(string endpoint)
        {
            using (var request = new HttpRequestMessage(HttpMethod.Get, endpoint))
            {
                request.Headers.Add("Accept", "application/json");

                using (var client = new HttpClient { BaseAddress = new Uri(_baseUrl) })
                {
                    using (var response = await client.SendAsync(request))
                    {
                        var content = await response.Content.ReadAsStringAsync();
                        var status = response.StatusCode;
                        CoinDeskResponse result = null;
                        
                        if (status == HttpStatusCode.OK)
                        {
                            result = !string.IsNullOrEmpty(content) ?
                                JsonConvert.DeserializeObject<CoinDeskResponse>(content)
                                : null;
                        }

                        return new Tuple<HttpStatusCode, CoinDeskResponse>(status, result);
                    }
                }
            }
        }
    }
}