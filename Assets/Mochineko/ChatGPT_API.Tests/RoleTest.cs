#nullable enable
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.ChatGPT_API.Tests
{
    [TestFixture]
    internal sealed class RoleTest
    {
        [TestCase(Role.System, "system")]
        [TestCase(Role.Assistant, "assistant")]
        [TestCase(Role.User, "user")]
        [RequiresPlayMode(false)]
        public void Resolve(Role role, string roleText)
        {
            role.ToText().Should().Be(roleText);
            roleText.ToRole().Should().Be(role);
        }
    }
}