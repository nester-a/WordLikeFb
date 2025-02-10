using System.Text;
using System.Windows.Documents;
using System.Windows;
using System.Xml;
using System.Xml.Serialization;
using WordLikeFb.Decorators.Xml;

namespace WordLikeFb.Tests
{
    public enum ParagraphStyles
    {
        None,
        Strong,
        Emphasis
    }

    public class ParagraphXmlSerializationDecoratorTests : BaseXmlSerializationDecoratorTests
    {

        private readonly XmlSerializer _serializer;

        public ParagraphXmlSerializationDecoratorTests()
        {
            _serializer = new XmlSerializer(typeof(ParagraphXmlSerializationDecorator));
        }

        [Theory]
        [InlineData(ParagraphStyles.None, "123", "<p>123</p>")]
        [InlineData(ParagraphStyles.Strong, "123", "<p><strong>123</strong></p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", "<p><emphasis>123</emphasis></p>")]
        public void PlainSerialized(ParagraphStyles style, string input, string expectedXml)
        {
            var r = new Run()
            {
                Text = input,
                FontWeight = style == ParagraphStyles.Strong ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = style == ParagraphStyles.Emphasis ? FontStyles.Italic : FontStyles.Normal
            };
            var p = new Paragraph(r);
            var sut = new ParagraphXmlSerializationDecorator(p);

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, Settings))
            {
                _serializer.Serialize(writer, sut, EmptyNameSpace);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }

        [Theory]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.None, "123", ParagraphStyles.None, "123", "<p>123123123</p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.None, "123", ParagraphStyles.Strong, "123", "<p>123123<strong>123</strong></p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.None, "123", ParagraphStyles.Emphasis, "123", "<p>123123<emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.Strong, "123", ParagraphStyles.None, "123", "<p>123<strong>123</strong>123</p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.None, "123", "<p>123<emphasis>123</emphasis>123</p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.Strong, "123", ParagraphStyles.Strong, "123", "<p>123<strong>123</strong><strong>123</strong></p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.Strong, "123", "<p>123<emphasis>123</emphasis><strong>123</strong></p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.Strong, "123", ParagraphStyles.Emphasis, "123", "<p>123<strong>123</strong><emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.None, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.Emphasis, "123", "<p>123<emphasis>123</emphasis><emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.None, "123", ParagraphStyles.None, "123", "<p><strong>123</strong>123123</p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.None, "123", ParagraphStyles.Strong, "123", "<p><strong>123</strong>123<strong>123</strong></p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.None, "123", ParagraphStyles.Emphasis, "123", "<p><strong>123</strong>123<emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.Strong, "123", ParagraphStyles.None, "123", "<p><strong>123</strong><strong>123</strong>123</p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.None, "123", "<p><strong>123</strong><emphasis>123</emphasis>123</p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.Strong, "123", ParagraphStyles.Strong, "123", "<p><strong>123</strong><strong>123</strong><strong>123</strong></p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.Strong, "123", "<p><strong>123</strong><emphasis>123</emphasis><strong>123</strong></p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.Strong, "123", ParagraphStyles.Emphasis, "123", "<p><strong>123</strong><strong>123</strong><emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.Strong, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.Emphasis, "123", "<p><strong>123</strong><emphasis>123</emphasis><emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.None, "123", ParagraphStyles.None, "123", "<p><emphasis>123</emphasis>123123</p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.None, "123", ParagraphStyles.Strong, "123", "<p><emphasis>123</emphasis>123<strong>123</strong></p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.None, "123", ParagraphStyles.Emphasis, "123", "<p><emphasis>123</emphasis>123<emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.Strong, "123", ParagraphStyles.None, "123", "<p><emphasis>123</emphasis><strong>123</strong>123</p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.None, "123", "<p><emphasis>123</emphasis><emphasis>123</emphasis>123</p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.Strong, "123", ParagraphStyles.Strong, "123", "<p><emphasis>123</emphasis><strong>123</strong><strong>123</strong></p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.Strong, "123", "<p><emphasis>123</emphasis><emphasis>123</emphasis><strong>123</strong></p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.Strong, "123", ParagraphStyles.Emphasis, "123", "<p><emphasis>123</emphasis><strong>123</strong><emphasis>123</emphasis></p>")]
        [InlineData(ParagraphStyles.Emphasis, "123", ParagraphStyles.Emphasis, "123", ParagraphStyles.Emphasis, "123", "<p><emphasis>123</emphasis><emphasis>123</emphasis><emphasis>123</emphasis></p>")]
        public void NestedSerialized(ParagraphStyles firstStyle, string firstInput, ParagraphStyles secondStyle, string secondInput, ParagraphStyles thirdStyle, string thirdInput, string expectedXml)
        {
            var r1 = new Run()
            {
                Text = firstInput,
                FontWeight = firstStyle == ParagraphStyles.Strong ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = firstStyle == ParagraphStyles.Emphasis ? FontStyles.Italic : FontStyles.Normal
            };
            var r2 = new Run()
            {
                Text = secondInput,
                FontWeight = secondStyle == ParagraphStyles.Strong ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = secondStyle == ParagraphStyles.Emphasis ? FontStyles.Italic : FontStyles.Normal
            };
            var r3 = new Run()
            {
                Text = thirdInput,
                FontWeight = thirdStyle == ParagraphStyles.Strong ? FontWeights.Bold : FontWeights.Normal,
                FontStyle = thirdStyle == ParagraphStyles.Emphasis ? FontStyles.Italic : FontStyles.Normal
            };
            var p = new Paragraph();
            p.Inlines.Add(r1);
            p.Inlines.Add(r2);
            p.Inlines.Add(r3);
            var sut = new ParagraphXmlSerializationDecorator(p);

            var sb = new StringBuilder();
            using (var writer = XmlWriter.Create(sb, Settings))
            {
                _serializer.Serialize(writer, sut, EmptyNameSpace);
            }

            Assert.Equal(expectedXml, sb.ToString());
        }
    }
}
