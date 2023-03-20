#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class Usage
    {
        [JsonProperty("prompt_tokens")] public int PromptTokens { get; private set; }
        [JsonProperty("completion_tokens")] public int CompletionTokens { get; private set; }
        [JsonProperty("total_tokens")] public int TotalTokens { get; private set; }
    }
}