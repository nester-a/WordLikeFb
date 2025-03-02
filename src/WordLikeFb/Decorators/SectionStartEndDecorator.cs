using System.Windows.Documents;
using System.Windows.Markup;
using WordLikeFb.Documents;

namespace WordLikeFb.Decorators
{
    public class SectionStartEndDecorator : SectionDecorator<Section>, IAddChild
    {
        public SectionStartEndDecorator() : this(new()) { }
        public SectionStartEndDecorator(Section section)
            : this(section, $"<{section.GetType().Name.ToLower()}>", $"</{section.GetType().Name.ToLower()}>") { }

        public SectionStartEndDecorator(Section section, string sectionStartText, string sectionEndText) : base(section)
        {
            Blocks.Add(new StructureNode(sectionStartText));
            Blocks.Add(section);
            Blocks.Add(new StructureNode(sectionEndText));
        }

        public void AddChild(object value)
        {
            (DecorationTarget as IAddChild).AddChild(value);
        }
    }
}
