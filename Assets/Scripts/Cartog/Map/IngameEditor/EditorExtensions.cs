using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cartog.Map.IngameEditor
{
    public static class EditorExtensions
    {
        public static void Fill(this Texture2D tex, Color color)
        {
            for (int x = 0; x < tex.width; x++)
            {
                for (int y = 0; y < tex.height; y++)
                {
                    tex.SetPixel(x, y, color);
                }
            }
        }
        
        public static void Clear(this Texture2D tex)
        {
            tex.Fill(new Color(0, 0, 0, 0));
        }
    }
}