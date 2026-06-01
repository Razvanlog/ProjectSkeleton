using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace TheAdventure.Models.Data
{
    internal class Layer
    {
        [JsonPropertyName("data")]
        public List<int?> Data { get; set; } = new();
        [JsonPropertyName("height")]
        public int? Height { get; set; }
        [JsonPropertyName("width")]
        public int? Width { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; } = "";
    }
}
