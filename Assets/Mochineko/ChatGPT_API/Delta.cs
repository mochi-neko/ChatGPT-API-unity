#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class Delta
    {
        [JsonProperty("role")]
        public string? RoleString { get; private set; }
        
        [JsonIgnore]
        public Role? Role => RoleString?.ToRole();

        [JsonProperty("content")]
        public string? Content { get; private set; }
    }
}