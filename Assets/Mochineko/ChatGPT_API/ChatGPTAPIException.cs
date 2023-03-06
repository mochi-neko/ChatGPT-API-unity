#nullable enable
using System;
using Mochineko.ChatGPT_API.Formats;

namespace Mochineko.ChatGPT_API
{
    public sealed class ChatGPTAPIException : Exception
    {
        internal ChatGPTAPIException(APIErrorResponseBody errorResponse)
            : base($"message:{errorResponse.Error.Message}, " +
                   $"type:{errorResponse.Error.Type}, " +
                   $"param:{errorResponse.Error.Param}, " +
                   $"code:{errorResponse.Error.Code}")
        {
        }
    }
}