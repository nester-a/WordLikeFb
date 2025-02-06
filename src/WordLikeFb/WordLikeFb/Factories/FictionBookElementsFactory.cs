using System.Xml.Linq;

namespace WordLikeFb.Factories
{
    internal class FictionBookElementsFactory
    {
        public static XElement CreateFictionBookRoot(XNamespace @namespace)
        {
            XNamespace xlink = "http://www.w3.org/1999/xlink";
            var links = new XAttribute(XNamespace.Xmlns + "xlink", xlink);
            var root = new XElement(@namespace + "FictionBook", links);

            return root;
        }

        public static XElement CreateDescription()
        {
            return CreateDescription(CreateTitleInfo(), CreateDocumentInfo());
        }

        public static XElement CreateDescription(params XElement[] elements)
        {
            return new XElement("description");
        }

        public static XElement CreateTitleInfo()
        {
            return new XElement("title-info");
        }

        public static XElement CreateDocumentInfo()
        {
            return new XElement("document-info");
        }

        public static XName CreateBodyName()
        {
            return XName.Get("body", "http://www.gribuser.ru/xml/fictionbook/2.0");
        }
    }
}
