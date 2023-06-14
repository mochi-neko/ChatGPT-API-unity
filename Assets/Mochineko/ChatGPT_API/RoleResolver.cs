#nullable enable
using System.Collections.Generic;

namespace Mochineko.ChatGPT_API
{
    public static class RoleResolver
    {
        private static readonly IReadOnlyDictionary<Role, string> Dictionary = new Dictionary<Role, string>
        {
            [Role.System] = "system",
            [Role.Assistant] = "assistant",
            [Role.User] = "user",
            [Role.Function] = "function",
        };

        public static Role ToRole(this string role)
            => Dictionary.Inverse(role);

        public static string ToText(this Role role)
            => Dictionary[role];
    }
}