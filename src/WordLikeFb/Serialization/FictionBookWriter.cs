using System.Windows.Documents;
using System.Windows;
using System.Xml;
using WordLikeFb.Common;
using WordLikeFb.Documents;

namespace WordLikeFb.Serialization
{
    public class FictionBookWriter : IFictionBookWriter
    {
        public void WriteBody(XmlWriter writer, Body body)
        {
            writer.WriteStartElement(FbTypes.Body);

            foreach (var block in body.Blocks)
            {
                if (block is Section section)
                    WriteSection(writer, section);
            }

            writer.WriteEndElement();
        }

        public void WriteSection(XmlWriter writer, Section section)
        {
            writer.WriteStartElement(FbTypes.Section);

            foreach (var block in section.Blocks)
            {
                if (block is Section subSection)
                    WriteSection(writer, subSection);
                else if (block is Paragraph p)
                    WriteParagraph(writer, p);
            }

            writer.WriteEndElement();
        }

        public void WriteParagraph(XmlWriter writer, Paragraph p)
        {
            writer.WriteStartElement(FbTypes.P);

            foreach (Run run in p.Inlines)
            {
                if (run.FontWeight == FontWeights.Normal && run.FontStyle == FontStyles.Normal)
                {
                    writer.WriteValue(run.Text);
                }
                else if (run.FontWeight == FontWeights.Bold && run.FontStyle == FontStyles.Italic)
                {
                    writer.WriteStartElement(FbTypes.Strong);
                    writer.WriteStartElement(FbTypes.Emphasis);
                    writer.WriteValue(run.Text);
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                }
                else if (run.FontWeight == FontWeights.Bold)
                {
                    writer.WriteStartElement(FbTypes.Strong);
                    writer.WriteValue(run.Text);
                    writer.WriteEndElement();
                }
                else if (run.FontStyle == FontStyles.Italic)
                {
                    writer.WriteStartElement(FbTypes.Emphasis);
                    writer.WriteValue(run.Text);
                    writer.WriteEndElement();
                }
            }

            writer.WriteEndElement();
        }
    }
}
