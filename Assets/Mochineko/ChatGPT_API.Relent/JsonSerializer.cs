#nullable enable
using System;
using Mochineko.Relent.Result;
using Newtonsoft.Json;

namespace Mochineko.ChatGPT_API.Relent
{
    public static class JsonSerializer
    {
        public static IResult<string> SerializeRequestBody(this ChatCompletionRequestBody requestBody)
        {
            try
            {
                var json = JsonConvert.SerializeObject(
                    requestBody,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                if (!string.IsNullOrEmpty(json))
                {
                    return ResultFactory.Succeed(json);
                }
                else
                {
                    return ResultFactory.Fail<string>(
                        $"Failed to serialize because serialized JSON of {nameof(ChatCompletionRequestBody)} was null or empty.");
                }
            }
            catch (JsonSerializationException exception)
            {
                return ResultFactory.Fail<string>(
                    $"Failed to serialize {nameof(ChatCompletionRequestBody)} to JSON because -> {exception}");
            }
            catch (Exception exception)
            {
                return ResultFactory.Fail<string>(
                    $"Failed to serialize {nameof(ChatCompletionRequestBody)} to JSON because unhandled exception -> {exception}");
            }
        }

        public static IResult<ChatCompletionRequestBody> DeserializeRequestBody(string json)
        {
            try
            {
                var requestBody = JsonConvert.DeserializeObject<ChatCompletionRequestBody>(json);

                if (requestBody != null)
                {
                    return ResultFactory.Succeed(requestBody);
                }
                else
                {
                    return ResultFactory.Fail<ChatCompletionRequestBody>(
                        $"Failed to deserialize because deserialized object of {nameof(ChatCompletionRequestBody)} was null.");
                }
            }
            catch (JsonSerializationException exception)
            {
                return ResultFactory.Fail<ChatCompletionRequestBody>(
                    $"Failed to deserialize {nameof(ChatCompletionRequestBody)} from JSON because -> {exception}");
            }
            catch (JsonReaderException exception)
            {
                return ResultFactory.Fail<ChatCompletionRequestBody>(
                    $"Failed to deserialize {nameof(ChatCompletionRequestBody)} from JSON because -> {exception}");
            }
            catch (Exception exception)
            {
                return ResultFactory.Fail<ChatCompletionRequestBody>(
                    $"Failed to deserialize {nameof(ChatCompletionRequestBody)} from JSON because unhandled exception -> {exception}");
            }
        }
        
        public static IResult<string> SerializeResponseBody(this ChatCompletionResponseBody requestBody)
        {
            try
            {
                var json = JsonConvert.SerializeObject(
                    requestBody,
                    Formatting.Indented,
                    new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });

                if (!string.IsNullOrEmpty(json))
                {
                    return ResultFactory.Succeed(json);
                }
                else
                {
                    return ResultFactory.Fail<string>(
                        $"Failed to serialize because serialized JSON of {nameof(ChatCompletionResponseBody)} was null or empty.");
                }
            }
            catch (JsonSerializationException exception)
            {
                return ResultFactory.Fail<string>(
                    $"Failed to serialize {nameof(ChatCompletionResponseBody)} to JSON because -> {exception}");
            }
            catch (Exception exception)
            {
                return ResultFactory.Fail<string>(
                    $"Failed to serialize {nameof(ChatCompletionResponseBody)} to JSON because unhandled exception -> {exception}");
            }
        }

        public static IResult<ChatCompletionResponseBody> DeserializeResponseBody(string json)
        {
            try
            {
                var requestBody = JsonConvert.DeserializeObject<ChatCompletionResponseBody>(json);

                if (requestBody != null)
                {
                    return ResultFactory.Succeed(requestBody);
                }
                else
                {
                    return ResultFactory.Fail<ChatCompletionResponseBody>(
                        $"Failed to deserialize because deserialized object of {nameof(ChatCompletionResponseBody)} was null.");
                }
            }
            catch (JsonSerializationException exception)
            {
                return ResultFactory.Fail<ChatCompletionResponseBody>(
                    $"Failed to deserialize {nameof(ChatCompletionResponseBody)} from JSON because -> {exception}");
            }
            catch (JsonReaderException exception)
            {
                return ResultFactory.Fail<ChatCompletionResponseBody>(
                    $"Failed to deserialize {nameof(ChatCompletionResponseBody)} from JSON because -> {exception}");
            }
            catch (Exception exception)
            {
                return ResultFactory.Fail<ChatCompletionResponseBody>(
                    $"Failed to deserialize {nameof(ChatCompletionResponseBody)} from JSON because unhandled exception -> {exception}");
            }
        }
    }
}