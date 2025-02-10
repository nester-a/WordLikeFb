using System.Windows.Documents;

namespace WordLikeFb.Decorators
{
    public interface IBlockDecorator<T> where T : Block
    {
        T DecorationTarget { get; }
    }
}
