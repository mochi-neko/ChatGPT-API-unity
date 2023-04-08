#nullable enable
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TiktokenSharp;

namespace Mochineko.ChatGPT_API.Memory
{
    /// <summary>
    /// A chat memory that stores messages in a queue that has finite lenght of tokens with fixed prompts.
    /// </summary>
    public sealed class FiniteTokenLengthQueueWithFixedPromptsChatMemory : IChatMemory
    {
        private readonly int maxTokenLengthWithoutPrompts;
        private readonly TikToken tikToken;
        private readonly Queue<Message> queue = new();
        private readonly List<Message> prompts = new();
        private readonly object lockObject = new();

        public FiniteTokenLengthQueueWithFixedPromptsChatMemory(
            int maxTokenLengthWithoutPrompts,
            Model model)
        {
            this.maxTokenLengthWithoutPrompts = maxTokenLengthWithoutPrompts;
            this.tikToken = TikToken.EncodingForModel(model.ToText());
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

        public int TokenLengthWithoutPrompts
        {
            get
            {
                lock (lockObject)
                {
                    return queue.TokenLength(tikToken);
                }
            }
        }

        private int PromptsTokenLength
        {
            get
            {
                lock (lockObject)
                {
                    return prompts.TokenLength(tikToken);
                }
            }
        }

        public int MemoriesTokenLength
        {
            get
            {
                lock (lockObject)
                {
                    return PromptsTokenLength
                           + TokenLengthWithoutPrompts;
                }
            }
        }

        public Task AddMessageAsync(Message message, CancellationToken cancellationToken)
        {
            lock (lockObject)
            {
                if (message.Role is Role.System)
                {
                    prompts.Add(message);
                }
                else
                {
                    queue.Enqueue(message);

                    while (TokenLengthWithoutPrompts > maxTokenLengthWithoutPrompts)
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