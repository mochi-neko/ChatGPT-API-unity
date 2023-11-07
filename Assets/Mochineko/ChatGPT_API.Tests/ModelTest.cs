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
        [TestCase(Model.Turbo1106, "gpt-3.5-turbo-1106")]
        [TestCase(Model.Turbo16K, "gpt-3.5-turbo-16k")]
        [TestCase(Model.Four, "gpt-4")]
        [TestCase(Model.Four0613, "gpt-4-0613")]
        [TestCase(Model.Four32K, "gpt-4-32k")]
        [TestCase(Model.Four32K0613, "gpt-4-32k-0613")]
        [TestCase(Model.Four1106Preview, "gpt-4-1106-preview")]
        [TestCase(Model.FourVisionPreview, "gpt-4-vision-preview")]
        [RequiresPlayMode(false)]
        public void Resolve(Model model, string modelText)
        {
            model.ToText().Should().Be(modelText);
            modelText.ToModel().Should().Be(model);
        }
    }
}