#nullable enable
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Mochineko.ChatGPT_API.Formats;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// Binds ChatGPT chat completion API.
    /// </summary>
    public sealed class ChatGPTConnection
    {
        private readonly IReadOnlyDictionary<string, string> headers;
        private readonly List<Message> messages;
        private readonly APIRequestBody requestBody;

        private static readonly HttpClient httpClient;
        private const string EndPoint = "https://api.openai.com/v1/chat/completions";

        static ChatGPTConnection()
        {
            // Pooling socket
            httpClient = new HttpClient();
        }

        /// <summary>
        /// Create an instance of ChatGPT chat completion API connection.
        /// https://platform.openai.com/docs/api-reference/chat/create
        /// </summary>
        /// <param name="apiKey">API key generated by OpenAI</param>
        /// <param name="model">Chat model</param>
        /// <param name="messages">Initial messages</param>
        /// <exception cref="ArgumentNullException">API Key must be set</exception>
        public ChatGPTConnection(string apiKey, Model model = Model.Turbo, List<Message>? messages = null)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            this.headers = CreateHeader(apiKey);
            this.messages = messages ?? new List<Message>();
            this.requestBody = new APIRequestBody(model, this.messages);
        }

        /// <summary>
        /// Create an instance of ChatGPT chat completion API connection.
        /// https://platform.openai.com/docs/api-reference/chat/create
        /// </summary>
        /// <param name="apiKey">API key generated by OpenAI</param>
        /// <param name="requestBody">Request parameters</param>
        /// <exception cref="ArgumentNullException">API Key must be set</exception>
        public ChatGPTConnection(string apiKey, APIRequestBody requestBody)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            this.headers = CreateHeader(apiKey);
            this.requestBody = requestBody;
            this.messages = this.requestBody.Messages;
        }

        /// <summary>
        /// Add system message to local messages stack.
        /// The system message helps set the behavior of the assistant.
        /// </summary>
        /// <param name="content">System message content</param>
        public void AddSystemMessage(string content)
        {
            messages.Add(new Message(Role.System, content));
        }

        /// <summary>
        /// Create a message though ChatGPT chat completion API.
        /// </summary>
        /// <param name="content">Message content to send ChatGPT API</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Response from ChatGPT chat completion API.</returns>
        /// <exception cref="Exception">System exceptions</exception>
        /// <exception cref="APIErrorException">API error response</exception>
        /// <exception cref="HttpRequestException">Network error</exception>
        /// <exception cref="TaskCanceledException">Cancellation or timeout</exception>
        /// <exception cref="JsonSerializationException">JSON error</exception>
        public async Task<APIResponseBody> CreateMessageAsync(string content, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            // Record sending message
            messages.Add(new Message(Role.User, content));

            using var requestMessage = CreateRequestMessage(headers, requestBody);

            // Post request and receive response
            // May throw exceptions e.g. HttpRequestException, TaskCanceledException and so on.
            using var responseMessage = await httpClient.SendAsync(requestMessage, cancellationToken);
            if (responseMessage == null)
            {
                throw new Exception($"[ChatGPT_API] HttpResponseMessage is null.");
            }

            var responseJson = await responseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseJson))
            {
                throw new Exception($"[ChatGPT_API] Response JSON is null or empty.");
            }
            
            if (responseMessage.IsSuccessStatusCode)
            {
                var responseBody = APIResponseBody.FromJson(responseJson);
                if (responseBody == null)
                {
                    throw new Exception($"[ChatGPT_API] Response body is null.");
                }
                
                if (responseBody.Choices.Length == 0)
                {
                    throw new Exception($"[ChatGPT_API] Not found any choices in response body:{responseJson}.");
                }

                // Record result to messages
                messages.Add(responseBody.Choices[0].Message);

                return responseBody;
            }
            else if (IsAPIError(responseMessage.StatusCode))
            {
                var errorResponseBody = APIErrorResponseBody.FromJson(responseJson);
                if (errorResponseBody != null)
                {
                    // Handle API error response
                    throw new APIErrorException(responseMessage.StatusCode, errorResponseBody);
                }

                throw new Exception($"[ChatGPT_API] Error response body is null with status code:{responseMessage.StatusCode}.");
            }
            else // Another error, e.g. 5XX errors.
            {
                // Throws HttpRequestException
                responseMessage.EnsureSuccessStatusCode();

                throw new Exception($"[ChatGPT_API] It should not be be reached with status code:{responseMessage.StatusCode}.");
            }
        }

        public string ExportMessages()
        {
            var records = new MessageRecords(messages);

            return records.ToJson();
        }

        private static IReadOnlyDictionary<string, string> CreateHeader(string apiKey)
            => new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {apiKey}",
            };

        private static HttpRequestMessage CreateRequestMessage(
            IReadOnlyDictionary<string, string> headers,
            APIRequestBody requestBody)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, EndPoint);
            foreach (var header in headers)
            {
                requestMessage.Headers.Add(header.Key, header.Value);
            }

            var requestContent = new StringContent(
                content: requestBody.ToJson(),
                encoding: System.Text.Encoding.UTF8,
                mediaType: "application/json");

            requestMessage.Content = requestContent;

            return requestMessage;
        }

        private static bool IsAPIError(HttpStatusCode statusCode)
            => 400 <= (int)statusCode && (int)statusCode <= 499;
    }
}