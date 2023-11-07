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
        /// "gpt-3.5-turbo" latest GPT-3.5 turbo model.
        /// </summary>
        Turbo,
        /// <summary>
        /// "gpt-3.5-turbo-0301" fixed GPT-3.5 turbo model at 03/01/2023.
        /// </summary>
        [Obsolete("This model version will be deprecated on 06/13/2024.")]
        Turbo0301,
        /// <summary>
        /// "gpt-3.5-turbo-0613" fixed GPT-3.5 turbo model at 06/13/2023.
        /// </summary>
        [Obsolete("This model version will be deprecated on 06/13/2024.")]
        Turbo0613,
        /// <summary>
        /// "gpt-3.5-turbo-1106" updated GPT-3.5 turbo model for 4k tokens at 11/06/2023.
        /// </summary>
        Turbo1106,
        /// <summary>
        /// "gpt-3.5-turbo-16k" latest GPT-3.5 turbo model for 16k tokens.
        /// </summary>
        Turbo16K,
        /// <summary>
        /// "gpt-3.5-turbo-16k-0613" fixed GPT-3.5 turbo model for 16k tokens at 06/13/2023.
        /// </summary>
        [Obsolete("This model version will be deprecated on 06/13/2024.")]
        Turbo16K0613,
        /// <summary>
        /// "gpt-4" latest GPT-4 model.
        /// </summary>
        Four,
        /// <summary>
        /// "gpt-4-0314", fixed GPT-4 model at 03/14/2023.
        /// </summary>
        [Obsolete("This model version will be deprecated on 06/13/2024.")]
        Four0314,
        /// <summary>
        /// "gpt-4-0613", fixed GPT-4 model at 06/13/2023.
        /// </summary>
        Four0613,
        /// <summary>
        /// "gpt-4-32k", latest GPT-4 model for 32k tokens.
        /// </summary>
        Four32K,
        /// <summary>
        /// "gpt-4-32k-0314", fixed GPT-4 model for 32k tokens at 03/14/2023.
        /// </summary>
        [Obsolete("This model version will be deprecated on 06/13/2024.")]
        Four32K0314,
        /// <summary>
        /// "gpt-4-32k-0613", fixed GPT-4 model for 32k tokens at 06/13/2023.
        /// </summary>
        Four32K0613,
        /// <summary>
        /// "gpt-4-1106-preview" latest GPT-4 turbo model for 4k tokens at 11/06/2023.
        /// </summary>
        Four1106Preview,
        /// <summary>
        /// "gpt-4-vision-preview" latest GPT-4 turbo with vision model.
        /// </summary>
        FourVisionPreview,
    }
}