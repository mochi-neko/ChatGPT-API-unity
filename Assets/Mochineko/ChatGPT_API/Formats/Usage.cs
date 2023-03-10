#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API.Formats
{
    [JsonObject]
    public sealed class Usage
    {
        [JsonProperty("prompt_tokens")] public uint PromptTokens { get; }
        [JsonProperty("completion_tokens")] public uint CompletionTokens { get; }
        [JsonProperty("total_tokens")] public uint TotalTokens { get; }
    }
}