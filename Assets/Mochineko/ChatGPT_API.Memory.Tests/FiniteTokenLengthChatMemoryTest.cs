#nullable enable
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.ChatGPT_API.Memory.Tests
{
    [TestFixture]
    internal sealed class FiniteTokenLengthChatMemoryTest
    {
        [Test]
        [RequiresPlayMode(false)]
        public async Task MemoriesTokenLengthShouldBeUpToCapacity()
        {
            var memory = new FiniteTokenLengthQueueChatMemory(10, Model.Turbo);
            memory.TokenLength.Should().Be(0);
            memory.ShouldBeSameContentsAs(new List<Message>());
            
            await memory.AddMessageAsync(new Message(Role.User, "a"), CancellationToken.None);
            memory.TokenLength.Should().Be(1);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "a"),
            });

            await memory.AddMessageAsync(new Message(Role.User, "b"), CancellationToken.None);
            memory.TokenLength.Should().Be(2);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "a"),
                new Message(Role.User, "b"),
            });

            await memory.AddMessageAsync(new Message(Role.User, "c d e f g h i j"), CancellationToken.None);
            memory.TokenLength.Should().Be(10);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "a"),
                new Message(Role.User, "b"),
                new Message(Role.User, "c d e f g h i j"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "k"), CancellationToken.None);
            memory.TokenLength.Should().Be(10);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "b"),
                new Message(Role.User, "c d e f g h i j"),
                new Message(Role.User, "k"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "l m n o p"), CancellationToken.None);
            memory.TokenLength.Should().Be(6);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "k"),
                new Message(Role.User, "l m n o p"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "q"), CancellationToken.None);
            memory.TokenLength.Should().Be(7);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "k"),
                new Message(Role.User, "l m n o p"),
                new Message(Role.User, "q"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "r"), CancellationToken.None);
            memory.TokenLength.Should().Be(8);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "k"),
                new Message(Role.User, "l m n o p"),
                new Message(Role.User, "q"),
                new Message(Role.User, "r"),
            });
            
            await memory.AddMessageAsync(new Message(Role.User, "1 2 3 4 5 "), CancellationToken.None);
            memory.TokenLength.Should().Be(10);
            memory.ShouldBeSameContentsAs(new[]
            {
                new Message(Role.User, "1 2 3 4 5 "),
            });
            
            memory.ClearAllMessages();
            memory.TokenLength.Should().Be(0);
            memory.ShouldBeSameContentsAs(new List<Message>());
        }
    }
}