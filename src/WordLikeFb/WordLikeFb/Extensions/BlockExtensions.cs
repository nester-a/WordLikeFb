using System.Windows;
using System.Windows.Documents;
using System.Xml.Linq;

namespace WordLikeFb.Extensions
{
    internal static class BlockExtensions
    {
        public static void TransformTooXElement(this FrameworkContentElement contentElement, XElement parent)
        {
            var contentElementTypeName = contentElement.GetType().Name;

            XElement? currentElement = null;

            switch (contentElementTypeName)
            {
                case nameof(FlowDocument):
                    currentElement = new XElement("body");
                    break;
                case nameof(Section):
                    currentElement = new XElement("section");
                    break;
                case nameof(Paragraph):
                    currentElement = new XElement("p");
                    break;
                case nameof(Run):
                    var run = (Run)contentElement;

                    if(run.FontWeight == FontWeights.Bold)
                    {
                        currentElement = new XElement("strong");
                    }
                    if(run.FontStyle == FontStyles.Italic)
                    {
                        var emph = new XElement("emphasis");
                        if (currentElement is null)
                        {
                            currentElement = emph;
                        }
                        else
                        {
                            currentElement.Add(emph);
                        }
                    }
                    else
                    {
                        parent.Add(run.Text);
                        return;
                    }
                    break;
                default:
                    return;
            }

            parent.Add(currentElement);

            if (contentElement is FlowDocument doc)
            {
                foreach (var block in doc.Blocks)
                {
                    block.TransformTooXElement(currentElement);
                }
            }
            else if(contentElement is Section section)
            {
                foreach (var block in section.Blocks)
                {
                    block.TransformTooXElement(currentElement);
                }
            }
            else if(contentElement is Paragraph paragraph)
            {
                foreach (var inline in paragraph.Inlines)
                {
                    inline.TransformTooXElement(currentElement);
                }
            }
        }
    }
}
