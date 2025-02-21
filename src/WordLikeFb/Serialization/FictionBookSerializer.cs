using System.Text;
using System.Windows.Documents;
using System.Xml;
using System.Xml.Linq;
using WordLikeFb.Common;
using WordLikeFb.Documents;

namespace WordLikeFb.Serialization
{
    public class FictionBookSerializer
    {
        static readonly XNamespace _fb = "http://www.gribuser.ru/xml/fictionbook/2.0";
        readonly IFictionBookWriter _writer;
        readonly IFictionBookReader _reader;

        public FictionBookSerializer()
        {
            _writer = new FictionBookWriter();
            _reader = new FictionBookReader();
        }

        public FictionBookSerializer(IFictionBookWriter writer, IFictionBookReader reader)
        {
            _writer = writer;
            _reader = reader;
        }

        public string Serialize(FlowDocument flowDocument)
        {
            var sb = new StringBuilder();

            using (var writer = XmlWriter.Create(sb))
            {
                writer.WriteStartDocument();
                //XNamespace xlink = "http://www.w3.org/1999/xlink";
                //var links = new XAttribute(XNamespace.Xmlns + "xlink", xlink);
                //var root = new XElement(@namespace + "FictionBook", links);
                writer.WriteStartElement("FictionBook", _fb.NamespaceName);

                foreach (var block in flowDocument.Blocks)
                {
                    if(block is Body body)
                    {
                        _writer.WriteBody(writer, body);
                    }
                }

                writer.WriteEndElement();

                writer.WriteEndDocument();
            }

            return sb.ToString();
        }

        public FlowDocument Deserialize(string xml)
        {
            var doc = XDocument.Parse(xml);
            var flowDocument = new FlowDocument();

            var root = doc.Root;

            if(root is null)
            {
                return flowDocument;
            }

            foreach (var node in root.Elements())
            {
                if (node.Name.LocalName.Equals(FbTypes.Body))
                {
                    var block = _reader.ReadBody(node);
                    flowDocument.Blocks.Add(block);
                }
            }

            return flowDocument;
        }


    }
}
