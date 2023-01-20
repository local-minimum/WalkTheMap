using MessagePack;

namespace Cartog.Map
{
    [MessagePackObject]
    public struct MapIconLayer
    {
        /// <summary>
        /// Seasons allows for swapping out icons based on e.g. seasons
        /// and other temporal effects.
        /// 
        /// Season 0 is special and will include all icons so it will
        /// cause collisions if two variants exists for the same
        /// iconLayerId
        /// </summary>
        [Key(0)]
        public readonly byte seasonLayers;

        /// <summary>
        /// A unique id for the icon and layer combination, if
        /// season layer 0 is used in the project it must not
        /// include icon layers with seasonal variants.
        /// </summary>
        [Key(1)]
        public readonly byte iconLayerId;

        public MapIconLayer(byte seasonLayers, byte iconLayerId)
        {
            if (iconLayerId == 0) throw new System.ArgumentException("Icon layer id 0 is reserved as empty");
            this.seasonLayers = seasonLayers;
            this.iconLayerId = iconLayerId;
        }

        public uint getSeasonValue(byte seasonId)
        {
            if (seasonId >= 8) throw new System.ArgumentException($"Maxium season allowed is 7 (got {seasonId})");
            return (uint)iconLayerId << (8 * seasonId);
        }
    }
}