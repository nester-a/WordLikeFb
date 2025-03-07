using System.Windows.Documents;
using WordLikeFb.Decorators;

namespace WordLikeFb.Decoration
{
    public class SectionStartEndDecoratorWrapper
    {
        public void Wrap(BlockCollection blocks)
        {
            if (blocks.Count == 0)
            {
                return;
            }
            var current = blocks.FirstBlock;
            do
            {
                var next = current?.NextBlock;

                if (current is not Section subSection)
                {
                    current = next;
                    continue;
                }

                blocks.Remove(subSection);

                var decorated = new SectionStartEndDecorator(subSection);

                Wrap(subSection.Blocks);

                if (next is not null)
                    blocks.InsertBefore(next, decorated);
                else
                    blocks.Add(decorated);

                current = next;
            } while (current is not null);
        }

        public void Unwrap(BlockCollection blocks)
        {
            if (blocks.Count == 0)
            {
                return;
            }
            var current = blocks.FirstBlock;
            do
            {
                var next = current?.NextBlock;

                if (current is not SectionStartEndDecorator decoreted)
                {
                    current = next;
                    continue;
                }

                blocks.Remove(decoreted);

                var section = decoreted.InjectTarget();

                Unwrap(section.Blocks);

                if (next is not null)
                    blocks.InsertBefore(next, section);
                else
                    blocks.Add(section);

                current = next;
            } while (current is not null);
        }
    }
}
