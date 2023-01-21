using System.Collections.Generic;
using System.Linq;
using Cartog.IO;
using Cartog.Map.Raster;


namespace Cartog.Map
{
    public class MapLegend
    {
        List<RasterizedItem> rasterItems;
        IAdaptor adaptor;

        public MapLegend(IAdaptor adaptor)
        {
            this.adaptor = adaptor;
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

        public bool Unsaved
        {
            get
            {
                return rasterItems.Any(item => item.Dirty);
            }
        }

        public void SaveAll()
        {
            rasterItems.ForEach(item => { if (item.Dirty) item.Save(adaptor); });
        }
    }
}
