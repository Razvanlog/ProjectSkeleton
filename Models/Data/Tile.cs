using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TheAdventure.Models.Data
{
    internal class Tile
    {
        [JsonPropertyName("id")]
        public int? Id { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; } = "";
        [JsonPropertyName("imageheight")]
        public int? ImageHeight { get; set; }
        [JsonPropertyName("imagewidth")]
        public int? ImageWidth { get; set; }
        [JsonIgnore]
        public int TextureId { get; set; } = -1;
    }
}
