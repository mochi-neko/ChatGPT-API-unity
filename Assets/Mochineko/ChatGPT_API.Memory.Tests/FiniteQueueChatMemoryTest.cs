#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.ChatGPT_API.Memory.Tests
{
    [TestFixture]
    internal sealed class FiniteQueueChatMemoryTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task MemoriesShouldBeUpToCapacity()
        {
            var memory = new FiniteQueueChatMemory(3);
            memory.ShouldBeSameContentsAs(new List<Message>());
            
            await memory.AddMessageAsync(new Message(Role.User, "a"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "a"),
            });
            
            await memory.AddMessageAsync(new Message(Role.Assistant, "b"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "a"),
                new Message(Role.Assistant, "b"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "c"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "a"),
                new Message(Role.Assistant, "b"),
                new Message(Role.User, "c"),
            });
            
            // Up to capacity
            await memory.AddMessageAsync(new Message(Role.Assistant, "d"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.Assistant, "b"),
                new Message(Role.User, "c"),
                new Message(Role.Assistant, "d"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "e"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "c"),
                new Message(Role.Assistant, "d"),
                new Message(Role.User, "e"),
            });

            memory.ClearAllMessages();
            memory.ShouldBeSameContentsAs(new List<Message>());
        }
    }
}
