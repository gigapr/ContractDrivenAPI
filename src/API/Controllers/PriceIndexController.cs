using System;
using System.Net;
using System.Threading.Tasks;
using API.CoinDesk;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/priceindex")]
    [ApiController]
    public class PriceIndexController : ControllerBase
    {
        private readonly ICoinDeskService _coinDeskService;

        public PriceIndexController(ICoinDeskService coinDeskService)
        {
            _coinDeskService = coinDeskService;
        }

        [HttpGet]
        public async Task<ActionResult<PriceIndexResponse>> Get(string cryptocurrency)
        {
            try
            {                    
                switch (cryptocurrency.ToLower())
                {
                    case "bitcoin":
                        return Ok(PriceIndexResponseMapper.Map(await _coinDeskService.GetBitcoinPriceIndex()));
                    default:
                        return StatusCode((int) HttpStatusCode.NotFound, $"'{cryptocurrency}' is not a supported cryptocurrency. Supported cryptocurrencies are ['bitcoin']");
                }
            }
            catch (Exception)
            {
                return StatusCode((int) HttpStatusCode.InternalServerError, $"Unable to fetch price indexes for '{cryptocurrency}'");
            }
        }
    }
}