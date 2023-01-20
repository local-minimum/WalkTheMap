using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace Cartog.Map
{
    [MessagePackObject]
    public struct RasterizedMetadata
    {
        /// <summary>
        /// Describes where icon should be placed
        /// </summary>
        [Key(1)]
        public readonly MapIconLayer iconLayer;

        /// <summary>
        /// Language identifier string for map icon
        /// </summary>
        [Key(0)]
        public readonly string nameIdentifier;

        /// <summary>
        /// Identity of raster layer configuration for icon placements
        /// </summary>
        [Key(2)]
        public readonly int rasterLayer;

        /// <summary>
        /// Relative distance from raster position that icon may be placed
        /// </summary>
        [Key(3), Range(0, 2), Tooltip("0 = No noise, 1 = Distance to closest neighbour")]
        public readonly float rasterPositionNoise;

        /// <summary>
        /// Allows for scaling of connected image
        /// </summary>
        [Key(4)]
        public readonly float imageScale;

        /// <summary>
        /// Causes legen to present items in certain order
        /// </summary>
        [Key(5)]
        public readonly int legendPriority;

        public RasterizedMetadata(string nameIdentifier, byte iconLayerId)
        {
            this.nameIdentifier = nameIdentifier;
            iconLayer = new MapIconLayer(0, iconLayerId);
            rasterLayer = 0;
            rasterPositionNoise = 0;
            imageScale = 1;
            legendPriority = 0;
        }

        public RasterizedMetadata(string nameIdentifier, MapIconLayer iconLayerId, int rasterLayer, float rasterPositionNoise, float imageScale, int legendPriority)
        {
            this.nameIdentifier = nameIdentifier;
            iconLayer = iconLayerId;
            this.rasterLayer = rasterLayer;
            this.rasterPositionNoise = rasterPositionNoise;
            this.imageScale = imageScale;
            this.legendPriority = legendPriority;
        }

        /// <summary>
        /// Qualified name identifier used in language tool to human language
        /// </summary>
        [IgnoreMember]
        public string QualifiedNameIdentifier
        {
            get
            {
                return $"map.icon.{nameIdentifier}";
            }
        }
    }
}