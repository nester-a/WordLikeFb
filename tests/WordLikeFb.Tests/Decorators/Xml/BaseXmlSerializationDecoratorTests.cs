using System.Xml.Serialization;
using System.Xml;

namespace WordLikeFb.Tests.Decorators.Xml
{
    public class BaseXmlSerializationDecoratorTests
    {
        protected XmlSerializerNamespaces EmptyNameSpace { get; }
        protected XmlWriterSettings Settings { get; }
        public BaseXmlSerializationDecoratorTests()
        {
            EmptyNameSpace = new XmlSerializerNamespaces();
            EmptyNameSpace.Add(string.Empty, string.Empty);
            Settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
            };
        }
    }
}
