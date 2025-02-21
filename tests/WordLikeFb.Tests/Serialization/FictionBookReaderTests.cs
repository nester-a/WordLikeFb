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

            Run? run = res.Inlines.First() as Run;

            Assert.NotNull(run);
            Assert.Equal("123", run?.Text);
            Assert.Equal(isItalic, run?.FontStyle == FontStyles.Italic);
            Assert.Equal(isStrong, run?.FontWeight == FontWeights.Bold);
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
        public void ReadParagraph_complex(string input, int expectedCount)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input, LoadOptions.None);

            var res = sut.ReadParagraph(fixture);

            Assert.Equal(expectedCount, res.Inlines.Count);
        }

        [Theory]
        [InlineData("<section></section>", 0)]
        [InlineData("<section/>", 0)]
        [InlineData("<section><p></p></section>", 1)]
        [InlineData("<section><p/></section>", 1)]
        [InlineData("<section><section></section></section>", 1)]
        [InlineData("<section><section/></section>", 1)]
        public void ReadSection_plain(string input, int expectedCount)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input);

            var res = sut.ReadSection(fixture);

            Assert.Equal(expectedCount, res.Blocks.Count);
        }

        [Theory]
        [InlineData("<section><p></p><p></p></section>")]
        [InlineData("<section><p/><p/></section>")]
        [InlineData("<section><p></p><section></section></section>")]
        [InlineData("<section><p/><section/></section>")]
        [InlineData("<section><section></section><p></p></section>")]
        [InlineData("<section><section/><p/></section>")]
        [InlineData("<section><section></section><section></section></section>")]
        [InlineData("<section><section/><section/></section>")]
        public void ReadSection_complex(string input)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input);

            var res = sut.ReadSection(fixture);

            Assert.Equal(2, res.Blocks.Count);
        }

        [Theory]
        [InlineData("<body></body>", 0)]
        [InlineData("<body/>", 0)]
        [InlineData("<body><section></section></body>", 1)]
        [InlineData("<body><section/></body>", 1)]
        public void ReadBody_plain(string input, int expectedCount)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input);

            var res = sut.ReadBody(fixture);

            Assert.Equal(expectedCount, res.Blocks.Count);
        }

        [Theory]
        [InlineData("<body><section></section><section></section></body>")]
        [InlineData("<body><section/><section/></body>")]
        public void ReadBody_complex(string input)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input);

            var res = sut.ReadBody(fixture);

            Assert.Equal(2, res.Blocks.Count);
        }
    }
}
