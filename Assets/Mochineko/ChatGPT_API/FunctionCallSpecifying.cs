#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class FunctionCallSpecifying
    {
        [JsonProperty("name"), JsonRequired]
        public string Name { get; private set; }
        
        public FunctionCallSpecifying(string name)
        {
            this.Name = name;
        }
    }
}