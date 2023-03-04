#nullable enable
using System;
using Mochineko.ChatGPT_API.Formats;

namespace Mochineko.ChatGPT_API
{
    public sealed class ChatGPTAPIException : Exception
    {
        internal ChatGPTAPIException(APIErrorResponseBody errorResponse)
            : base(errorResponse.Error.Message)
        {
        }
    }
}