using System.Windows.Documents;
using WordLikeFb.Documents;

namespace WordLikeFb.Factories
{
    internal static class WindowDocumentElementsFactory
    {
        public static Section CreateSection()
        {
            var sect = new Section();

            var title = new Title();
            sect.Blocks.Add(title);

            return sect;
        }
    }
}
