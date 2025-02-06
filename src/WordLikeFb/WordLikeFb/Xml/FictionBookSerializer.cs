using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Xml.Linq;
using WordLikeFb.Documents;
using WordLikeFb.Extensions;
using WordLikeFb.Factories;

namespace WordLikeFb.Xml
{
    internal static class FictionBookSerializer
    {
        static readonly XNamespace _fb = "http://www.gribuser.ru/xml/fictionbook/2.0";

        public static FrameworkContentElement? Deserialize(XNode node)
        {
            if (node is XDocument document)
            {
                var flowDoc = new FlowDocument();

                var bodyElemName = FictionBookElementsFactory.CreateBodyName();
                var bodies = document.Root?.Elements(bodyElemName);

                var childs = bodies?.Nodes() ?? [];

                foreach (var child in childs)
                {
                    var elem = Deserialize(child);
                    flowDoc.Blocks.Add(elem as Block);
                }
                return flowDoc;
            }
            else if(node is XElement element)
            {

            }
            else if(node is XText text)
            {
                var run = new Run();
            }
        }


        public static XNode? Serialize(FrameworkContentElement content)
        {
            if (content is FlowDocument document)
            {
                var doc = new XDocument(new XDeclaration("1.0", "iso-8859-1", "yes"));
                var root = FictionBookElementsFactory.CreateFictionBookRoot(_fb);
                doc.Add(root);
                var body = new XElement(_fb + "body");
                root.Add(body);

                foreach(var block in document.Blocks)
                {
                    var element = Serialize(block);
                    body.Add(element);
                }
                return doc;
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
