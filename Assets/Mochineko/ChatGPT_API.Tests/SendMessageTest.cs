using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Mochineko.ChatGPT_API.Formats;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#nullable enable

namespace Mochineko.ChatGPT_API.Tests
{
    [TestFixture]
    internal sealed class ChatGptConnectionTest
    {
        [TestCase("あなたのことを教えて")]
        [RequiresPlayMode(false)]
        public async Task SendMessage(string message)
        {
            // This file is a target of .gitignore.
            var apiKeyPath = Path.Combine(
                Application.dataPath,
                "Mochineko/ChatGPT_API.Tests/OpenAI_API_Key.txt");

            var apiKey = await File.ReadAllTextAsync(apiKeyPath);

            var connection = new ChatGPTConnection(apiKey, Model.Turbo);

            var result = await connection.CreateMessageAsync(message, CancellationToken.None);

            string.IsNullOrEmpty(result.ResultMessage).Should().BeFalse();

            Debug.Log($"Result:\n{result}");
        }

        [TestCase("あなたのことを教えて")]
        [RequiresPlayMode(false)]
        public async Task SendMessageWithDetailedParameters(string message)
        {
            // This file is a target of .gitignore.
            var apiKeyPath = Path.Combine(
                Application.dataPath,
                "Mochineko/ChatGPT_API.Tests/OpenAI_API_Key.txt");

            var apiKey = await File.ReadAllTextAsync(apiKeyPath);

            var requestBody = new APIRequestBody(
                model: Model.Turbo.ToText(),
                messages: new List<Message>(),
                temperature: 1f,
                topP: 1f,
                n: 1,
                stream: false,
                stop: null,
                maxTokens: null,
                presencePenalty: 0f,
                frequencyPenalty: 0f,
                user: "test"
            );

            var connection = new ChatGPTConnection(apiKey, requestBody);

            var result = await connection.CreateMessageAsync(message, CancellationToken.None);

            string.IsNullOrEmpty(result.ResultMessage).Should().BeFalse();

            Debug.Log($"Result:\n{result}");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task Error()
        {
            // This file is a target of .gitignore.
            var apiKeyPath = Path.Combine(
                Application.dataPath,
                "Mochineko/ChatGPT_API.Tests/OpenAI_API_Key.txt");

            var apiKey = await File.ReadAllTextAsync(apiKeyPath);

            var requestBody = new APIRequestBody(
                model: Model.Turbo.ToText(),
                messages: new List<Message>(),
                temperature: 1f,
                topP: 1f,
                n: 1,
                stream: false,
                stop: null,
                maxTokens: int.MaxValue, // Over 4096 tokens
                presencePenalty: 0f,
                frequencyPenalty: 0f,
                user: "test"
            );

            var connection = new ChatGPTConnection(apiKey, requestBody);

            Func<Task> send = async () => await connection.CreateMessageAsync("a", CancellationToken.None);
            
            await send.Should().ThrowAsync<ChatGPTAPIException>(because: "max_tokens is too long.");
        }
    }
}