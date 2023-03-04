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

            var connection = new ChatGPTConnection(apiKey, Model.Turbo0301);

            var result = await connection.SendMessage(message, CancellationToken.None);

            string.IsNullOrEmpty(result.ResultMessage).Should().BeFalse();
            
            Debug.Log($"Result:\n{result}");
        }
    }
}
