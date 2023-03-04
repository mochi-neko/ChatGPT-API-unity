#nullable enable
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace Mochineko.ChatGPT_API
{
    public sealed class ChatGPTConnection
    {
        private readonly IReadOnlyDictionary<string, string> headers;
        private readonly List<Message> messages;
        private readonly APIRequestBody requestBody;

        private static readonly HttpClient httpClient;
        private const string EndPoint = "https://api.openai.com/v1/chat/completions";

        static ChatGPTConnection()
        {
            httpClient = new HttpClient();
        }

        public ChatGPTConnection(string apiKey, Model model = Model.Turbo)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            this.headers = CreateHeader(apiKey);
            this.messages = new List<Message>();
            this.requestBody = new APIRequestBody(model, messages);
        }

        public ChatGPTConnection(string apiKey, string messageRecordsJson, Model model = Model.Turbo)
        {
            if (string.IsNullOrEmpty(apiKey))
            {
                throw new ArgumentNullException(nameof(apiKey));
            }

            if (string.IsNullOrEmpty(messageRecordsJson))
            {
                throw new ArgumentNullException(nameof(messageRecordsJson));
            }

            this.headers = CreateHeader(apiKey);

            var messagesRecordsJson = MessageRecords.FromJson(messageRecordsJson);
            if (messagesRecordsJson == null)
            {
                throw new Exception($"[ChatGPT_API] MessageRecords is null.");
            }

            this.messages = messagesRecordsJson.Messages;
            this.requestBody = new APIRequestBody(model, this.messages);
        }

        public void AddSystemMessage(string content)
        {
            messages.Add(new Message(Role.System, content));
        }

        public async UniTask<string> SendMessage(string content, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            messages.Add(new Message(Role.User, content));

            using var requestMessage = CreateRequestMessage(headers, requestBody);

            // TODO: Implement retrying and circuit breaking?
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

            APIResponseBody? responseBody;
            try
            {
                responseBody = APIResponseBody.FromJson(responseJson);
                if (responseBody == null)
                {
                    throw new Exception($"[ChatGPT_API] Response body is null.");
                }
            }
            catch (Exception e)
            {
                var errorResponseBody = APIErrorResponseBody.FromJson(responseJson);
                if (errorResponseBody != null)
                {
                    // Handle API error response
                    throw new ChatGPTAPIException(errorResponseBody);
                }

                throw new Exception($"[ChatGPT_API] Error response body is null.", e);
            }

            if (responseBody.Choices.Length == 0)
            {
                throw new Exception($"[ChatGPT_API] Not found any choices in response body.");
            }

            var choice = responseBody.Choices[0];

            // Record message
            messages.Add(choice.Message);

            return choice.Message.Content;
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
    }
}