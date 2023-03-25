#nullable enable
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// Binds ChatGPT chat completion API.
    /// </summary>
    public sealed class ChatCompletionAPIConnection
    {
        private readonly string apiKey;
        private readonly IChatMemory chatMemory;
        private readonly HttpClient httpClient;

        private const string ChatCompletionEndPoint = "https://api.openai.com/v1/chat/completions";

        /// <summary>
        /// Create an instance of ChatGPT chat completion API connection.
        /// https://platform.openai.com/docs/api-reference/chat/create
        /// </summary>
        /// <param name="apiKey">API key generated by OpenAI</param>
        /// <param name="chatMemory"></param>
        /// <param name="prompt"></param>
        /// <param name="httpClient"></param>
        /// <exception cref="ArgumentNullException">API Key must be set</exception>
        public ChatCompletionAPIConnection(
            string apiKey,
            IChatMemory? chatMemory = null,
            string? prompt = null,
            HttpClient? httpClient = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            this.apiKey = apiKey;
            this.chatMemory = chatMemory ?? new SimpleChatMemory();

            if (!string.IsNullOrEmpty(prompt))
            {
                this.chatMemory.AddMessage(new Message(Role.System, prompt));
            }
            
            this.httpClient = httpClient ?? HttpClientPool.PooledClient;
        }

        /// <summary>
        /// Completes chat though ChatGPT chat completion API.
        /// </summary>
        /// <param name="content">Message content to send ChatGPT API</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <param name="topP"></param>
        /// <param name="n"></param>
        /// <param name="stream"></param>
        /// <param name="stop"></param>
        /// <param name="maxTokens"></param>
        /// <param name="presencePenalty"></param>
        /// <param name="frequencyPenalty"></param>
        /// <param name="logitBias"></param>
        /// <param name="user"></param>
        /// <returns>Response from ChatGPT chat completion API.</returns>
        /// <exception cref="Exception">System exceptions</exception>
        /// <exception cref="APIErrorException">API error response</exception>
        /// <exception cref="HttpRequestException">Network error</exception>
        /// <exception cref="TaskCanceledException">Cancellation or timeout</exception>
        /// <exception cref="JsonSerializationException">JSON error</exception>
        public async Task<ChatCompletionResponseBody> CompleteChatAsync(
             string content,
             CancellationToken cancellationToken,
             Model model = Model.Turbo,
             float? temperature = null,
             float? topP = null,
             uint? n = null,
             bool? stream = null,
             string[]? stop = null,
             int? maxTokens = null,
             float? presencePenalty = null,
             float? frequencyPenalty = null,
             Dictionary<int, int>? logitBias = null,
             string? user = null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Record user message
            chatMemory.AddMessage(new Message(Role.User, content));

            // Create request body
            var requestBody = new ChatCompletionRequestBody(
                model.ToText(),
                chatMemory.Memories,
                temperature,
                topP,
                n,
                stream,
                stop,
                maxTokens,
                presencePenalty,
                frequencyPenalty,
                logitBias,
                user);

            // Build request message
            using var requestMessage = new HttpRequestMessage(
                HttpMethod.Post, ChatCompletionEndPoint);
            requestMessage.Headers
                .Add("Authorization", $"Bearer {apiKey}");

            var requestContent = new StringContent(
                content: requestBody.ToJson(),
                encoding: System.Text.Encoding.UTF8,
                mediaType: "application/json");

            requestMessage.Content = requestContent;
            
            // Post request and receive response
            // NOTE: Can throw exceptions
            using var responseMessage = await httpClient
                .SendAsync(requestMessage, cancellationToken);
            
            if (responseMessage == null)
            {
                throw new Exception($"[ChatGPT_API] HttpResponseMessage is null.");
            }

            if (responseMessage.Content == null)
            {
                throw new Exception($"[ChatGPT_API] HttpResponseMessage.Content is null.");
            }

            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseJson))
            {
                throw new Exception($"[ChatGPT_API] Response JSON is null or empty.");
            }

            // Succeeded
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBody = ChatCompletionResponseBody.FromJson(responseJson);
                if (responseBody == null)
                {
                    throw new Exception($"[ChatGPT_API] Response body is null.");
                }

                if (responseBody.Choices.Length == 0)
                {
                    throw new Exception($"[ChatGPT_API] Not found any choices in response body:{responseJson}.");
                }

                // Record assistant messages
                foreach (var choice in responseBody.Choices)
                {
                    chatMemory.AddMessage(choice.Message);
                }
                
                return responseBody;
            }
            // Failed
            else
            {
                try
                {
                    responseMessage.EnsureSuccessStatusCode();
                }
                catch (Exception e)
                {
                    throw new APIErrorException(responseJson, responseMessage.StatusCode, e);
                }
                
                throw new Exception(
                    $"[ChatGPT_API] System error with status code:{responseMessage.StatusCode}, message:{responseJson}");
            }
        }
        
        /// <summary>
        /// Completes chat as <see cref="Stream"/> though ChatGPT chat completion API.
        /// </summary>
        /// <param name="content">Message content to send ChatGPT API</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="model"></param>
        /// <param name="temperature"></param>
        /// <param name="topP"></param>
        /// <param name="n"></param>
        /// <param name="stream"></param>
        /// <param name="stop"></param>
        /// <param name="maxTokens"></param>
        /// <param name="presencePenalty"></param>
        /// <param name="frequencyPenalty"></param>
        /// <param name="logitBias"></param>
        /// <param name="user"></param>
        /// <returns>Response stream from ChatGPT chat completion API.</returns>
        /// <exception cref="Exception">System exceptions</exception>
        /// <exception cref="APIErrorException">API error response</exception>
        /// <exception cref="HttpRequestException">Network error</exception>
        /// <exception cref="TaskCanceledException">Cancellation or timeout</exception>
        /// <exception cref="JsonSerializationException">JSON error</exception>
        public async Task<Stream> CompleteChatAsStreamAsync(
             string content,
             CancellationToken cancellationToken,
             Model model = Model.Turbo,
             float? temperature = null,
             float? topP = null,
             uint? n = null,
             bool? stream = null,
             string[]? stop = null,
             int? maxTokens = null,
             float? presencePenalty = null,
             float? frequencyPenalty = null,
             Dictionary<int, int>? logitBias = null,
             string? user = null)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Record user message
            chatMemory.AddMessage(new Message(Role.User, content));

            // Create request body
            var requestBody = new ChatCompletionRequestBody(
                model.ToText(),
                chatMemory.Memories,
                temperature,
                topP,
                n,
                stream,
                stop,
                maxTokens,
                presencePenalty,
                frequencyPenalty,
                logitBias,
                user);

            // Build request message
            using var requestMessage = new HttpRequestMessage(
                HttpMethod.Post, ChatCompletionEndPoint);
            requestMessage.Headers
                .Add("Authorization", $"Bearer {apiKey}");

            var requestContent = new StringContent(
                content: requestBody.ToJson(),
                encoding: System.Text.Encoding.UTF8,
                mediaType: "application/json");

            requestMessage.Content = requestContent;
            
            // Post request and receive response
            // NOTE: Can throw exceptions
            using var responseMessage = await httpClient
                .SendAsync(requestMessage, cancellationToken);
            
            if (responseMessage == null)
            {
                throw new Exception($"[ChatGPT_API] HttpResponseMessage is null.");
            }
            
            if (responseMessage.Content == null)
            {
                throw new Exception($"[ChatGPT_API] HttpResponseMessage.Content is null.");
            }
            
            // Succeeded
            if (responseMessage.IsSuccessStatusCode)
            {
                return await responseMessage.Content.ReadAsStreamAsync();
            }
            // Failed
            else
            {
                var responseJson = await responseMessage.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseJson))
                {
                    throw new Exception($"[ChatGPT_API] Response JSON is null or empty.");
                }
                
                try
                {
                    responseMessage.EnsureSuccessStatusCode();
                }
                catch (Exception e)
                {
                    throw new APIErrorException(responseJson, responseMessage.StatusCode, e);
                }
                
                throw new Exception(
                    $"[ChatGPT_API] System error with status code:{responseMessage.StatusCode}, message:{responseJson}");
            }
        }
    }
}