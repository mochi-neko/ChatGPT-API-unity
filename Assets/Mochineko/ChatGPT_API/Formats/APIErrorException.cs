#nullable enable
using System;
using System.Net;

namespace Mochineko.ChatGPT_API.Formats
{
    /// <summary>
    /// See https://platform.openai.com/docs/guides/error-codes/api-errors
    /// </summary>
    public sealed class APIErrorException : Exception
    {
        internal APIErrorException(HttpStatusCode statusCode, APIErrorResponseBody errorResponse)
            : base($"status code:{statusCode}, " +
                   $"message:{errorResponse.Error.Message}, " +
                   $"type:{errorResponse.Error.Type}, " +
                   $"param:{errorResponse.Error.Param}, " +
                   $"code:{errorResponse.Error.Code}")
        {
        }
    }
}