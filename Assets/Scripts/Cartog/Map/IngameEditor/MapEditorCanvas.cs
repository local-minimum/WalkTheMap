using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cartog.Map.IngameEditor
{
    public class MapEditorCanvas : MonoBehaviour
    {
        [SerializeField]
        int mapHeight = 500;
        [SerializeField]
        int mapWidth = 500;

        SpriteRenderer sr;
        Texture2D outTexture;
        Texture2D layerTexture;

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();

            outTexture = BlankCanvas();
            outTexture.Fill(Color.gray);
            outTexture.Apply();
            sr.sprite = Sprite.Create(outTexture, new Rect(0, 0, mapWidth, mapHeight), Vector2.one * 0.5f);

            layerTexture = BlankCanvas();
        }

        Texture2D BlankCanvas()
        {
            var tex = new Texture2D(mapWidth, mapHeight);
            tex.Clear();
            tex.Apply();
            return tex;
        }            
    }
}
