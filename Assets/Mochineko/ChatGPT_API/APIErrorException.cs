#nullable enable
using System;
using System.Net;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// See https://platform.openai.com/docs/guides/error-codes/api-errors
    /// </summary>
    public sealed class APIErrorException : Exception
    {
        internal APIErrorException(string message, HttpStatusCode statusCode, Exception innerException)
            : base(
                message: $"[ChatGPT_API] ({(int)statusCode}:{statusCode}) {message}",
                innerException: innerException)
        {
        }
    }
}