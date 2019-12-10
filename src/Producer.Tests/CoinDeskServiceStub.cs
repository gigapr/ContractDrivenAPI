using System.Net.Http;
using System.Threading.Tasks;
using API.CoinDesk;

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
}