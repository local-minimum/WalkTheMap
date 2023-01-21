using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MessagePack;

namespace Cartog.Map.Raster
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
        public readonly string rasterId;

        /// <summary>
        /// Icons with lowest priority are drawn first to the target texture
        /// </summary>
        [Key(3)]
        public readonly int drawPriority;

        /// <summary>
        /// Relative distance from raster position that icon may be placed
        /// </summary>
        [Key(4), Range(0, 2), Tooltip("0 = No noise, 1 = Distance to closest neighbour")]
        public readonly float rasterPositionNoise;

        /// <summary>
        /// Scale of raster
        /// </summary>
        [Key(5)]
        public readonly int rasterScale;

        /// <summary>
        /// Causes legen to present items in certain order
        /// </summary>
        [Key(6)]
        public readonly int legendPriority;

        public RasterizedMetadata(string nameIdentifier, string rasterId, byte iconLayerId)
        {
            this.nameIdentifier = nameIdentifier;
            iconLayer = new MapIconLayer(0, iconLayerId);
            this.rasterId = rasterId;
            rasterPositionNoise = 0;
            rasterScale = 1;
            legendPriority = 0;
            drawPriority = 0;
        }

        public RasterizedMetadata(
            string nameIdentifier, 
            MapIconLayer iconLayerId, 
            string rasterId,
            int drawPriority,
            float rasterPositionNoise, 
            int rasterScale, 
            int legendPriority
        )
        {
            this.nameIdentifier = nameIdentifier;
            iconLayer = iconLayerId;
            this.rasterId = rasterId;
            this.drawPriority = drawPriority;
            this.rasterPositionNoise = rasterPositionNoise;
            this.rasterScale = rasterScale;
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

        public RasterizedMetadata Mutate(
            string rasterId = null,
            float? rasterPositionNoise = null,
            int? rasterScale = null,
            int? drawPriority = null,
            int? legendPriority = null
        ) => new RasterizedMetadata(
                nameIdentifier,
                iconLayer,
                rasterId ?? this.rasterId,
                drawPriority ?? this.drawPriority,
                rasterPositionNoise ?? this.rasterPositionNoise,
                rasterScale ?? this.rasterScale,
                legendPriority ?? this.legendPriority
        );
    }
}