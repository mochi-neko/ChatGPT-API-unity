#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mochineko.ChatGPT_API.Memory
{
    /// <summary>
    /// A chat memory that stores messages in a finite queue with fixed prompts.
    /// </summary>
    public sealed class FiniteQueueWithFixedPromptsChatMemory : IChatMemory
    {
        private readonly int maxMemoriesCountWithoutPrompts;
        private readonly Queue<Message> queue;
        private readonly List<Message> prompts = new();
        private readonly object lockObject = new();

        public FiniteQueueWithFixedPromptsChatMemory(int maxMemoriesCountWithoutPrompts)
        {
            this.maxMemoriesCountWithoutPrompts = maxMemoriesCountWithoutPrompts;
            this.queue = new Queue<Message>(maxMemoriesCountWithoutPrompts);
        }

        public IReadOnlyList<Message> Messages
        {
            get
            {
                lock (lockObject)
                {
                    return prompts
                        .Concat(queue)
                        .ToList();
                }
            }
        }

        public Task AddMessageAsync(Message message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            lock (lockObject)
            {
                if (message.Role is Role.System)
                {
                    prompts.Add(message);
                }
                else
                {
                    queue.Enqueue(message);

                    while (queue.Count > maxMemoriesCountWithoutPrompts)
                    {
                        queue.Dequeue();
                    }
                }
            }

            return Task.CompletedTask;
        }

        public void ClearAllMessages()
        {
            lock (lockObject)
            {
                prompts.Clear();
                queue.Clear();
            }
        }
        
        public void ClearMessagesWithoutPrompts()
        {
            lock (lockObject)
            {
                queue.Clear();
            }
        }
    }
}