using System.Collections.Generic;

namespace API.Models
{
    public class PriceIndexResponse
    {
        public string LastUpdatedIso { get; set; }
        public IList<PriceIndex> PriceIndexes { get; set; }
    }
}