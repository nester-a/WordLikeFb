using System.Windows;
using System.Windows.Documents;
using System.Xml.Serialization;
using WordLikeFb.Common;

namespace WordLikeFb.Decorators.Xml
{
    [XmlType(TypeName = FbTypes.Strong)]
    public class StrongRunXmlSerializationDecorator : IXmlSerializationDecorator<Run>
    {
        public StrongRunXmlSerializationDecorator() : this(new()) { }

        public StrongRunXmlSerializationDecorator(Run target)
        {
            DecorationTarget = target;
        }

        [XmlIgnore]
        public Run DecorationTarget { get; }

        [XmlText(Type = typeof(string))]
        [XmlElement(FbTypes.Emphasis, Type = typeof(EmphasisRunXmlSerializationDecorator))]
        public List<object> Elements => new() { DecorationTarget.FontStyle == FontStyles.Italic ? new EmphasisRunXmlSerializationDecorator(DecorationTarget) : DecorationTarget.Text };
    }
}
