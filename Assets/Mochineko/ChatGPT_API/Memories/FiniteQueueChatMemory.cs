#nullable enable
using System.Collections.Generic;

namespace Mochineko.ChatGPT_API.Memories
{
    public sealed class FiniteQueueChatMemory : IChatMemory
    {
        private Queue<Message> memories;
        public IReadOnlyList<Message> Memories => memories.ToArray();

        public FiniteQueueChatMemory(int capacity)
        {
            memories = new Queue<Message>(capacity);
        }
        
        public void AddMessage(Message message)
        {
            memories.Enqueue(message);
        }

        public void ClearAllMessages()
        {
            memories.Clear();
        }
    }
}