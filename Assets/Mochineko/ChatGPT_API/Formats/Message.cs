#nullable enable
using System;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API.Formats
{
    [JsonObject]
    public sealed class Message
    {
        [JsonProperty("role")] public string Role { get; private set; }
        [JsonProperty("content")] public string Content { get; private set; }

        internal Message()
        {
            this.Role = Formats.Role.Assistant.ToText();
            this.Content = string.Empty;
        }

        internal Message(Role role, string content)
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