#nullable enable
using System.Collections.Generic;

namespace Mochineko.ChatGPT_API
{
    public static class ModelResolver
    {
        private static readonly IReadOnlyDictionary<Model, string> Dictionary = new Dictionary<Model, string>
        {
            [Model.Turbo] = "gpt-3.5-turbo",
            [Model.Turbo0301] = "gpt-3.5-turbo-0301",
            [Model.Turbo0613] = "gpt-3.5-turbo-0613",
            [Model.Turbo1106] = "gpt-3.5-turbo-1106",
            [Model.Turbo16K] = "gpt-3.5-turbo-16k",
            [Model.Turbo16K0613] = "gpt-3.5-turbo-16k-0613",
            [Model.Four] = "gpt-4",
            [Model.Four0314] = "gpt-4-0314",
            [Model.Four0613] = "gpt-4-0613",
            [Model.Four32K] = "gpt-4-32k",
            [Model.Four32K0314] = "gpt-4-32k-0314",
            [Model.Four32K0613] = "gpt-4-32k-0613",
            [Model.Four1106Preview] = "gpt-4-1106-preview",
            [Model.FourVisionPreview] = "gpt-4-vision-preview",
        };

        public static Model ToModel(this string model)
            => Dictionary.Inverse(model);

        public static string ToText(this Model model)
            => Dictionary[model];
    }
}