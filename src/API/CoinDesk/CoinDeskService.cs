using System.Net.Http;
using System.Threading.Tasks;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace API.CoinDesk
{
    public class CoinDeskService : ICoinDeskService
    {
        private readonly HttpClient _client;
        
        public CoinDeskService(HttpClient client)
        {
            _client = client;
        }

        public async Task<CoinDeskResponse> GetBitcoinPriceIndex()
        {
            var request = new HttpRequestMessage(HttpMethod.Get,"bpi/currentprice.json");

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();
            
            var responseStream = await response.Content.ReadAsStreamAsync();
            {
                return await JsonSerializer.DeserializeAsync<CoinDeskResponse>(responseStream);
            }
        }
    }
}