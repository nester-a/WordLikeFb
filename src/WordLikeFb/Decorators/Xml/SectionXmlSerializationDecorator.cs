using System.Windows.Documents;
using System.Xml.Serialization;
using WordLikeFb.Common;
using WordLikeFb.Documents;

namespace WordLikeFb.Decorators.Xml
{
    [XmlType(TypeName = FbTypes.Section)]
    public class SectionXmlSerializationDecorator : IXmlSerializationDecorator<Section>
    {
        public SectionXmlSerializationDecorator() : this(new()) { }

        public SectionXmlSerializationDecorator(Section target)
        {
            DecorationTarget = target;
        }

        [XmlIgnore]
        public Section DecorationTarget { get; }

        [XmlElement(FbTypes.P, Type = typeof(ParagraphXmlSerializationDecorator))]
        [XmlElement(FbTypes.Title, Type = typeof(TitleXmlSerializationDecorator))]
        [XmlElement(FbTypes.Title, Type = typeof(SectionXmlSerializationDecorator))]
        public List<object> Elements => DecorationTarget.Blocks
                                                        .Select(b =>
                                                        {
                                                            object result;
                                                            if(b is Section section)
                                                            {
                                                                result = new SectionXmlSerializationDecorator(section);
                                                            }
                                                            else if(b is Title title)
                                                            {
                                                                result = new TitleXmlSerializationDecorator(title);
                                                            }
                                                            else
                                                            {
                                                                result = new ParagraphXmlSerializationDecorator((b as Paragraph)!);
                                                            }
                                                            return result;
                                                        })
                                                        .ToList();
    }
}
