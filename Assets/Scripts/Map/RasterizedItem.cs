using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using System.IO;

namespace Cartog.Map
{
    public class RasterizedItem
    {

        string sourceId { get; set; }
        public Texture2D texture { get; private set; }
        public RasterizedMetadata metadata { get; private set; }

        public RasterizedItem(string sourceId, byte iconLayerId)
        {
            this.sourceId = sourceId;
            metadata = new RasterizedMetadata(sourceId, iconLayerId);
            texture = new Texture2D(64, 64);
        }

        private RasterizedItem(string sourceId, byte[] rawMetadata, byte[] texturePng)
        {
            this.sourceId = sourceId;
            metadata = MessagePackSerializer.Deserialize<RasterizedMetadata>(rawMetadata);

            if (sourceId != metadata.nameIdentifier) {
                Debug.LogWarning($"Rasterized item with id {sourceId} identified by other name: {metadata.nameIdentifier}");
            }
            texture = new Texture2D(2, 2);
            texture.LoadImage(texturePng);
        }

        static string MetadataPath(string basePath) => $"{basePath}.data";

        static private byte[] LoadMetadata(string basePath)
        {
            var dataFile = MetadataPath(basePath);
            if (File.Exists(dataFile))
            {
                return File.ReadAllBytes(dataFile);
            }
            else
            {
                throw new System.ArgumentException($"Missing file at {dataFile}");
            }
        }

        static string PngPath(string basePath) => $"{basePath}.png";
        static private byte[] LoadPng(string basePath)
        {
            var imageFile = PngPath(basePath);
            if (File.Exists(imageFile))
            {
                return File.ReadAllBytes(imageFile);
            }
            else
            {
                throw new System.ArgumentException($"Missing file at {imageFile}");
            }
        }

        public static RasterizedItem Load(string path, string id)
        {
            var basePath = Path.Combine(path, id);
            
            byte[] rawMetadata = LoadMetadata(basePath);
            byte[] texturePng = LoadPng(basePath);

            return new RasterizedItem(id, rawMetadata, texturePng);
            
        }

        public void Save(string path)
        {
            var metadataBytes = MessagePackSerializer.Serialize(metadata);
            var pngBytes = texture.EncodeToPNG();
            var basePath = Path.Combine(path, sourceId);

            File.WriteAllBytes(PngPath(basePath), pngBytes);
            File.WriteAllBytes(MetadataPath(basePath), metadataBytes);
        }
    }
}