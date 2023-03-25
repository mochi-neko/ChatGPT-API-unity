#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// Defines a memory of chat.
    /// </summary>
    public interface IChatMemory
    {
        /// <summary>
        /// Messages in the memory.
        /// </summary>
        IReadOnlyList<Message> Messages { get; }
        /// <summary>
        /// Adds a message to the memory.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="TaskCanceledException"></exception>
        Task AddMessageAsync(Message message, CancellationToken cancellationToken);
        /// <summary>
        /// Clears all messages in the memory.
        /// </summary>
        void ClearAllMessages();
    }
}