using System.Windows.Documents;
using System.Windows.Markup;
using WordLikeFb.Documents;

namespace WordLikeFb.Decorators
{
    public class SectionStartEndDecorator : Section, IAddChild, IBlockDecorator<Section>
    {
        public SectionStartEndDecorator() : this(new()) { }
        public SectionStartEndDecorator(Section section)
            : this(section, "Начало секции", "Конец секции") { }

        public SectionStartEndDecorator(Section section, string sectionStartText, string sectionEndText)
        {
            DecorationTarget = section;
            Blocks.Add(new ImmutableParagrath(sectionStartText));
            Blocks.Add(section);
            Blocks.Add(new ImmutableParagrath(sectionEndText));
        }

        public Section DecorationTarget {  get; }

        public void AddChild(object value)
        {
            (DecorationTarget as IAddChild).AddChild(value);
        }
    }
}
