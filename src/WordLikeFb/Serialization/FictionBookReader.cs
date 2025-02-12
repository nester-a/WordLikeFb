using System.Windows.Documents;
using System.Windows;
using System.Xml.Linq;
using WordLikeFb.Common;
using WordLikeFb.Documents;
using System.Text;

namespace WordLikeFb.Serialization
{
    public class FictionBookReader : IFictionBookReader
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

            if (!pNode.HasElements)
            {
                var run = new Run() { Text = pNode.Value };
                paragraph.Inlines.Add(run);
            }
            else
            {
                foreach (var child in pNode.Elements())
                {
                    var run = new Run() { Text = child.Value };

                    switch (child.Name.LocalName)
                    {
                        case FbTypes.Strong:
                            run.FontWeight = FontWeights.Bold;
                            break;
                        case FbTypes.Emphasis:
                            run.FontStyle = FontStyles.Italic;
                            break;
                    }

                    if (child.HasElements)
                    {
                        switch (child.Name.LocalName)
                        {
                            case FbTypes.Strong:
                                run.FontStyle = FontStyles.Italic;
                                break;
                            case FbTypes.Emphasis:
                                run.FontWeight = FontWeights.Bold;
                                break;
                        }
                    }

                    paragraph.Inlines.Add(run);
                }
            }

            return paragraph;
        }
    }
}
