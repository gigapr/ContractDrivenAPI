namespace API.CoinDesk
{
    public class CoinDeskResponse
    {
        public CoinDeskTime time { get; set; }
        
        public BitcoinPriceIndexes bpi { get; set; }
    }
}