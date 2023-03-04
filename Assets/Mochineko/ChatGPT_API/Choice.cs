#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    internal sealed class Choice
    {
        [JsonProperty("message")] public Message Message { get; private set; } = new Message();
        [JsonProperty("finish_reason")] public string FinishReason { get; private set; } = string.Empty;
        [JsonProperty("index")] public uint Index { get; private set; }
    }
}