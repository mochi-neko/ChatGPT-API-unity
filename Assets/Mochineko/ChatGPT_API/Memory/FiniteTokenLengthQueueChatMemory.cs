#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TiktokenSharp;

namespace Mochineko.ChatGPT_API.Memory
{
    /// <summary>
    /// A chat memory that stores messages in a queue that has finite lenght of tokens.
    /// </summary>
    public sealed class FiniteTokenLengthQueueChatMemory : IChatMemory
    {
        private readonly int maxTokenLength;
        private readonly TikToken tikToken;
        private readonly Queue<Message> queue = new();
        private readonly object lockObject = new();

        public FiniteTokenLengthQueueChatMemory(
            int maxTokenLength,
            Model model)
        {
            this.maxTokenLength = maxTokenLength;
            this.tikToken = TikToken.EncodingForModel(model.ToText());
        }
        
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

        public int TokenLength
        {
            get
            {
                lock (lockObject)
                {
                    return queue.TokenLength(tikToken);
                }
            }
        }

        public Task AddMessageAsync(Message message, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            lock (lockObject)
            {
                queue.Enqueue(message);

                while (TokenLength > maxTokenLength)
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