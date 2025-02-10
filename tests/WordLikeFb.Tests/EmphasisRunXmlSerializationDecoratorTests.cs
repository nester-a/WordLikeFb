using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Xml.Serialization;
using System.Xml;
using WordLikeFb.Decorators.Xml;

namespace WordLikeFb.Tests
{
    public class EmphasisRunXmlSerializationDecoratorTests : BaseXmlSerializationDecoratorTests
    {
        private readonly XmlSerializer _serializer;

        public EmphasisRunXmlSerializationDecoratorTests()
        {
            _serializer = new XmlSerializer(typeof(EmphasisRunXmlSerializationDecorator));
        }

        [Theory]
        [InlineData(true, "123", "<emphasis><strong>123</strong></emphasis>")]
        [InlineData(false, "123", "<emphasis>123</emphasis>")]
        public void Serialized(bool isBold, string input, string expectedXml)
        {
            var r = new Run()
            {
                Text = input,
                FontWeight = isBold ? FontWeights.Bold : FontWeights.Normal
            };
            var sut = new EmphasisRunXmlSerializationDecorator(r);

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, Settings))
            {
                _serializer.Serialize(writer, sut, EmptyNameSpace);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }
    }
}
