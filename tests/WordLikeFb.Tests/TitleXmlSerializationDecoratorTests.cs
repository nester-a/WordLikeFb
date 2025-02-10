using System.Text;
using System.Xml;
using System.Xml.Serialization;
using WordLikeFb.Decorators.Xml;

namespace WordLikeFb.Tests
{
    public class TitleXmlSerializationDecoratorTests
    {
        private readonly XmlSerializer _serializer;
        private readonly XmlSerializerNamespaces _emptyNameSpace;
        private readonly XmlWriterSettings _settings;

        public TitleXmlSerializationDecoratorTests()
        {
            _serializer = new XmlSerializer(typeof(TitleXmlSerializationDecorator));
            _emptyNameSpace = new XmlSerializerNamespaces();
            _emptyNameSpace.Add(string.Empty, string.Empty);
            _settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                Indent = true
            };
        }

        [Theory]
        [InlineData("123","<title>123</title>")]
        [InlineData("","<title>***</title>")]
        public void Test1(string titleInput, string expectedXml)
        {
            var sut = new TitleXmlSerializationDecorator(string.IsNullOrEmpty(titleInput) ? new() : new(titleInput));

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, _settings))
            {
                _serializer.Serialize(writer, sut, _emptyNameSpace);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }
    }
}
