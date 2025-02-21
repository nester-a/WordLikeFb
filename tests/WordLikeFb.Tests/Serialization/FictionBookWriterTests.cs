using System.Text;
using System.Windows;
using System.Windows.Documents;
using System.Xml;
using WordLikeFb.Documents;
using WordLikeFb.Serialization;

namespace WordLikeFb.Tests.Serialization
{
    public enum SectionChildType
    {
        None,
        Title,
        Section,
        Paragraph,
        Both
    }

    public enum ParagraphChildType
    {
        None,
        Italic,
        Bold,
        Both
    }

    public class FictionBookWriterTests
    {
        [Theory]
        [InlineData("<body />", 0)]
        [InlineData("<body><section /></body>", 1)]
        [InlineData("<body><section /><section /></body>", 2)]
        public void WriteBody_tests(string expectedXml, int childCount)
        {
            var b = new Body();

            for (int i = 0; i < childCount; i++)
            {
                b.Blocks.Add(new Section());
            }

            var sb = new StringBuilder();

            var sut = new FictionBookWriter();
            var opt = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
            };
            using (var writer = XmlWriter.Create(sb, opt))
            {
                sut.WriteBody(writer, b);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }

        [Theory]
        [InlineData("<section />", SectionChildType.None)]
        [InlineData("<section><section /></section>", SectionChildType.Section)]
        [InlineData("<section><p /></section>", SectionChildType.Paragraph)]
        [InlineData("<section><p /><section /></section>", SectionChildType.Both)]
        public void WriteSection_tests(string expectedXml, SectionChildType childs)
        {
            var parent = new Section();

            switch (childs)
            {
                case SectionChildType.Section:
                    parent.Blocks.Add(new Section());
                    break;
                case SectionChildType.Paragraph: 
                    parent.Blocks.Add(new Paragraph());
                    break;
                case SectionChildType.Both:
                    parent.Blocks.Add(new Paragraph());
                    parent.Blocks.Add(new Section());
                    break;
                default:
                    break;
            }

            var sb = new StringBuilder();

            var sut = new FictionBookWriter();
            var opt = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
            };
            using (var writer = XmlWriter.Create(sb, opt))
            {
                sut.WriteSection(writer, parent);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }

        [Theory]
        [InlineData("<p>123</p>", ParagraphChildType.None)]
        [InlineData("<p><strong>123</strong></p>", ParagraphChildType.Bold)]
        [InlineData("<p><emphasis>123</emphasis></p>", ParagraphChildType.Italic)]
        [InlineData("<p><strong><emphasis>123</emphasis></strong></p>", ParagraphChildType.Both)]
        public void WriteParagraph_tests(string expectedXml, ParagraphChildType childs)
        {
            var parent = new Paragraph();

            switch (childs)
            {
                case ParagraphChildType.Bold:
                    var bold = new Run()
                    {
                        Text = "123",
                        FontWeight = FontWeights.Bold
                    };
                    parent.Inlines.Add(bold); ;
                    break;
                case ParagraphChildType.Italic:
                    var italic = new Run()
                    {
                        Text = "123",
                        FontStyle = FontStyles.Italic,
                    };
                    parent.Inlines.Add(italic);
                    break;
                case ParagraphChildType.Both:
                    var run = new Run()
                    {
                        Text = "123",
                        FontStyle = FontStyles.Italic,
                        FontWeight = FontWeights.Bold
                    };
                    parent.Inlines.Add(run);
                    break;
                case ParagraphChildType.None:
                    var text = new Run()
                    {
                        Text = "123",
                    };
                    parent.Inlines.Add(text);
                    break;
                default:
                    break;
            }

            var sb = new StringBuilder();

            var sut = new FictionBookWriter();
            var opt = new XmlWriterSettings()
            {
                OmitXmlDeclaration = true,
            };
            using (var writer = XmlWriter.Create(sb, opt))
            {
                sut.WriteParagraph(writer, parent);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }
    }
}
