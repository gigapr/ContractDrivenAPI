namespace API.Models
{
    public class PriceIndex
    {
        public string Code { get;  }
        public string Symbol { get;  }
        public double Rate { get; }

        public PriceIndex(string code, string symbol, double rate)
        {
            Code = code;
            Symbol = symbol;
            Rate = rate;
        }
    }
}