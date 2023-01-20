using System.Collections.Generic;
using System.Linq;
using Cartog.IO;


namespace Cartog.Map
{
    public class MapLegend
    {
        List<RasterizedItem> rasterItems;

        public MapLegend(IAdaptor adaptor)
        {
            rasterItems = RasterizedItem
                .LoadMany(adaptor)
                .OrderBy(item => item.metadata.legendPriority)
                .ToList();
        }

        public IEnumerable<RasterizedItem> SeasonItems(byte seasonId)
        {
            if (seasonId == 0) return rasterItems;

            return rasterItems.Where(item => item.metadata.iconLayer.UsedInSeason(seasonId));
        }
    }
}
