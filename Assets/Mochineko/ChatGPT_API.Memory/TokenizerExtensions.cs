#nullable enable
using System.Collections.Generic;
using TiktokenSharp;

namespace Mochineko.ChatGPT_API.Memory
{
    /// <summary>
    /// Extensions for <see cref="TikToken"/>.
    /// </summary>
    public static class TokenizerExtensions
    {
        /// <summary>
        /// Calculates the length of tokens of the messages.
        /// </summary>
        /// <param name="messages"></param>
        /// <param name="tikToken"></param>
        /// <returns></returns>
        public static int TokenLength(
            this IEnumerable<Message> messages,
            TikToken tikToken)
        {
            var length = 0;
            foreach (var message in messages)
            {
                length += tikToken
                    .Encode(message.Content)
                    .Count;
            }

            return length;
        }

        /// <summary>
        /// Calculates the length of tokens of the message.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="tikToken"></param>
        /// <returns></returns>
        public static int TokenLength(this Message message, TikToken tikToken)
            => tikToken
                .Encode(message.Content)
                .Count;
    }
}