using System.Collections.Generic;
using UnityEngine;
using MessagePack;
using Cartog.IO;

namespace Cartog.Map.Raster
{
    [MessagePackObject]
    public struct RasterFunctionParallelogram : IRasterFunction
    {
        static string FileName(string itemId) => $"raster-func-parallel.{itemId}.data";

        [Key(0)]
        public readonly string id;
        [Key(1)]
        public readonly Vector2 spacing;
        [Key(2)]
        public readonly Vector2 baseOffset;
        [Key(3)]
        public readonly float oddRowOffset;
        [Key(4)]
        public readonly float rotation;

        public RasterFunctionParallelogram(string id, Vector2 spacing, float oddRowOffset = 0, float rotation = 0)
        {
            this.id = id;
            this.spacing = spacing;
            baseOffset = new Vector2(0, 0);
            this.oddRowOffset = oddRowOffset;
            this.rotation = rotation;
        }

        [SerializationConstructor]
        public RasterFunctionParallelogram(string id, Vector2 spacing, Vector2 baseOffset, float oddRowOffset = 0, float rotation = 0)
        {
            this.id = id;
            this.spacing = spacing;
            this.baseOffset = baseOffset;
            this.oddRowOffset = oddRowOffset;
            this.rotation = rotation;
        }

        public IEnumerable<Vector2> GetCoordinates(Rect area, int scale = 1)
        {
            if (scale < 1) throw new System.ArgumentException($"Scale {scale} is less than one");
            //TODO: Make rotation!

            int yi = 0;
            for (var y=area.yMin + baseOffset.y; y<=area.yMax; y+=spacing.y * scale, yi++)
            {
                if (y < area.yMin) continue;

                for (var x=area.xMin + baseOffset.x + (yi % 2 != 0 ? oddRowOffset : 0); x <= area.xMax; x+=spacing.x * scale)
                {
                    if (x < area.xMin) continue;

                    yield return new Vector2(x, y);
                }
            }
        }

        public void Save(IAdaptor adaptor)
        {            
            var data = MessagePackSerializer.Serialize(this);
            adaptor.Save(FileName(id), data);
        }

        public override string ToString()
        {
            List<string> settings = new List<string>();

            settings.Add($"s={spacing}");
            if (baseOffset.sqrMagnitude != 0)
            {
                settings.Add($"o={baseOffset}");
            }
            if (rotation != 0)
            {
                settings.Add($"a={rotation}");
            }
            if (oddRowOffset != 0)
            {
                settings.Add($"skew={oddRowOffset}");
            }

            var settingsStr = string.Join(", ", settings);

            return $"<RasterFunction Parallelogram ({id}): {settingsStr}>";
        }
    }
}