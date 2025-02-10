using System.Windows.Documents;
using System.Xml.Serialization;
using WordLikeFb.Common;
using WordLikeFb.Documents;

namespace WordLikeFb.Decorators
{
    [XmlType(TypeName = FbTypes.Section)]
    [XmlInclude(typeof(Paragraph)), XmlInclude(typeof(Title))]
    public class SectionXmlSerializationDecorator : Section, IBlockDecorator<Section>
    {
        public SectionXmlSerializationDecorator(): this(new()) { }

        public SectionXmlSerializationDecorator(Section section)
        {
            DecorationTarget = section;
        }

        [XmlElement(FbTypes.P, Type = typeof(Paragraph))]
        [XmlElement(FbTypes.Title, Type = typeof(Title))]
        public new BlockCollection Blocks => DecorationTarget.Blocks;

        [XmlIgnore]
        public Section DecorationTarget { get; }
    }
}
