using System.Windows.Documents;
using System.Xml.Linq;
using WordLikeFb.Serialization;

namespace WordLikeFb.Tests.Serialization
{
    public class FictionBookReaderTests
    {
        [Theory]
        [InlineData("<p>123</p>")]
        public void ReadParagraph_works_well(string input)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input);

            var res = sut.ReadParagraph(fixture);

            var text = string.Join(string.Empty, res.Inlines.Select(i => (i as Run).Text).ToArray());

            Assert.Equal("123", text);
        }

        [Theory]
        [InlineData("<p><strong>123</strong></p>")]
        public void ReadParagraph_works_well(string input)
        {
            var sut = new FictionBookReader();

            var fixture = XElement.Parse(input);

            var res = sut.ReadParagraph(fixture);

            var text = string.Join(string.Empty, res.Inlines.Select(i => (i as Run).Text).ToArray());

            Assert.Equal("123", text);
        }
    }
}
