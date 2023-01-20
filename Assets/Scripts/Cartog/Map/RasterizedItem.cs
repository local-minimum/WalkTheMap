using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using UnityEngine;
using MessagePack;
using Cartog.IO;

namespace Cartog.Map
{
    public class RasterizedItem
    {
        static string MetadataPath(string itemId) => $"raster-item.{itemId}.data";

        static string PngPath(string itemId) => $"raster-item.{itemId}.png";

        private static Regex idPattern = new Regex(@"^raster-item\.(.*)\.png$");

        private static string FileNameToItemId(string fileName) => 
            idPattern.Matches(fileName).First().Groups.Skip(1).First().Value;

        public static bool Exists(IAdaptor adaptor, string id)
        {
            return adaptor.Exists(MetadataPath(id));
        }

        public static RasterizedItem Load(IAdaptor adaptor, string id)
        {
            byte[] rawMetadata = adaptor.Load(MetadataPath(id));

            if (adaptor.Exists(PngPath(id)))
            {
                byte[] texturePng = adaptor.Load(PngPath(id));

                return new RasterizedItem(id, rawMetadata, texturePng);

            }

            return new RasterizedItem(id, rawMetadata);
        }

        public static IEnumerable<RasterizedItem> LoadMany(IAdaptor adaptor) =>
            adaptor
                .ListAvailable(idPattern)
                .Select(fileName => Load(adaptor, FileNameToItemId(fileName)));

        string sourceId { get; set; }
        public Texture2D texture { get; private set; }
        public RasterizedMetadata metadata { get; private set; }

        /// <summary>
        /// If changes have been made that have not been saved
        /// </summary>
        public bool Dirty { get; private set; }

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

        private RasterizedItem(string sourceId, byte[] rawMetadata)
        {
            this.sourceId = sourceId;
            metadata = MessagePackSerializer.Deserialize<RasterizedMetadata>(rawMetadata);

            if (sourceId != metadata.nameIdentifier)
            {
                Debug.LogWarning($"Rasterized item with id {sourceId} identified by other name: {metadata.nameIdentifier}");
            }
        }

        public void Save(IAdaptor adaptor)
        {
            var metadataBytes = MessagePackSerializer.Serialize(metadata);
            var pngBytes = texture.EncodeToPNG();

            adaptor.Save(MetadataPath(sourceId), metadataBytes);
            adaptor.Save(PngPath(sourceId), pngBytes);
        }

        public override string ToString()
        {
            var imageText = texture == null ? "No Image" : $"{texture.width}x{texture.height}";
            return $"<RasterizedItem {metadata.QualifiedNameIdentifier} | {imageText}>";
        }
    }
}