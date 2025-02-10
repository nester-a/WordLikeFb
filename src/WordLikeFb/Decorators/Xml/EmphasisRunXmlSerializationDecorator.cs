using System.Windows;
using System.Windows.Documents;
using System.Xml.Serialization;
using WordLikeFb.Common;

namespace WordLikeFb.Decorators.Xml
{
    [XmlType(TypeName = FbTypes.Emphasis)]
    public class EmphasisRunXmlSerializationDecorator : IXmlSerializationDecorator<Run>
    {
        public EmphasisRunXmlSerializationDecorator() : this(new()) { }

        public EmphasisRunXmlSerializationDecorator(Run target)
        {
            DecorationTarget = target;
        }

        [XmlIgnore]
        public Run DecorationTarget { get; }

        [XmlText(Type = typeof(string))]
        [XmlElement(FbTypes.Strong, Type = typeof(StrongRunXmlSerializationDecorator))]
        public List<object> Elements => new() { DecorationTarget.FontWeight == FontWeights.Bold ? new StrongRunXmlSerializationDecorator(DecorationTarget) : DecorationTarget.Text };
    }
}
