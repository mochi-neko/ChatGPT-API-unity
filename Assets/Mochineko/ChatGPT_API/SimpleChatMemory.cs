#nullable enable
using System.Collections.Generic;

namespace Mochineko.ChatGPT_API
{
    public sealed class SimpleChatMemory : IChatMemory
    {
        private List<Message> memories = new();
        public IReadOnlyList<Message> Memories => memories;

        public void AddMessage(Message message)
        {
            memories.Add(message);
        }

        public void ClearAllMessages()
        {
            memories.Clear();
        }
    }
}