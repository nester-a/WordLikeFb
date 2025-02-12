using System.Windows.Documents;
using System.Xml;
using WordLikeFb.Documents;

namespace WordLikeFb.Serialization
{
    internal interface IFictionBookWriter
    {
        void WriteBody(XmlWriter writer, Body body);
        void WriteSection(XmlWriter writer, Section section);
        void WriteParagraph(XmlWriter writer, Paragraph p);
    }
}
