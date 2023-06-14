#nullable enable
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    public sealed class Function
    {
        /// <summary>
        /// [Required]
        /// The name of the function to be called.
        /// Must be a-z, A-Z, 0-9, or contain underscores and dashes, with a maximum length of 64.
        /// </summary>
        [JsonProperty("name"), JsonRequired]
        public string Name { get; private set; }
        
        /// <summary>
        /// [Optional]
        /// The description of what the function does.
        /// </summary>
        [JsonProperty("description")]
        public string? Description { get; private set; }
        
        /// <summary>
        /// [Optional]
        /// The parameters the functions accepts, described as a JSON Schema object.
        /// See the guide for examples, and the JSON Schema reference for documentation about the format.
        /// </summary>
        [JsonProperty("parameters")]
        public IReadOnlyDictionary<string, object>? Parameters { get; private set; }

        public Function(
            string name,
            string? description = null,
            IReadOnlyDictionary<string, object>? parameters = null)
        {
            this.Name = name;
            this.Description = description;
            this.Parameters = parameters;
        }
    }
}