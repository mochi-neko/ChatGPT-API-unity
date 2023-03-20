using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.Relent.Resilience;
using Mochineko.Relent.Resilience.Bulkhead;
using Mochineko.Relent.Resilience.CircuitBreaker;
using Mochineko.Relent.Resilience.Retry;
using Mochineko.Relent.Resilience.Timeout;
using Mochineko.Relent.Resilience.Wrap;
using Mochineko.Relent.UncertainResult;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Debug = UnityEngine.Debug;

namespace Mochineko.ChatGPT_API.Relent.Tests
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

            var connection = new RelentChatCompletionAPIConnection(apiKey);

            var policy = BuildPolicy();

            var stopWatch = new Stopwatch();
            stopWatch.Start();

            var result = await policy.ExecuteAsync(
                async cancellationToken
                    => await connection.CompleteChatAsync(message, cancellationToken),
                CancellationToken.None);
            
            stopWatch.Stop();
            Debug.Log($"Elapsed time:{stopWatch.ElapsedMilliseconds / 1000}ms.");

            if (result is IUncertainSuccessResult<ChatCompletionResponseBody> success)
            {
                Debug.Log($"Result:\n{success.Result.ToJson()}");
            }
            else if (result is IUncertainRetryableResult<ChatCompletionResponseBody> retryable)
            {
                Debug.LogError(retryable.Message);
            }
            else if (result is IUncertainFailureResult<ChatCompletionResponseBody> failure)
            {
                Debug.LogError(failure.Message);
            }
            else
            {
                throw new UncertainResultPatternMatchException(nameof(result));
            }
        }

        private IPolicy<ChatCompletionResponseBody> BuildPolicy()
        {
            var totalTimeoutPolicy = TimeoutFactory.Timeout<ChatCompletionResponseBody>(
                timeout: TimeSpan.FromSeconds(30d));
            
            var retryPolicy = RetryFactory.RetryWithJitter<ChatCompletionResponseBody>(
                maxRetryCount: 5,
                minimum: 1d,
                maximum: 5d);

            var eachTimeoutPolicy = TimeoutFactory.Timeout<ChatCompletionResponseBody>(
                timeout: TimeSpan.FromSeconds(10d));
            
            var circuitBreakerPolicy = CircuitBreakerFactory.CircuitBreaker<ChatCompletionResponseBody>(
                failureThreshold: 3,
                interval: TimeSpan.FromSeconds(1d));
            
            var bulkheadPolicy = BulkheadFactory.Bulkhead<ChatCompletionResponseBody>(
                maxParallelization: 1);

            return totalTimeoutPolicy
                .Wrap(retryPolicy)
                .Wrap(eachTimeoutPolicy)
                .Wrap(circuitBreakerPolicy)
                .Wrap(bulkheadPolicy);
        }
    }
}