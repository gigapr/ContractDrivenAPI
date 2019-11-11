using System.Collections.Generic;
using API.CoinDesk;
using API.Models;

namespace API.Controllers
{
    internal static class PriceIndexResponseMapper
    {
        internal static PriceIndexResponse Map(CoinDeskResponse data)
        {
            return new PriceIndexResponse
            {
                LastUpdatedIso = data.time.updatedISO,
                PriceIndexes = new List<PriceIndex>
                {
                    Map(data.bpi.EUR),
                    Map(data.bpi.GBP),
                    Map(data.bpi.USD),
                }
            };
        }

        private static PriceIndex Map(BitcoinPriceIndex priceIndex)
        {
            return new PriceIndex(priceIndex.code, priceIndex.symbol, priceIndex.rate_float);
        }
    }
}