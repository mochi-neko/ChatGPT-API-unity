#nullable enable
using System;

namespace Mochineko.ChatGPT_API
{
    public enum Role : byte
    {
        /// <summary>
        /// "system" that instructs behavior of completion.
        /// </summary>
        System,
        /// <summary>
        /// "assistant" that is generated message by completion.
        /// </summary>
        Assistant,
        /// <summary>
        /// "user" that is input message by user.
        /// </summary>
        User,
        /// <summary>
        /// "function" that is generated message by function calling.
        /// </summary>
        [Obsolete("Deprecated and replaced by tool")]
        Function,
        /// <summary>
        /// "tool" that is generated message by tools.
        /// </summary>
        Tool,
    }
}