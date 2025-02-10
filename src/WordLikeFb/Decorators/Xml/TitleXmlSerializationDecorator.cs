using System.Text;
using System.Windows.Documents;
using System.Xml.Serialization;
using WordLikeFb.Common;
using WordLikeFb.Documents;

namespace WordLikeFb.Decorators.Xml
{
    [XmlType(TypeName = FbTypes.Title)]
    public class TitleXmlSerializationDecorator : IXmlSerializationDecorator<Title>
    {
        public TitleXmlSerializationDecorator() : this(new()) { }

        public TitleXmlSerializationDecorator(Title target)
        {
            DecorationTarget = target;
        }

        [XmlIgnore]
        public Title DecorationTarget { get; }

        [XmlText(Type = typeof(string))]
        public List<object> Elements => DecorationTarget.Blocks
                                                        .Select(par =>
                                                        {
                                                            var p = par as Paragraph;
                                                            var sb = new StringBuilder();

                                                            foreach (var run in p.Inlines)
                                                            {
                                                                sb.Append((run as Run)?.Text ?? string.Empty);
                                                            }

                                                            return sb.ToString() as object;
                                                        })
                                                        .ToList();
    }
}
