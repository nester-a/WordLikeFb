using System.Windows.Documents;
using WordLikeFb.Documents;

namespace WordLikeFb.Decorators
{
    public class SectionStartEndDecorator : SectionDecorator<Section>
    {
        public SectionStartEndDecorator(Section section)
            : this(section, $"<{section.GetType().Name.ToLower()}>", $"</{section.GetType().Name.ToLower()}>") { }

        public SectionStartEndDecorator(Section section, string sectionStartText, string sectionEndText) : base(section)
        {
            Blocks.InsertBefore(DecorationTarget, new StructureNode(sectionStartText));
            Blocks.InsertAfter(DecorationTarget, new StructureNode(sectionEndText));
        }
    }
}
