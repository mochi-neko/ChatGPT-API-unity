#nullable enable
using System;

namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// Chat model.
    /// See also https://platform.openai.com/docs/models/model-endpoint-compatibility.
    /// </summary>
    public enum Model : byte
    {
        /// <summary>
        /// "gpt-3.5-turbo" latest GPT3.5 turbo model.
        /// </summary>
        Turbo,
        /// <summary>
        /// "gpt-3.5-turbo-0301" fixed GPT3.5 turbo model at 03/01/2023.
        /// </summary>
        [Obsolete("Use Turbo0613 instead of Turbo0301 until 09/13/2023.")]
        Turbo0301,
        /// <summary>
        /// "gpt-3.5-turbo-0613" fixed GPT3.5 turbo model at 06/13/2023.
        /// </summary>
        Turbo0613,
        /// <summary>
        /// "gpt-3.5-turbo-16k" latest GPT3.5 turbo model for 16k tokens.
        /// </summary>
        Turbo16K,
        /// <summary>
        /// "gpt-3.5-turbo-16k-0613" fixed GPT3.5 turbo model for 16k tokens at 06/13/2023.
        /// </summary>
        Turbo16K0613,
        /// <summary>
        /// "gpt-4" latest GPT4 model.
        /// </summary>
        Four,
        /// <summary>
        /// "gpt-4-0314", fixed GPT4 model at 03/14/2023.
        /// </summary>
        [Obsolete("Use Four0613 instead of Four0314 until 09/13/2023.")]
        Four0314,
        /// <summary>
        /// "gpt-4-0613", fixed GPT4 model at 06/13/2023.
        /// </summary>
        Four0613,
        /// <summary>
        /// "gpt-4-32k", latest GPT4 model for 32k tokens.
        /// </summary>
        Four32K,
        /// <summary>
        /// "gpt-4-32k-0314", fixed GPT4 model for 32k tokens at 03/14/2023.
        /// </summary>
        [Obsolete("Use Four32K0613 instead of Four32K0314 until 09/13/2023.")]
        Four32K0314,
        /// <summary>
        /// "gpt-4-32k-0613", fixed GPT4 model for 32k tokens at 06/13/2023.
        /// </summary>
        Four32K0613,
    }
}