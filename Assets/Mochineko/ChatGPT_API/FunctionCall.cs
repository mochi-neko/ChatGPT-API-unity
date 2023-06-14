#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class FunctionCall
    {
        [JsonProperty("name"), JsonRequired]
        public string Name { get; private set; } = string.Empty;
        
        [JsonProperty("arguments"), JsonRequired]
        public string Arguments { get; private set; } = string.Empty;
    }
}