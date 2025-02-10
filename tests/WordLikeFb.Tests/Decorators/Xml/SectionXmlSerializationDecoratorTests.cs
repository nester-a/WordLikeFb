using System;
using System.Text;
using System.Xml.Serialization;
using WordLikeFb.Decorators.Xml;

namespace WordLikeFb.Tests.Decorators.Xml
{
    public class SectionXmlSerializationDecoratorTests
    {
        private readonly XmlSerializer _serializer;

        public SectionXmlSerializationDecoratorTests()
        {
            _serializer = new XmlSerializer(typeof(SectionXmlSerializationDecorator));
        }

        public void Test1()
        {
            var sut = new SectionXmlSerializationDecorator();

            var sb = new StringBuilder();
            using (var writer = new StringWriter(sb))
            {
                _serializer.Serialize(writer, sut);
            }

            Assert.NotNull(sb.ToString());
        }
    }
}