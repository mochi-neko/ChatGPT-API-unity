#nullable enable
using System.Collections.Generic;

namespace Mochineko.ChatGPT_API
{
    public interface IChatMemory
    {
        IReadOnlyList<Message> Memories { get; }
        void AddMessage(Message message);
        void ClearAllMessages();
    }
}