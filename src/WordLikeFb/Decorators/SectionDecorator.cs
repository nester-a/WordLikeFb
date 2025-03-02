using System.Windows.Documents;

namespace WordLikeFb.Decorators
{
    public abstract class SectionDecorator<T> : Section where T : Section, new()
    {
        public T DecorationTarget { get; init; }
        public SectionDecorator(T target)
        {
            DecorationTarget = target;
        }
        public SectionDecorator() : this(new T())
        {
            
        }
    }
}
