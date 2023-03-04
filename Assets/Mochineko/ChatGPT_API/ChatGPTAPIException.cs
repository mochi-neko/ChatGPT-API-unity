#nullable enable
using System;

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