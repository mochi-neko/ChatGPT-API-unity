#nullable enable
using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Mochineko.ChatGPT_API.Memory;
using Mochineko.Relent.Resilience;
using Mochineko.Relent.Resilience.Bulkhead;
using Mochineko.Relent.Resilience.Retry;
using Mochineko.Relent.Resilience.Timeout;
using Mochineko.Relent.Resilience.Wrap;
using Mochineko.Relent.UncertainResult;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

namespace Mochineko.ChatGPT_API.Relent.Samples
{
    /// <summary>
    /// A sample component to complete chat by ChatGPT API on Unity.
    /// </summary>
    public sealed class RelentChatCompletionSample : MonoBehaviour
    {
        /// <summary>
        /// API key generated by OpenAPI.
        /// </summary>
        [SerializeField] private string apiKey = string.Empty;

        /// <summary>
        /// System message to instruct assistant.
        /// </summary>
        [SerializeField, TextArea] private string systemMessage = string.Empty;

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
        private IPolicy<ChatCompletionResponseBody>? policy;

        private void Start()
        {
            // API Key must be set.
            if (string.IsNullOrEmpty(apiKey))
            {
                Debug.LogError("OpenAI API key must be set.");
                return;
            }

            memory = new FiniteQueueChatMemory(maxMemoryCount);

            // Create instance of ChatGPTConnection with specifying chat model.
            connection = new RelentChatCompletionAPIConnection(
                apiKey,
                memory,
                systemMessage);

            policy = BuildPolicy();
        }
        
        private IPolicy<ChatCompletionResponseBody> BuildPolicy()
        {
            var totalTimeoutPolicy = TimeoutFactory.Timeout<ChatCompletionResponseBody>(
                timeout: TimeSpan.FromSeconds(totalTimeoutSeconds));
            
            var retryPolicy = RetryFactory.RetryWithInterval<ChatCompletionResponseBody>(
                maxRetryCount,
                interval: TimeSpan.FromSeconds(retryIntervalSeconds));

            var eachTimeoutPolicy = TimeoutFactory.Timeout<ChatCompletionResponseBody>(
                timeout: TimeSpan.FromSeconds(eachTimeoutSeconds));

            var bulkheadPolicy = BulkheadFactory.Bulkhead<ChatCompletionResponseBody>(
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
                    => await connection.CompleteChatAsync(message, innerCancellationToken),
                cancellationToken);

            await UniTask.SwitchToMainThread(cancellationToken);
            
            if (result is IUncertainSuccessResult<ChatCompletionResponseBody> success)
            {
                Debug.Log($"[ChatGPT_API.Relent.Samples] Result:\n{success.Result.ResultMessage}");
            }
            else if (result is IUncertainRetryableResult<ChatCompletionResponseBody> retryable)
            {
                Debug.LogError($"[ChatGPT_API.Relent.Samples] Failed to complete chat -> {retryable.Message}");
            }
            else if (result is IUncertainFailureResult<ChatCompletionResponseBody> failure)
            {
                Debug.LogError($"[ChatGPT_API.Relent.Samples] Failed to complete chat -> {failure.Message}");
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }
    }
}