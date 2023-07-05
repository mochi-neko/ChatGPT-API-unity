#nullable enable
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Mochineko.ChatGPT_API.Memory;
using Mochineko.Relent.Resilience;
using Mochineko.Relent.Resilience.Bulkhead;
using Mochineko.Relent.Resilience.Retry;
using Mochineko.Relent.Resilience.Timeout;
using Mochineko.Relent.Resilience.Wrap;
using Mochineko.Relent.UncertainResult;
using UnityEngine;

namespace Mochineko.ChatGPT_API.Relent.Samples
{
    /// <summary>
    /// A sample component to complete chat as stream by ChatGPT API on Unity.
    /// </summary>
    public sealed class RelentChatCompletionStreamSample : MonoBehaviour
    {
        /// <summary>
        /// System message to instruct assistant.
        /// </summary>
        [SerializeField, TextArea] private string prompt = string.Empty;

        /// <summary>
        /// Message sent to ChatGPT API.
        /// </summary>
        [SerializeField, TextArea] private string message = string.Empty;

        /// <summary>
        /// Max number of chat memory of queue.
        /// </summary>
        [SerializeField] private int maxMemoryCount = 20;
        
        /// <summary>
        /// Total timeout seconds of API calling.
        /// </summary>
        [SerializeField] private float totalTimeoutSeconds = 30f;
        
        /// <summary>
        /// Each retry timeout seconds of API calling.
        /// </summary>
        [SerializeField] private float eachTimeoutSeconds = 10f;

        /// <summary>
        /// Max retry count of API calling.
        /// </summary>
        [SerializeField] private int maxRetryCount = 5;

        /// <summary>
        /// Retry interval seconds of API calling.
        /// </summary>
        [SerializeField] private float retryIntervalSeconds = 1f;
        
        /// <summary>
        /// Max parallelization of API calling.
        /// </summary>
        [SerializeField] private int maxParallelization = 1;

        private RelentChatCompletionAPIConnection? connection;
        private FiniteQueueChatMemory? memory;
        private IPolicy<IAsyncEnumerable<ChatCompletionStreamResponseChunk>>? policy;

        private void Start()
        {
            // API Key must be set.
            var apiKey = Environment.GetEnvironmentVariable("OPENAI_API_KEY");
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new NullReferenceException(nameof(apiKey));
            }
            memory = new FiniteQueueChatMemory(maxMemoryCount);

            // Create instance of ChatGPTConnection with specifying chat model.
            connection = new RelentChatCompletionAPIConnection(
                apiKey,
                memory,
                prompt);

            policy = BuildPolicy();
        }
        
        private IPolicy<IAsyncEnumerable<ChatCompletionStreamResponseChunk>> BuildPolicy()
        {
            var totalTimeoutPolicy = TimeoutFactory.Timeout<IAsyncEnumerable<ChatCompletionStreamResponseChunk>>(
                timeout: TimeSpan.FromSeconds(totalTimeoutSeconds));
            
            var retryPolicy = RetryFactory.RetryWithInterval<IAsyncEnumerable<ChatCompletionStreamResponseChunk>>(
                maxRetryCount,
                interval: TimeSpan.FromSeconds(retryIntervalSeconds));

            var eachTimeoutPolicy = TimeoutFactory.Timeout<IAsyncEnumerable<ChatCompletionStreamResponseChunk>>(
                timeout: TimeSpan.FromSeconds(eachTimeoutSeconds));

            var bulkheadPolicy = BulkheadFactory.Bulkhead<IAsyncEnumerable<ChatCompletionStreamResponseChunk>>(
                maxParallelization);

            return totalTimeoutPolicy
                .Wrap(retryPolicy)
                .Wrap(eachTimeoutPolicy)
                .Wrap(bulkheadPolicy);
        }

        [ContextMenu(nameof(SendChat))]
        public void SendChat()
        {
            SendChatAsync(this.GetCancellationTokenOnDestroy()).Forget();
        }
        
        [ContextMenu(nameof(ClearChatMemory))]
        public void ClearChatMemory()
        {
            memory?.ClearAllMessages();
        }
        
        private async UniTask SendChatAsync(CancellationToken cancellationToken)
        {
            // Validations
            if (connection == null)
            {
                Debug.LogError($"[ChatGPT_API.Relent.Samples] Connection is null.");
                return;
            }

            if (policy == null)
            {
                Debug.LogError($"[ChatGPT_API.Relent.Samples] Policy is null.");
                return;
            }

            if (string.IsNullOrEmpty(message))
            {
                Debug.LogError($"[ChatGPT_API.Relent.Samples] Chat content is empty.");
                return;
            }

            await UniTask.SwitchToThreadPool();

            var result = await policy.ExecuteAsync(
                async innerCancellationToken
                    => await connection.CompleteChatAsStreamAsync(message, innerCancellationToken),
                cancellationToken);

            await UniTask.SwitchToMainThread(cancellationToken);

            var builder = new StringBuilder();
            
            switch (result)
            {
                case IUncertainSuccessResult<IAsyncEnumerable<ChatCompletionStreamResponseChunk>> success:
                    await foreach (var chunk in success.Result.WithCancellation(cancellationToken))
                    {
                        var delta = chunk.Choices[0].Delta.Content;
                        if (delta == null)
                        {
                            // First chunk has only "role" element.
                            Debug.Log($"[ChatGPT_API.Relent.Samples] Role: {chunk.Choices[0].Delta.Role}.");
                            continue;
                        }
                            
                        builder.Append(delta);
                        Debug.Log($"[ChatGPT_API.Relent.Samples] Delta: {delta}, Current: {builder}");
                    }
                    Debug.Log($"[ChatGPT_API.Relent.Samples] Completed chat -> {builder}");
                    break;
                
                case IUncertainRetryableResult<IAsyncEnumerable<ChatCompletionStreamResponseChunk>> retryable:
                    Debug.LogError($"[ChatGPT_API.Relent.Samples] Failed to complete chat -> {retryable.Message}");
                    break;
                
                case IUncertainFailureResult<IAsyncEnumerable<ChatCompletionStreamResponseChunk>> failure:
                    Debug.LogError($"[ChatGPT_API.Relent.Samples] Failed to complete chat -> {failure.Message}");
                    break;
                
                default:
                    throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
    }
}