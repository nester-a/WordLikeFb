using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.Linq;

namespace WordLikeFb.Extensions
{
    internal static class XElementExtensions
    {
        static readonly string[] _inlines = { "emphasis", "strong" };
        static readonly string[] _blocks = { "section", "p" };

        public static void FillTextElement(this XNode node, FrameworkContentElement contentElement)
        {
            if(node is XText text)
            {
                var run = (Run)(contentElement is Run ? contentElement : new Run());
                var value = Regex.Replace(text.Value, @"\s+", " ");
                run.Text += value;

                if(contentElement is Paragraph paragraph)
                {
                    paragraph.Inlines.Add(run);
                }
            }
            else if(node is XElement element)
            {
                var elementName = element.Name.LocalName;
                var childs = element.Nodes();

                FrameworkContentElement? currentElement = null;

                if (_inlines.Any(el => el.Equals(elementName)))
                {
                    var run = (Run)(contentElement is Run ? contentElement : new Run());
                    switch (elementName)
                    {
                        case "emphasis":
                            run.FontStyle = FontStyles.Italic;
                            break;
                        case "strong":
                            run.FontWeight = FontWeights.Bold;
                            break;
                    }

                    if (contentElement is Paragraph paragraph)
                    {
                        paragraph.Inlines.Add(run);
                    }
                    currentElement = run;
                }
                else if(_blocks.Any(b => b.Equals(elementName)))
                {
                    Block block;
                    switch (elementName)
                    {
                        case "section":
                            block = new Section();
                            break;
                        case "p":
                            block = new Paragraph();
                            break;
                        default:
                            return;
                    }

                    if(contentElement is IAddChild paragraph)
                    {
                        paragraph.AddChild(block);
                    }

                    currentElement = block;
                }

                if(currentElement is null)
                {
                    return;
                }

                foreach (var child in childs)
                {
                    child.FillTextElement(currentElement);
                }
            }
        }
    }
}
