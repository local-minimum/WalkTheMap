using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cartog.Map.Raster;
using Cartog.IO;
using System.Linq;

namespace Cartog.Map.IngameEditor
{
    public class MapEditorRenderer : MonoBehaviour
    {
        SpriteRenderer sr;
        Texture2D outTexture;

        [SerializeField]
        Color backgroundColor = Color.gray;

        [SerializeField]
        MapEditorCanvas canvas;

        [SerializeField]
        string[] location;

        IAdaptor adaptor;
        MapLegend legend;

        byte activeSeason = 0;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();

            outTexture = new Texture2D(canvas.MapWidth, canvas.MapHeight);
            outTexture.Fill(backgroundColor);
            outTexture.Apply();
            sr.sprite = Sprite.Create(outTexture, new Rect(0, 0, outTexture.width, outTexture.height), Vector2.one * 0.5f);

            adaptor = new UnityProjectDriveStorage(location);
            legend = new MapLegend(adaptor);

        }

        void Update()
        {
            legend.SeasonItems(activeSeason).ToList().ForEach(item => {
                canvas.GetRaster(item).DrawItemOn(item, outTexture, canvas.GetMask(item));                
            });
            outTexture.Apply();
        }
    }
}
