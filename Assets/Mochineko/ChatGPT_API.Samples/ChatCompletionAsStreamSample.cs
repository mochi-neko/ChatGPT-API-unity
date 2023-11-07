#nullable enable
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mochineko.ChatGPT_API.Memory;
using UnityEngine;

namespace Mochineko.ChatGPT_API.Samples
{
    /// <summary>
    /// A sample component to complete chat as stream by ChatGPT API on Unity.
    /// </summary>
    public sealed class ChatCompletionAsStreamSample : MonoBehaviour
    {
        [SerializeField, TextArea(3, 10)]
        private string prompt = string.Empty;

        /// <summary>
        /// Message sent to ChatGPT API.
        /// </summary>
        [SerializeField, TextArea(3, 10)] private string message = string.Empty;

        /// <summary>
        /// Max number of chat memory of queue.
        /// </summary>
        [SerializeField] private int maxMemoryCount = 10;

        private ChatCompletionAPIConnection? connection;
        private IChatMemory? memory;

        private void Start()
        {
            // Get API key from environment variable.
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                Debug.LogError("OpenAI API key must be set.");
                return;
            }

            memory = new FiniteQueueChatMemory(maxMemoryCount);

            // Create instance of ChatGPTConnection with specifying chat model.
            connection = new ChatCompletionAPIConnection(
                apiKey,
                memory,
                prompt);
        }

        [ContextMenu(nameof(SendChatAsStream))]
        public void SendChatAsStream()
        {
            SendChatAsStreamAsync(this.GetCancellationTokenOnDestroy())
                .Forget();
        }

        [ContextMenu(nameof(ClearChatMemory))]
        public void ClearChatMemory()
        {
            memory?.ClearAllMessages();
        }

        private async UniTask SendChatAsStreamAsync(CancellationToken cancellationToken)
        {
            // Validations
            if (connection == null)
            {
                Debug.LogError($"[ChatGPT_API.Samples] Connection is null.");
                return;
            }

            if (memory == null)
            {
                Debug.LogError($"[ChatGPT_API.Samples] Memory is null.");
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                Debug.LogError($"[ChatGPT_API.Samples] Chat content is empty.");
                return;
            }

            var builder = new StringBuilder();
            try
            {
                await UniTask.SwitchToThreadPool();

                // Receive enumerable from ChatGPT chat completion API.
                var enumerable = await connection.CompleteChatAsStreamAsync(
                    message,
                    cancellationToken,
                    verbose: true);

                await foreach (var chunk in enumerable.WithCancellation(cancellationToken))
                {
                    // First chunk has only "role" element.
                    if (chunk.Choices[0].Delta.Content is null)
                    {
                        Debug.Log($"[ChatGPT_API.Samples] Role:{chunk.Choices[0].Delta.Role}.");
                        continue;
                    }

                    var delta = chunk.Choices[0].Delta.Content;
                    builder.Append(delta);
                    Debug.Log($"[ChatGPT_API.Samples] Delta:{delta}, Current:{builder}");
                }

                var result = builder.ToString();

                // Log chat completion result.
                Debug.Log($"[ChatGPT_API.Samples] Completed: \n{result}");

                // Record result to memory.
                await memory.AddMessageAsync(Message.CreateAssistantMessage(
                    content: result),
                    cancellationToken);
            }
            catch (Exception e)
            {
                // Exceptions should be caught.
                Debug.LogException(e);
                return;
            }
        }
    }
}