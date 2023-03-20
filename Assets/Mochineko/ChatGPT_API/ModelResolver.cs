#nullable enable
using System.Collections.Generic;

namespace Mochineko.ChatGPT_API
{
    internal static class ModelResolver
    {
        private static readonly IReadOnlyDictionary<Model, string> Dictionary = new Dictionary<Model, string>
        {
            [Model.Turbo] = "gpt-3.5-turbo",
            [Model.Turbo0301] = "gpt-3.5-turbo-0301",
            [Model.Four] = "gpt-4",
        };

        public static Model ToModel(this string model)
            => Dictionary.Inverse(model);

        public static string ToText(this Model model)
            => Dictionary[model];
    }
}