#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class ChoiceChunk
    {
        [JsonProperty("delta"), JsonRequired]
        public Delta Delta { get; private set; } = new Delta();

        [JsonProperty("index"), JsonRequired]
        public uint Index { get; private set; }
        
        [JsonProperty("finish_reason")]
        public string? FinishReason { get; private set; }
    }
}