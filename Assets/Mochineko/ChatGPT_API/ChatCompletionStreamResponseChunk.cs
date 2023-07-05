#nullable enable
using System;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// Response body of ChatGPT chat completion API.
    /// See https://platform.openai.com/docs/guides/chat/introduction.
    /// </summary>
    [JsonObject]
    public sealed class ChatCompletionStreamResponseChunk
    {
        [JsonProperty("id"), JsonRequired]
        public string ID { get; private set; } = string.Empty;

        [JsonProperty("object"), JsonRequired]
        public string Object { get; private set; } = string.Empty;

        [JsonProperty("created"), JsonRequired]
        public int Created { get; private set; }

        [JsonProperty("model"), JsonRequired]
        public string Model { get; private set; } = string.Empty;

        [JsonProperty("choices"), JsonRequired]
        public ChoiceChunk[] Choices { get; private set; } = Array.Empty<ChoiceChunk>();

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static ChatCompletionStreamResponseChunk? FromJson(string json)
            => JsonConvert.DeserializeObject<ChatCompletionStreamResponseChunk>(json, new JsonSerializerSettings());
    }
}