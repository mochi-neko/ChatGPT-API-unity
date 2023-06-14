using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

#nullable enable

namespace Mochineko.ChatGPT_API.Tests
{
    [TestFixture]
    internal sealed class ChatCompletionAPIConnectionTest
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

            var connection = new ChatCompletionAPIConnection(apiKey);

            var result = await connection.CompleteChatAsync(message, CancellationToken.None);

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

            var connection = new ChatCompletionAPIConnection(apiKey);

            var result = await connection.CompleteChatAsync(
                message,
                CancellationToken.None,
                model: Model.Turbo,
                temperature: 1f,
                topP: 1f,
                n: 1,
                stream: false,
                stop: null,
                maxTokens: null,
                presencePenalty: 0f,
                frequencyPenalty: 0f,
                user: "test");

            string.IsNullOrEmpty(result.ResultMessage).Should().BeFalse();

            Debug.Log($"Result:\n{result}");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task TooLongMaxTokens()
        {
            // This file is a target of .gitignore.
            var apiKeyPath = Path.Combine(
                Application.dataPath,
                "Mochineko/ChatGPT_API.Tests/OpenAI_API_Key.txt");

            var apiKey = await File.ReadAllTextAsync(apiKeyPath);

            var connection = new ChatCompletionAPIConnection(apiKey);

            Func<Task> send = async () => await connection.CompleteChatAsync(
                "a",
                CancellationToken.None,
                model: Model.Turbo,
                temperature: 1f,
                topP: 1f,
                n: 1,
                stream: false,
                stop: null,
                maxTokens: int.MaxValue, // Over 4096 tokens
                presencePenalty: 0f,
                frequencyPenalty: 0f,
                user: "test");

            await send.Should().ThrowAsync<APIErrorException>(because: "max_tokens is too long.");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task InvalidAPIKey()
        {
            var apiKey = "invalid";

            var connection = new ChatCompletionAPIConnection(apiKey);

            Func<Task> send = async () => await connection.CompleteChatAsync("a", CancellationToken.None);

            await send.Should().ThrowAsync<APIErrorException>(because: "Invalid API key.");
        }

        [Test]
        [RequiresPlayMode(false)]
        public async Task FunctionCalling()
        {
            // This file is a target of .gitignore.
            var apiKeyPath = Path.Combine(
                Application.dataPath,
                "Mochineko/ChatGPT_API.Tests/OpenAI_API_Key.txt");

            var apiKey = await File.ReadAllTextAsync(apiKeyPath);

            var connection = new ChatCompletionAPIConnection(apiKey);

            var function = new Function
            (
                name: "emotion_simulator",
                description: "Simulate emotion of the assistant as human.",
                parameters: new Dictionary<string, object>
                {
                    { "type", "object" },
                    {
                        "properties", new Dictionary<string, object>
                        {
                            {
                                "emotion", new Dictionary<string, object>
                                {
                                    { "type", "string" },
                                    {
                                        "enum", new List<string>
                                        {
                                            "neutral", "happy", "sad", "angry", "surprised", "disgusted", "fearful"
                                        }
                                    },
                                }
                            }
                        }
                    },
                    { "required", new List<string> { "emotion" } },
                }
            );

            var result = await connection
                .CompleteChatAsync(
                    "Please tell me about yourself.",
                    CancellationToken.None,
                    model: Model.Turbo0613,
                    functions: new List<Function> { function },
                    functionCallSpecifying: new FunctionCallSpecifying(name: "emotion_simulator")
                );

            Debug.Log($"Result:\nname:{result.Choices[0].Message.FunctionCall?.Name},\narguments:{result.Choices[0].Message.FunctionCall?.Arguments}");
        }
    }
}