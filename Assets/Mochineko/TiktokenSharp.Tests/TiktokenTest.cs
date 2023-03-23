#nullable enable
using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace TiktokenSharp.Tests
{
    [TestFixture]
    internal sealed class TiktokenTest
    {
        [TestCase("gpt-3.5-turbo", "hello world", new[] { 15339, 1917 }, null)]
        [TestCase("gpt-3.5-turbo", "hello <|endoftext|>", new[] { 15339, 220, 100257 }, "all")]
        [TestCase("text-davinci-003", "hello world", new[] { 31373, 995 }, null)]
        [TestCase("text-davinci-003", "hello <|endoftext|>", new[] { 31373, 220, 50256 }, "all")]
        [RequiresPlayMode(false)]
        public void TokenizeTest(string model, string text, int[] expected, object allowedSpecial)
        {
            var tikToken = TikToken.EncodingForModel(model);

            var tokens = tikToken.Encode(text, allowedSpecial);
            var decoded = tikToken.Decode(tokens);

            tokens.Count.Should().Be(expected.Length);
            ShouldBeSameAs(expected, tokens);

            decoded.Should().Be(text);
            tikToken.Decode(new List<int>(expected)).Should().Be(text);
        }
        
        private void ShouldBeSameAs<T>(T[] expected, List<T> actual)
        {
            actual.Count.Should().Be(expected.Length);
            for (var i = 0; i < expected.Length; i++)
            {
                actual[i].Should().Be(expected[i]);
            }
        }
    }
}
