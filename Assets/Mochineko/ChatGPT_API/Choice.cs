#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class Choice
    {
        [JsonProperty("message"), JsonRequired]
        public Message Message { get; private set; } = new Message();
        
        [JsonProperty("finish_reason"), JsonRequired]
        public string FinishReason { get; private set; } = string.Empty;
        
        [JsonProperty("index"), JsonRequired]
        public uint Index { get; private set; }
    }
}