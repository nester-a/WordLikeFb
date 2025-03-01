using System.Windows.Documents;
using System.Windows.Markup;
using WordLikeFb.Documents;

namespace WordLikeFb.Decorators
{
    public class SectionStartEndDecorator : Section, IAddChild, IBlockDecorator<Section>
    {
        public SectionStartEndDecorator() : this(new()) { }
        public SectionStartEndDecorator(Section section)
            : this(section, $"<{section.GetType().Name.ToLower()}>", $"</{section.GetType().Name.ToLower()}>") { }

        public SectionStartEndDecorator(Section section, string sectionStartText, string sectionEndText)
        {
            DecorationTarget = section;
            Blocks.Add(new StructureNode(sectionStartText));
            Blocks.Add(section);
            Blocks.Add(new StructureNode(sectionEndText));
        }

        public Section DecorationTarget {  get; }

        public void AddChild(object value)
        {
            (DecorationTarget as IAddChild).AddChild(value);
        }
    }
}
