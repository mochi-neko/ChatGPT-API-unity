#nullable enable
namespace Mochineko.ChatGPT_API.Formats
{
    internal enum Role : byte
    {
        /// <summary>
        /// "system"
        /// </summary>
        System,
        /// <summary>
        /// "assistant"
        /// </summary>
        Assistant,
        /// <summary>
        /// "user"
        /// </summary>
        User,
    }
}