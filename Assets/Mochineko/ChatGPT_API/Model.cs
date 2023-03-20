#nullable enable
namespace Mochineko.ChatGPT_API
{
    /// <summary>
    /// Chat model.
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
        Turbo0301,
        /// <summary>
        /// "gpt-4" latest GPT4 model.
        /// </summary>
        Four,
    }
}