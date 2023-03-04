#nullable enable
using System;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    [JsonObject]
    internal sealed class Message
    {
        [JsonProperty("role")] public string Role { get; private set; }
        [JsonProperty("content")] public string Content { get; private set; }

        internal Message()
        {
            this.Role = ChatGPT_API.Role.Assistant.ToText();
            this.Content = string.Empty;
        }

        public Message(Role role, string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException(content);
            }

            this.Role = role.ToText();
            this.Content = content;
        }
    }
}