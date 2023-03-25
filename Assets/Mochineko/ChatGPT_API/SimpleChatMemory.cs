#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mochineko.ChatGPT_API
{
    public sealed class SimpleChatMemory : IChatMemory
    {
        private readonly List<Message> messages = new();
        public IReadOnlyList<Message> Messages
        {
            get
            {
                lock (lockObject)
                {
                    return messages;
                }
            }
        }

        private readonly object lockObject = new();
        
        public Task AddMessageAsync(Message message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            lock (lockObject)
            {
                messages.Add(message);
            }

            return Task.CompletedTask;
        }

        public void ClearAllMessages()
        {
            lock (lockObject)
            {
                messages.Clear();
            }
        }
    }
}