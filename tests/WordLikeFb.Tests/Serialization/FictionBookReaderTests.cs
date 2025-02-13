using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;
using WordLikeFb.Serialization;

namespace WordLikeFb.Tests.Serialization
{
    public class FictionBookReaderTests
    {
        [Theory]
        [InlineData("<p>123</p>", false, false)]
        [InlineData("<p><strong>123</strong></p>", false, true)]
        [InlineData("<p><emphasis>123</emphasis></p>", true, false)]
        [InlineData("<p><strong><emphasis>123</emphasis></strong></p>", true, true)]
        [InlineData("<p><emphasis><strong>123</strong></emphasis></p>", true, true)]
        public void ReadParagraph_plain(string input, bool isItalic, bool isStrong)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input);

            var res = sut.ReadParagraph(fixture);

            Run run = res.Inlines.First() as Run;

            Assert.NotNull(run);
            Assert.Equal("123", run.Text);
            Assert.Equal(isItalic, run.FontStyle == FontStyles.Italic);
            Assert.Equal(isStrong, run.FontWeight == FontWeights.Bold);
        }

        [Theory]
        [InlineData("<p>123123123</p>", 1)]
        [InlineData("<p>123123<strong>123</strong></p>", 2)]
        [InlineData("<p>123123<emphasis>123</emphasis></p>", 2)]
        [InlineData("<p>123<strong>123</strong>123</p>", 3)]
        [InlineData("<p>123<strong>123</strong><strong>123</strong></p>", 3)]
        [InlineData("<p>123<strong>123</strong><emphasis>123</emphasis></p>", 3)]
        [InlineData("<p>123<emphasis>123</emphasis>123</p>", 3)]
        [InlineData("<p>123<emphasis>123</emphasis><strong>123</strong></p>", 3)]
        [InlineData("<p>123<emphasis>123</emphasis><emphasis>123</emphasis></p>", 3)]
        [InlineData("<p><strong>123</strong>123123</p>", 2)]
        [InlineData("<p><strong>123</strong>123<strong>123</strong></p>", 3)]
        [InlineData("<p><strong>123</strong>123<emphasis>123</emphasis></p>", 3)]
        [InlineData("<p><strong>123</strong><strong>123</strong>123</p>", 3)]
        [InlineData("<p><strong>123</strong><strong>123</strong><strong>123</strong></p>", 3)]
        [InlineData("<p><strong>123</strong><strong>123</strong><emphasis>123</emphasis></p>", 3)]
        [InlineData("<p><strong>123</strong><emphasis>123</emphasis>123</p>", 3)]
        [InlineData("<p><strong>123</strong><emphasis>123</emphasis><strong>123</strong></p>", 3)]
        [InlineData("<p><strong>123</strong><emphasis>123</emphasis><emphasis>123</emphasis></p>", 3)]
        [InlineData("<p><emphasis>123</emphasis>123123</p>", 2)]
        [InlineData("<p><emphasis>123</emphasis>123<strong>123</strong></p>", 3)]
        [InlineData("<p><emphasis>123</emphasis>123<emphasis>123</emphasis></p>", 3)]
        [InlineData("<p><emphasis>123</emphasis><strong>123</strong>123</p>", 3)]
        [InlineData("<p><emphasis>123</emphasis><strong>123</strong><strong>123</strong></p>", 3)]
        [InlineData("<p><emphasis>123</emphasis><strong>123</strong><emphasis>123</emphasis></p>", 3)]
        [InlineData("<p><emphasis>123</emphasis><emphasis>123</emphasis>123</p>", 3)]
        [InlineData("<p><emphasis>123</emphasis><emphasis>123</emphasis><strong>123</strong></p>", 3)]
        [InlineData("<p><emphasis>123</emphasis><emphasis>123</emphasis><emphasis>123</emphasis></p>", 3)]
        public void ReadParagraph_composite(string input, int expectedCount)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input, LoadOptions.None);

            var res = sut.ReadParagraph(fixture);

            Assert.Equal(expectedCount, res.Inlines.Count);
        }
    }
}
