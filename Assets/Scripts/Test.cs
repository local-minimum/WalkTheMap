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

    [SerializeField, Range(0, 40)]
    int oddRowOffset = 20;

    [SerializeField, Range(1, 10)]
    int rasterScale = 1;
    
    Texture2D tex;
    IAdaptor adaptor;
    Raster raster;

    void Start()
    {
        adaptor = new UnityProjectDriveStorage(location);

        //NewEmptySprite();
        DoLegend();

        InitRaster();

        tex = new Texture2D(size, size);
        sr.sprite = Sprite.Create(tex, new Rect(0, 0, size, size), Vector2.one * 0.5f);        
    }

    void NewEmptySprite()
    {
        item = new RasterizedItem("Foo2", "Bar", 1);
        item.Save(adaptor);
    }


    void InitRaster()
    {
        raster = new Raster(
            new RasterFunctionParallelogram("Bar", Vector2.one * spacing, oddRowOffset)
        );
    }

    void DoLegend()
    {
        var legend = new MapLegend(adaptor);
        Debug.Log(string.Join(",", legend.SeasonItems(0)));

        item = legend.SeasonItems(0).First();
        item.MutateMetadata(rasterPositionNoise: 0.1f);
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
        Fill(Color.gray);

        raster.DrawItemOn(item, tex);
        raster.DrawRasterOn(tex, Color.magenta, rasterScale, Raster.RasterMarker.Plus);

        tex.Apply();
    }
}
