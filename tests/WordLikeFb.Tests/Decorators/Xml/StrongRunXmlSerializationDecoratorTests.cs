using System.Text;
using System.Xml.Serialization;
using System.Xml;
using WordLikeFb.Decorators.Xml;
using System.Windows.Documents;
using System.Windows;

namespace WordLikeFb.Tests.Decorators.Xml
{
    public class StrongRunXmlSerializationDecoratorTests : BaseXmlSerializationDecoratorTests
    {
        private readonly XmlSerializer _serializer;

        public StrongRunXmlSerializationDecoratorTests()
        {
            _serializer = new XmlSerializer(typeof(StrongRunXmlSerializationDecorator));
        }

        [Theory]
        [InlineData(true, "123", "<strong><emphasis>123</emphasis></strong>")]
        [InlineData(false, "123", "<strong>123</strong>")]
        public void Serialized(bool isItalic, string input, string expectedXml)
        {
            var r = new Run()
            {
                Text = input,
                FontStyle = isItalic ? FontStyles.Italic : FontStyles.Normal
            };
            var sut = new StrongRunXmlSerializationDecorator(r);

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, Settings))
            {
                _serializer.Serialize(writer, sut, EmptyNameSpace);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }
    }
}
