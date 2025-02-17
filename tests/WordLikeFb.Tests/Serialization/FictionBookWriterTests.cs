using System.Text;
using System.Windows.Documents;
using System.Xml;
using WordLikeFb.Documents;
using WordLikeFb.Serialization;

namespace WordLikeFb.Tests.Serialization
{
    public class FictionBookWriterTests
    {
        [Theory]
        [InlineData("<body />", 0)]
        [InlineData("<body><section /></body>", 1)]
        [InlineData("<body><section /><section /></body>", 2)]
        public void WriteBody_tests(string expectedJson, int childCount)
        {
            var b = new Body();

            for (int i = 0; i < childCount; i++)
            {
                b.Blocks.Add(new Section());
            }

            var sb = new StringBuilder();

            var sut = new FictionBookWriter();
            var opt = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
            };
            using (var writer = XmlWriter.Create(sb, opt))
            {
                sut.WriteBody(writer, b);
            }

            Assert.Equal(expectedJson, sb.ToString());
        }
    }
}
