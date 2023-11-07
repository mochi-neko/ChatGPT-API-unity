#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class ToolCalling
    {
        /// <summary>
        /// [Required]
        /// The ID of the tool call.
        /// </summary>
        [JsonProperty("id"), JsonRequired]
        public string ID { get; private set; } = string.Empty;

        /// <summary>
        /// [Required]
        /// The type of the tool. Currently, only function is supported.
        /// </summary>
        [JsonProperty("type"), JsonRequired]
        public string Type { get; private set; } = string.Empty;

        /// <summary>
        /// [Required]
        /// The function that the model called.
        /// </summary>
        [JsonProperty("function"), JsonRequired]
        public ToolFunction Function { get; private set; } = new();
    }
}