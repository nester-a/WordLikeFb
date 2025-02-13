using System.Windows.Documents;
using System.Windows;
using System.Xml.Linq;
using WordLikeFb.Common;
using WordLikeFb.Documents;

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
                var childs = pNode.Nodes();
                foreach (var child in childs)
                {
                    if(child is XText text)
                    {
                        var run = new Run() { Text = text.Value };
                        paragraph.Inlines.Add(run);
                    }
                    else if (child is XElement element)
                    {
                        switch (element.Name.LocalName)
                        {
                            case FbTypes.Strong:
                                var strong = ReadStrong(element);
                                paragraph.Inlines.Add(strong);
                                break;
                            case FbTypes.Emphasis:
                                var emphasis = ReadEmphasis(element);
                                paragraph.Inlines.Add(emphasis);
                                break;
                        }
                    }
                }
            }

            return paragraph;
        }

        Run ReadStrong(XElement strongNode)
        {
            if (!strongNode.HasElements)
            {
                var run = new Run();
                run.Text = strongNode.Value;
                run.FontWeight = FontWeights.Bold;
                return run;
            }
            else
            {
                Run? strong = null;
                foreach (var child in strongNode.Elements())
                {
                    switch (child.Name.LocalName)
                    {
                        case FbTypes.Emphasis:
                            strong = ReadEmphasis(child);
                            break;
                    }
                }

                if (strong is null)
                {
                    strong = new();
                }

                strong.FontWeight = FontWeights.Bold;
                return strong;
            }
        }

        Run ReadEmphasis(XElement emphasisNode)
        {
            if (!emphasisNode.HasElements)
            {
                var run = new Run();
                run.Text = emphasisNode.Value;
                run.FontStyle = FontStyles.Italic;
                return run;
            }
            else
            {
                Run? emphasis = null;
                foreach (var child in emphasisNode.Elements())
                {
                    switch (child.Name.LocalName)
                    {
                        case FbTypes.Strong:
                            emphasis = ReadStrong(child);
                            break;
                    }
                }

                if (emphasis is null)
                {
                    emphasis = new();
                }

                emphasis.FontStyle = FontStyles.Italic;
                return emphasis;
            }
        }
    }
}
