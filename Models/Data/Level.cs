using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using TheAdventure.Entities.Player;

namespace TheAdventure.Models.Data
{
    internal class Level
    {
        [JsonPropertyName("height")]
        public int? Height { get; set; }
        [JsonPropertyName("width")]
        public int? Width { get; set; }
        [JsonPropertyName("layers")]
        public List<Layer> Layers { get; set; } = new();
        [JsonPropertyName("tilesets")]
        public List<TileSetReference> TileSets { get; set; } = new();
        [JsonPropertyName("tileheight")]
        public int? TileHeight { get; set; }
        [JsonPropertyName("tilewidth")]
        public int? TileWidth { get; set; }
    }
}
