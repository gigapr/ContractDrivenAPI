namespace API.CoinDesk
{
    public class BitcoinPriceIndex
    {
        public string code { get; set; }
        public string symbol { get; set; }
        public string rate { get; set; }
        public string description { get; set; }
        public float rate_float { get; set; }
    }
}