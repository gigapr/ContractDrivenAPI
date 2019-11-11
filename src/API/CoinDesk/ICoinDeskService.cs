using System.Threading.Tasks;

namespace API.CoinDesk
{
    public interface ICoinDeskService
    {
        Task<CoinDeskResponse> GetBitcoinPriceIndex();
    }
}