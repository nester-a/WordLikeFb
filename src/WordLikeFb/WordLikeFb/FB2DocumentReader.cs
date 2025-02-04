using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace WordLikeFb
{
    internal static class FB2DocumentReader
    {
        public static XDocument Read(string filePath)
        {
            var schemeFile = new string[] { "FictionBook.xsd", "FictionBookGenres.xsd", "FictionBookLang.xsd", "FictionBookLinks.xsd", "FictionNotes.xsd" };

            var schemas = new XmlSchemaSet();

            foreach(var scheme in schemeFile)
            {
                var schemePath = Path.Combine(Environment.CurrentDirectory, "schemes", scheme);
                schemas.Add(null, schemePath);
            }

            var settings = new XmlReaderSettings()
            {
                ValidationType = ValidationType.Schema,
                Schemas = schemas,
            };

            using var reader = XmlReader.Create(filePath, settings);
            return XDocument.Load(reader, LoadOptions.None);
        }
    }
}
