using System.Windows;
using System.Windows.Documents;

namespace WordLikeFb.Documents
{
    public class Title : Section
    {
        public Title(string text)
        {
            FontWeight = FontWeights.Bold;
            var p = new Paragraph();
            var run = new Run() { Text = text };
            p.Inlines.Add(run);
            Blocks.Add(p);
        }

        public Title() : this("***")
        {

        }
    }
}
