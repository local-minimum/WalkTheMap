using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Cartog.Map;
using Cartog.Map.Raster;
using Cartog.IO;

public class Test : MonoBehaviour
{
    [SerializeField]
    string id;

    [SerializeField]
    string[] location;

    [SerializeField]
    SpriteRenderer sr;

    RasterizedItem item;

    [SerializeField]
    int size = 500;

    [SerializeField, Range(30, 80)]
    int spacing = 40;
    [SerializeField, Range(0, 30)]
    int noise = 8;
    
    Texture2D tex;
    
    void Start()
    {
        var adaptor = new UnityProjectDriveStorage(location);
        var legend = new MapLegend(adaptor);
        Debug.Log(string.Join(",", legend.SeasonItems(0)));

        item = legend.SeasonItems(0).First();
        tex = new Texture2D(size, size);
       
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, size, size), Vector2.one * 0.5f);
    }

    void DrawForrest()
    {
        int trees = size / spacing + 3;
        for (int i = 0; i < trees; i++)
        {
            for (int j = 0; j < trees; j++)
            {
                Blit(
                    item.texture,
                    tex,
                    i * spacing + Random.Range(-noise, noise) + (j % 2 == 0 ? spacing / 2 : 0),
                    j * spacing + Random.Range(-noise, noise)
                    );
            }
        }

        tex.Apply();
    }

    void Blit(Texture2D source, Texture2D target, int centerX, int centerY)
    {
        // Debug.Log($"({centerX}, {centerY})");
        int halfSourceHeight = source.height / 2;
        int halfSourceWidth = source.width / 2;

        for (int sourceX = 0, sourceWidth = source.width; sourceX < sourceWidth; sourceX++)
        {
            int targetX = centerX - halfSourceWidth + sourceX;

            if (targetX < 0 || targetX >= target.width) continue;

            for (int sourceY = 0, sourceHeight = source.height; sourceY < sourceHeight; sourceY++)
            {
                int targetY = centerY - halfSourceHeight + sourceY;
                if (targetY < 0 || targetY >= target.height) continue;

                var color = source.GetPixel(sourceX, sourceY);
                if (color.a == 0) continue;

                target.SetPixel(targetX, targetY, color);
            }
        }
    }
    
    void Fill(Color32 color)
    {
        for (int x = 0; x<tex.width; x++)
        {
            for (int y = 0; y<tex.height; y++)
            {
                tex.SetPixel(x, y, color);
            }
        }
    }

    private void Update()
    {
        Fill(Color.white);
        DrawForrest();
    }
}
