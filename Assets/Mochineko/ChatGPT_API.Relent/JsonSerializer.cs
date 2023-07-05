#nullable enable
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
                    return Results.Succeed(json);
                }
                else
                {
                    return Results.FailWithTrace<string>(
                        $"Failed to serialize because serialized JSON of {nameof(ChatCompletionRequestBody)} was null or empty.");
                }
            }
            catch (JsonException exception)
            {
                return Results.FailWithTrace<string>(
                    $"Failed to serialize {nameof(ChatCompletionRequestBody)} to JSON because -> {exception}");
            }
        }

        public static IResult<ChatCompletionRequestBody> DeserializeRequestBody(string json)
        {
            try
            {
                var requestBody = JsonConvert.DeserializeObject<ChatCompletionRequestBody>(json);

                if (requestBody != null)
                {
                    return Results.Succeed(requestBody);
                }
                else
                {
                    return Results.FailWithTrace<ChatCompletionRequestBody>(
                        $"Failed to deserialize because deserialized object of {nameof(ChatCompletionRequestBody)} was null.");
                }
            }
            catch (JsonException exception)
            {
                return Results.FailWithTrace<ChatCompletionRequestBody>(
                    $"Failed to deserialize {nameof(ChatCompletionRequestBody)} from JSON because -> {exception}");
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
                    return Results.Succeed(json);
                }
                else
                {
                    return Results.FailWithTrace<string>(
                        $"Failed to serialize because serialized JSON of {nameof(ChatCompletionResponseBody)} was null or empty.");
                }
            }
            catch (JsonException exception)
            {
                return Results.FailWithTrace<string>(
                    $"Failed to serialize {nameof(ChatCompletionResponseBody)} to JSON because -> {exception}");
            }
        }

        public static IResult<ChatCompletionResponseBody> DeserializeResponseBody(string json)
        {
            try
            {
                var requestBody = JsonConvert.DeserializeObject<ChatCompletionResponseBody>(json);

                if (requestBody != null)
                {
                    return Results.Succeed(requestBody);
                }
                else
                {
                    return Results.FailWithTrace<ChatCompletionResponseBody>(
                        $"Failed to deserialize because deserialized object of {nameof(ChatCompletionResponseBody)} was null.");
                }
            }
            catch (JsonSerializationException exception)
            {
                return Results.FailWithTrace<ChatCompletionResponseBody>(
                    $"Failed to deserialize {nameof(ChatCompletionResponseBody)} from JSON because -> {exception}");
            }
        }
        
        public static IResult<ChatCompletionStreamResponseChunk> DeserializeRequestChunk(string json)
        {
            try
            {
                var requestBody = JsonConvert.DeserializeObject<ChatCompletionStreamResponseChunk>(json);

                if (requestBody != null)
                {
                    return Results.Succeed(requestBody);
                }
                else
                {
                    return Results.FailWithTrace<ChatCompletionStreamResponseChunk>(
                        $"Failed to deserialize because deserialized object of {nameof(ChatCompletionStreamResponseChunk)} was null.");
                }
            }
            catch (JsonException exception)
            {
                return Results.FailWithTrace<ChatCompletionStreamResponseChunk>(
                    $"Failed to deserialize {nameof(ChatCompletionStreamResponseChunk)} from JSON because -> {exception}");
            }
        }
    }
}