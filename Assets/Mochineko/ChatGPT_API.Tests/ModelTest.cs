#nullable enable
using FluentAssertions;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Mochineko.ChatGPT_API.Tests
{
    [TestFixture]
    internal sealed class ModelTest
    {
        [TestCase(Model.Turbo, "gpt-3.5-turbo")]
        [TestCase(Model.Turbo0301, "gpt-3.5-turbo-0301")]
        [TestCase(Model.Four, "gpt-4")]
        [TestCase(Model.Four0314, "gpt-4-0314")]
        [TestCase(Model.Four32K, "gpt-4-32k")]
        [TestCase(Model.Four32K0314, "gpt-4-32k-0314")]
        [RequiresPlayMode(false)]
        public void Resolve(Model model, string modelText)
        {
            model.ToText().Should().Be(modelText);
            modelText.ToModel().Should().Be(model);
        }
    }
}