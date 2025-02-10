using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WordLikeFb.Decorators.Xml;

namespace WordLikeFb.Tests
{
    public class TitleXmlSerializationDecoratorTests : BaseXmlSerializationDecoratorTests
    {
        private readonly XmlSerializer _serializer;

        public TitleXmlSerializationDecoratorTests()
        {
            _serializer = new XmlSerializer(typeof(TitleXmlSerializationDecorator));
        }

        [Theory]
        [InlineData("123","<title>123</title>")]
        [InlineData("","<title>***</title>")]
        public void Serialized(string titleInput, string expectedXml)
        {
            var sut = new TitleXmlSerializationDecorator(string.IsNullOrEmpty(titleInput) ? new() : new(titleInput));

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, Settings))
            {
                _serializer.Serialize(writer, sut, EmptyNameSpace);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }
    }
}
