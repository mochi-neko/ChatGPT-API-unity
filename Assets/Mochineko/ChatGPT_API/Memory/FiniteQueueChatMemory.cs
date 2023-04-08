#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mochineko.ChatGPT_API.Memory
{
    /// <summary>
    /// A chat memory that stores messages in a finite queue.
    /// </summary>
    public sealed class FiniteQueueChatMemory : IChatMemory
    {
        private readonly Queue<Message> queue;
        private readonly int maxMemoriesCount;
        private readonly object lockObject = new();

        public IReadOnlyList<Message> Messages
        {
            get
            {
                lock (lockObject)
                {
                    return queue.ToArray();
                }
            }
        }

        public FiniteQueueChatMemory(int maxMemoriesCount)
        {
            this.maxMemoriesCount = maxMemoriesCount;
            this.queue = new Queue<Message>(maxMemoriesCount);
        }

        public Task AddMessageAsync(Message message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            lock (lockObject)
            {
                queue.Enqueue(message);

                while (queue.Count > maxMemoriesCount)
                {
                    queue.Dequeue();
                }
            }

            return Task.CompletedTask;
        }

        public void ClearAllMessages()
        {
            lock (lockObject)
            {
                queue.Clear();
            }
        }
    }
}