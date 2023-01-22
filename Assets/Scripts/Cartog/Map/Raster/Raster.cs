using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Cartog.Map.Raster
{
    public class Raster
    {
        public enum RasterOrigin { NorthWest, NorthEast, SouthWest, SouthEast };

        IRasterFunction rasterFunction;

        Dictionary<int, Vector2[]> cache = new Dictionary<int, Vector2[]>();
        Dictionary<int, Coordinates[]> coordinatesCache = new Dictionary<int, Coordinates[]>();

        Rect cachedRect;
        RasterOrigin origin;

        public Raster(IRasterFunction rasterFunction, RasterOrigin origin = RasterOrigin.NorthEast)
        {
            this.rasterFunction = rasterFunction;
            this.origin = origin;
        }

        Vector2 GetSortOrigin(Rect rect)
        {
            switch (origin)
            {
                case RasterOrigin.NorthWest:
                    return new Vector2(rect.xMin, rect.yMax);
                case RasterOrigin.NorthEast:
                    return new Vector2(rect.xMax, rect.yMax);
                case RasterOrigin.SouthWest:
                    return new Vector2(rect.xMin, rect.yMin);
                case RasterOrigin.SouthEast:
                    return new Vector2(rect.xMax, rect.yMin);
                default:
                    return rect.center;
            }
        }

        void UpdateNodes(Rect rect, int scale)
        {
            if (!NeedsUpdate(rect, scale)) return;

            var originPos = GetSortOrigin(rect);

            cache[scale] = rasterFunction
                .GetCoordinates(rect, scale)
                .OrderBy(pos => Vector2.SqrMagnitude(new Vector2(0.5f * (pos.x - originPos.x), pos.y - originPos.y)))
                .ToArray();

            coordinatesCache[scale] = cache[scale]
                .Select(vec => new Coordinates(vec))
                .ToArray();

            cachedRect = rect;
        }

        bool NeedsUpdate(Rect rect, int scale)
        {
            return cachedRect == null || !cache.ContainsKey(scale) || cachedRect != rect;
        }

        public enum RasterMarker { Dot, Plus };

        public void DrawRasterOn(Texture2D tex, Color color, int scale = 1, RasterMarker marker = RasterMarker.Dot)
        {
            var rect = new Rect(0, 0, tex.width, tex.height);
            UpdateNodes(rect, scale);

            var nodes = coordinatesCache[scale];

            for (int i = 0; i < nodes.Length; i++)
            {                
                var node = nodes[i];
                if (node.x < 0 || node.y < 0 || node.x >= tex.width || node.y >= tex.height) continue;
                tex.SetPixel(node.x, node.y, color);

                switch (marker)
                {
                    case RasterMarker.Plus:
                        if (node.x - 1 >= 0) tex.SetPixel(node.x - 1, node.y, color);
                        if (node.x + 1 < tex.width) tex.SetPixel(node.x + 1, node.y, color);
                        if (node.y - 1 >= 0) tex.SetPixel(node.x, node.y - 1, color);
                        if (node.y + 1 < tex.height) tex.SetPixel(node.x, node.y + 1, color);
                        break;
                }
            }
        }

        public void DrawItemOn(RasterizedItem item, Texture2D tex, Texture2D mask = null)
        {
            var rect = new Rect(0, 0, tex.width, tex.height);
            int scale = item.metadata.rasterScale;

            Vector2 offset = new Vector2(item.texture.width, item.texture.height) / 2;

            UpdateNodes(rect, scale);
            var nodes = cache[scale];
            var coordinates = coordinatesCache[scale];

            for (int i = 0; i < nodes.Length; i++)
            {
                var coord = coordinates[i];
                if (mask == null || mask.GetPixel(coord.x, coord.y).a != 0)
                {
                    var noise = new Vector2(
                        Random.Range(-item.metadata.rasterPositionNoise, item.metadata.rasterPositionNoise) * offset.x,
                        Random.Range(-item.metadata.rasterPositionNoise, item.metadata.rasterPositionNoise) * offset.y
                    );

                    item.texture.BlitOnto(tex, new Coordinates(nodes[i] - offset + noise));                    
                }                
            }
        }
    }
}
