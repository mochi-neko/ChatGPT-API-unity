#nullable enable
using System;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API.Formats
{
    /// <summary>
    /// Response body of ChatGPT chat completion API.
    /// See https://platform.openai.com/docs/guides/chat/introduction.
    /// </summary>
    [JsonObject]
    public sealed class APIResponseBody
    {
        [JsonProperty("id")] public string ID { get; private set; } = string.Empty;
        [JsonProperty("object")] public string Object { get; private set; } = string.Empty;
        [JsonProperty("created")] public uint Created { get; private set; }
        [JsonProperty("model")] public string Model { get; private set; } = string.Empty;
        [JsonProperty("usage")] public Usage Usage { get; private set; } = new();
        [JsonProperty("choices")] public Choice[] Choices { get; private set; } = Array.Empty<Choice>();

        /// <summary>
        /// Result of chat completion.
        /// </summary>
        public string ResultMessage
            => Choices.Length != 0 ? Choices[0].Message.Content : string.Empty;
        
        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static APIResponseBody? FromJson(string json)
            => JsonConvert.DeserializeObject<APIResponseBody>(json);
    }
}