#nullable enable
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// A message between user, assistant and system.
    /// </summary>
    [JsonObject]
    public sealed class Message
    {
        /// <summary>
        /// "role" string of message owner.
        /// </summary>
        [JsonProperty("role")]
        public string RoleString
        {
            get => this.Role.ToText();
            set => this.Role = value.ToRole();
        }
        /// <summary>
        /// Role of message owner.
        /// </summary>
        [JsonIgnore]
        public Role Role { get; private set; }
        
        /// <summary>
        /// Message content.
        /// </summary>
        [JsonProperty("content")]
        public string Content { get; private set; }

        internal Message()
        {
            this.Role = Role.Assistant;
            this.Content = string.Empty;
        }
        
        public Message(Role role, string content)
        {
            this.Role = role;
            this.Content = content;
        }
    }
}