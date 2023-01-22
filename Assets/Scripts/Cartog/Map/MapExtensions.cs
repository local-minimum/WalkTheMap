using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cartog.Map
{
    public static class MapExtensions
    {
        public static void BlitOnto(this Texture2D source, Texture2D target, Coordinates origin)
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
    }
}