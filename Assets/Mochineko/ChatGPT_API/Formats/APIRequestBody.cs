#nullable enable
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API.Formats
{
    [JsonObject]
    internal sealed class APIRequestBody
    {
        [JsonProperty("model")] public string Model { get; }
        [JsonProperty("messages")] public List<Message> Messages { get; }

        public APIRequestBody(Model model, List<Message> messages)
        {
            this.Model = model.ToText();
            this.Messages = messages;
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static APIRequestBody? FromJson(string json)
            => JsonConvert.DeserializeObject<APIRequestBody>(json);
    }
}