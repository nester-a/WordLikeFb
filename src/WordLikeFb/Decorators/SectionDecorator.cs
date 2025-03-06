using System.Windows.Documents;

namespace WordLikeFb.Decorators
{
    public abstract class SectionDecorator<T> : Section where T : Section
    {
        public T DecorationTarget { get; }
        public SectionDecorator(T target) : base(target)
        {
            DecorationTarget = target;
            Blocks.Add(target);
        }
    }
}
