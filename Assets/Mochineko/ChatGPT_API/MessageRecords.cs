#nullable enable
using System.Collections.Generic;
using Mochineko.ChatGPT_API.Formats;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    internal sealed class MessageRecords
    {
        [JsonProperty("messages")] public List<Message> Messages { get; }

        public MessageRecords(List<Message> messages)
        {
            this.Messages = messages;
        }

        public string ToJson()
            => JsonConvert.SerializeObject(this, Formatting.Indented);

        public static MessageRecords? FromJson(string json)
            => JsonConvert.DeserializeObject<MessageRecords>(json);
    }
}