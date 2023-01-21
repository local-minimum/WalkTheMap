using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Cartog.Map.Raster
{
    public class Raster
    {
        struct Coordinates
        {
            public readonly int x;
            public readonly int y;

            public Coordinates(Vector2 vec)
            {
                x = Mathf.RoundToInt(vec.x);
                y = Mathf.RoundToInt(vec.y);
            }
        }

        IRasterFunction rasterFunction;

        Dictionary<int, Vector2[]> cache = new Dictionary<int, Vector2[]>();
        Dictionary<int, Coordinates[]> coordinatesCache = new Dictionary<int, Coordinates[]>();

        Rect cachedRect;

        public Raster(IRasterFunction rasterFunction)
        {
            this.rasterFunction = rasterFunction;
        }

        void UpdateNodes(Rect rect, int scale)
        {
            if (!NeedsUpdate(rect, scale)) return;

            cache[scale] = rasterFunction.GetCoordinates(rect, scale).ToArray();
            coordinatesCache[scale] = cache[scale].Select(vec => new Coordinates(vec)).ToArray();
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
                tex.SetPixel(node.x, node.y, color);

                switch (marker)
                {
                    case RasterMarker.Plus:
                        tex.SetPixel(node.x - 1, node.y, color);
                        tex.SetPixel(node.x + 1, node.y, color);
                        tex.SetPixel(node.x, node.y - 1, color);
                        tex.SetPixel(node.x, node.y + 1, color);
                        break;
                }
            }
        }

        void Blit(Texture2D source, Texture2D target, Coordinates origin)
        {
            for (int sourceX = 0, sourceWidth = source.width; sourceX < sourceWidth; sourceX++)
            {
                int targetX = origin.x + sourceX;

                if (targetX < 0 || targetX >= target.width) continue;

                for (int sourceY = 0, sourceHeight = source.height; sourceY < sourceHeight; sourceY++)
                {
                    int targetY = origin.y + sourceY;
                    if (targetY < 0 || targetY >= target.height) continue;

                    var color = source.GetPixel(sourceX, sourceY);
                    if (color.a == 0) continue;

                    target.SetPixel(targetX, targetY, color);
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
                    var noise = Random.Range(-item.metadata.rasterPositionNoise, item.metadata.rasterPositionNoise);
                    Blit(item.texture, tex, new Coordinates(nodes[i] - offset + noise * offset));
                }                
            }
        }
    }
}
