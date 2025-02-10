using System.Windows;
using System.Windows.Documents;
using System.Xml.Serialization;
using WordLikeFb.Common;

namespace WordLikeFb.Decorators.Xml
{

    [XmlType(TypeName = FbTypes.P)]
    public class ParagraphXmlSerializationDecorator : IXmlSerializationDecorator<Paragraph>
    {
        public ParagraphXmlSerializationDecorator() : this(new()) { }

        public ParagraphXmlSerializationDecorator(Paragraph target) 
        {
            DecorationTarget = target;
        }

        [XmlIgnore]
        public Paragraph DecorationTarget { get; }

        [XmlText(Type = typeof(string))]
        [XmlElement(FbTypes.Strong, Type = typeof(StrongRunXmlSerializationDecorator))]
        [XmlElement(FbTypes.Emphasis, Type = typeof(EmphasisRunXmlSerializationDecorator))]
        public List<object> Elements => DecorationTarget.Inlines
                                                        .Select(i =>
                                                        {
                                                            object result;
                                                            var run = i as Run;

                                                            if(run is null)
                                                            {
                                                                result = string.Empty;
                                                            }
                                                            else if(run.FontWeight == FontWeights.Bold)
                                                            {
                                                                result = new StrongRunXmlSerializationDecorator(run);
                                                            }
                                                            else if(run.FontStyle == FontStyles.Italic)
                                                            {
                                                                result = new EmphasisRunXmlSerializationDecorator(run);
                                                            }
                                                            else
                                                            {
                                                                result = run.Text;
                                                            }

                                                            return result;

                                                        })
                                                        .ToList();
    }
}
