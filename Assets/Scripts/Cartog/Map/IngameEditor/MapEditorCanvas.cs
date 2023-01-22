using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cartog.Map.Raster;

namespace Cartog.Map.IngameEditor
{
    public class MapEditorCanvas : MonoBehaviour
    {
        [SerializeField]
        int mapHeight = 500;
        public int MapHeight
        {
            get { return mapHeight;  }
        }

        [SerializeField]
        int mapWidth = 500;
        public int MapWidth
        {
            get { return mapWidth; }
        }

        SpriteRenderer sr;
        Texture2D outTexture;
        Texture2D mask;
        Texture2D pencil;

        Raster.Raster activeRaster;
        public Raster.Raster GetRaster(RasterizedItem item)
        {
            // TODO: Get matching
            return activeRaster;
        }


        [SerializeField, Range(30, 80)]
        int spacing = 40;

        [SerializeField, Range(0, 40)]
        int oddRowOffset = 20;

        BoxCollider2D collider;

        public Texture2D GetMask(RasterizedItem item)
        {
            // TODO: Filter on item
            return mask;
        }

        void Start()
        {
            sr = GetComponent<SpriteRenderer>();
            collider = GetComponent<BoxCollider2D>();


            activeRaster = new Raster.Raster(
                new RasterFunctionParallelogram("Bar", Vector2.one * spacing, oddRowOffset)
            );

            outTexture = BlankCanvas();
            outTexture.Fill(Color.gray);
            outTexture.Apply();
            sr.sprite = Sprite.Create(outTexture, new Rect(0, 0, mapWidth, mapHeight), Vector2.one * 0.5f);

            collider.size = new Vector2(mapWidth / 100, mapHeight / 100);

            mask = BlankCanvas();

            DrawCanvas();
            outTexture.Apply();

            InitSquarePencil();
        }

        void InitSquarePencil(int size = 9)
        {
            pencil = new Texture2D(size, size);
            pencil.Fill(Color.black);
            pencil.Apply();
        }

        Texture2D BlankCanvas()
        {
            var tex = new Texture2D(mapWidth, mapHeight);
            tex.Clear();
            tex.Apply();
            return tex;
        }

        bool mayPaint = false;

        void DrawCanvas() {
            outTexture.Clear();
            mask.BlitOnto(outTexture, Coordinates.zero);
            activeRaster.DrawRasterOn(outTexture, Color.magenta, 1, Raster.Raster.RasterMarker.Plus);
        }

        Coordinates MousePixel
        {
            get
            {
                var mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 pos = transform.InverseTransformPoint(mouseWorldPos);
                pos.x = (Mathf.Clamp(pos.x, -mapWidth/200f, mapWidth/200f) + mapWidth/200f) * 100;
                pos.y = (Mathf.Clamp(pos.y, -mapHeight/200f, mapHeight/200f) + mapHeight/200f) * 100;
                return new Coordinates(pos);
            }
        }
        void DrawPosition() {
            var mouse = MousePixel;
            pencil.BlitOnto(mask, mouse - new Coordinates(pencil.width / 2f, pencil.height / 2f));
        }

        private void Update()
        {
            if (!mayPaint) return;

            if (Input.anyKey)
            {
                DrawPosition();
            }

            DrawCanvas();
            outTexture.Apply();
        }

        private void OnMouseOver()
        {
            mayPaint = true;
        }

        private void OnMouseExit()
        {
            mayPaint = false;
        }
    }
}
