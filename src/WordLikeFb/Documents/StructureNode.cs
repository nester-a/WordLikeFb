using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WordLikeFb.Documents
{
    internal class StructureNode : BlockUIContainer
    {
        public StructureNode(string text)
        {
            var r = new Run() 
            { 
                Text = text, 
                IsEnabled = false, 
                Foreground = new SolidColorBrush(Colors.LightGray),
                BaselineAlignment = System.Windows.BaselineAlignment.Center
            };
            
            Child = new TextBlock(r);
            Focusable = false;
            PreviewMouseDown += ImmutableText_PreviewMouseDown;            
        }

        private void ImmutableText_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
