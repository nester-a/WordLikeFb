using System.Windows.Documents;
using System.Xml.Linq;
using WordLikeFb.Documents;

namespace WordLikeFb.Serialization
{
    internal interface IFictionBookReader
    {
        Body ReadBody(XElement bodyNode);
        Section ReadSection(XElement sectionNode);
        Paragraph ReadParagraph(XElement pNode);
    }
}
