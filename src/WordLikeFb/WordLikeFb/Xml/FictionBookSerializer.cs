using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Xml.Linq;
using WordLikeFb.Documents;
using WordLikeFb.Factories;

namespace WordLikeFb.Xml
{
    internal static class FictionBookSerializer
    {
        static readonly XNamespace _fb = "http://www.gribuser.ru/xml/fictionbook/2.0";
        static readonly string[] _inlines = { "emphasis", "strong" };
        static readonly string[] _blocks = { "body", "section", "title", "p" };

        public static FrameworkContentElement? Deserialize(XNode node)
        {
            if(node is XDocument document)
            {
                var flowDoc = new FlowDocument();

                var childs = document.Nodes();

                foreach (var child in childs)
                {
                    var childElement = Deserialize(child);
                    if (childElement != null && childElement is Block addChild)
                    {
                        flowDoc.Blocks.Add(addChild);
                    }
                }

                return flowDoc;
            }
            else if (node is XText text)
            {
                var run = new Run();
                var value = Regex.Replace(text.Value, @"\s+", " ");
                run.Text += value;
                return run;
            }
            else if (node is XElement element)
            {
                var elementName = element.Name.LocalName;
                var childs = element.Nodes();

                FrameworkContentElement? currentElement = null;

                if (_inlines.Contains(elementName))
                {
                    var run = new Run();
                    switch (elementName)
                    {
                        case "emphasis":
                            run.FontStyle = FontStyles.Italic;
                            break;
                        case "strong":
                            run.FontWeight = FontWeights.Bold;
                            break;
                    }
                    currentElement = run;
                }
                else if (_blocks.Contains(elementName))
                {
                    Block block;
                    switch (elementName)
                    {
                        case "body":
                            block = new Body();
                            break;
                        case "section":
                            block = new Section();
                            break;
                        case "title":
                            block = new Title();
                            break;
                        case "p":
                            block = new Paragraph();
                            break;
                        default:
                            return null;
                    }
                    currentElement = block;
                }

                if (currentElement == null)
                {
                    return null;
                }

                foreach (var child in childs)
                {
                    var childElement = Deserialize(child);
                    if (childElement != null && currentElement is IAddChild addChild)
                    {
                        addChild.AddChild(childElement);
                    }
                }

                return currentElement;
            }

            return null;
        }


        public static XNode? Serialize(FrameworkContentElement content)
        {
            if (content is FlowDocument document)
            {
                var doc = new XDocument(new XDeclaration("1.0", "iso-8859-1", "yes"));
                var root = FictionBookElementsFactory.CreateFictionBookRoot(_fb);
                doc.Add(root);

                foreach(var block in document.Blocks)
                {
                    var element = Serialize(block);
                    root.Add(element);
                }
                return doc;
            }
            else if (content is Body body)
            {
                var b = new XElement(_fb + "body");

                foreach (var block in body.Blocks)
                {
                    var element = Serialize(block);
                    b.Add(element);
                }
                return b;
            }
            else if (content is Section section)
            {
                var sect = new XElement(_fb + "section");

                foreach (var block in section.Blocks)
                {
                    var element = Serialize(block);
                    sect.Add(element);
                }

                return sect;
            }
            else if(content is Title title)
            {
                var t = new XElement(_fb + "title");

                foreach (var block in title.Inlines)
                {
                    var txt = block is Run run ? run.Text : "";
                    t.Add(new XText(txt));
                }

                return t;
            }
            else if(content is Paragraph paragraph)
            {
                var p = new XElement(_fb + "p");

                foreach (var block in paragraph.Inlines)
                {
                    var element = Serialize(block);
                    p.Add(element);
                }

                return p;
            }
            else if(content is Run run)
            {
                XElement? el = null;

                if (run.FontWeight == FontWeights.Bold)
                {
                    el = new XElement(_fb + "strong");
                }

                if (run.FontStyle == FontStyles.Italic)
                {
                    var italic = new XElement(_fb + "italic");
                    if (el == null)
                    {
                        el = italic;
                    }
                    else
                    {
                        el.Add(italic);
                    }
                }

                var text = new XText(run.Text);
                if(el == null)
                {
                    return text;
                }
                else
                {
                    el.Add(text);
                    return el;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
