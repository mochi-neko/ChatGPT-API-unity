#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.ChatGPT_API.Memory.Tests
{
    [TestFixture]
    internal sealed class FiniteQueueWithFixedPromptsChatMemoryTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task MemoriesShouldBeUpToCapacityWithFixedPrompts()
        {
            var memory = new FiniteQueueWithFixedPromptsChatMemory(3);
            memory.ShouldBeSameContentsAs(new List<Message>());
            
            await memory.AddMessageAsync(new Message(Role.System, "prompt"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "a"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.User, "a"),
            });
            
            await memory.AddMessageAsync(new Message(Role.Assistant, "b"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.User, "a"),
                new Message(Role.Assistant, "b"),
            });
            
            // System messages are not counted as capacity
            await memory.AddMessageAsync(new Message(Role.User, "c"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.User, "a"),
                new Message(Role.Assistant, "b"),
                new Message(Role.User, "c"),
            });
            
            await memory.AddMessageAsync(new Message(Role.Assistant, "d"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.Assistant, "b"),
                new Message(Role.User, "c"),
                new Message(Role.Assistant, "d"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "e"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.User, "c"),
                new Message(Role.Assistant, "d"),
                new Message(Role.User, "e"),
            });
            
            memory.ClearMessagesWithoutPrompts();
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "f"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.User, "f"),
            });
            
            await memory.AddMessageAsync(new Message(Role.System, "second prompt"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.System, "second prompt"),
                new Message(Role.User, "f"),
            });
            
            await memory.AddMessageAsync(new Message(Role.Assistant, "g"), CancellationToken.None);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.System, "prompt"),
                new Message(Role.System, "second prompt"),
                new Message(Role.User, "f"),
                new Message(Role.Assistant, "g"),
            });

            memory.ClearAllMessages();
            memory.ShouldBeSameContentsAs(new List<Message>());
        }
        
        [Test]
        [RequiresPlayMode(false)]
        public async Task ShouldBeSameBehaviourWhenHasNoPrompts()
        {
            var memory = new FiniteQueueWithFixedPromptsChatMemory(3);
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