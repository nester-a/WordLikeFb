using System.Windows.Documents;
using System.Windows;
using System.Xml.Linq;
using WordLikeFb.Common;
using WordLikeFb.Documents;

namespace WordLikeFb.Serialization
{
    internal class FictionBookReader : IFictionBookReader
    {
        public Body ReadBody(XElement bodyNode)
        {
            var body = new Body();

            foreach (var node in bodyNode.Elements())
            {
                if (node.Name.LocalName.Equals(FbTypes.Section))
                {
                    var block = ReadBody(node);
                    body.Blocks.Add(block);
                }
            }

            return body;
        }

        public Section ReadSection(XElement sectionNode)
        {
            var section = new Section();

            foreach (var node in sectionNode.Elements())
            {
                switch (node.Name.LocalName)
                {
                    case FbTypes.Section:
                        var subSection = ReadSection(node);
                        section.Blocks.Add(subSection);
                        break;
                    case FbTypes.P:
                        var p = ReadParagraph(node);
                        section.Blocks.Add(p);
                        break;
                }
            }

            return section;
        }

        public Paragraph ReadParagraph(XElement pNode)
        {
            var paragraph = new Paragraph();

            foreach (var node in pNode.Elements())
            {
                var run = new Run() { Text = node.Value };

                switch (node.Name.LocalName)
                {
                    case FbTypes.Strong:
                        run.FontWeight = FontWeights.Bold;
                        break;
                    case FbTypes.Emphasis:
                        run.FontStyle = FontStyles.Italic;
                        break;
                }

                if (node.HasElements)
                {
                    var childNode = node.Element(XName.Get(node.Elements().First().Name.ToString()));

                    if (childNode.Name.LocalName == FbTypes.Emphasis)
                        run.FontStyle = FontStyles.Italic;
                    else if (childNode.Name.LocalName == FbTypes.Strong)
                        run.FontWeight = FontWeights.Bold;
                }
                paragraph.Inlines.Add(run);
            }

            return paragraph;
        }
    }
}
