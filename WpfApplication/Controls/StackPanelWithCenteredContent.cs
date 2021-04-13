using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MaCompta.Controls
{
    class StackPanelWithCenteredContent:StackPanel
    {
        static StackPanelWithCenteredContent()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(StackPanelWithCenteredContent),
                new FrameworkPropertyMetadata(typeof(StackPanelWithCenteredContent)));
        }

        protected override void OnVisualChildrenChanged(DependencyObject visualAdded, DependencyObject visualRemoved)
        {
            CenterChildren();
            base.OnVisualChildrenChanged(visualAdded, visualRemoved);
        }

        private void CenterChildren()
        {
            //if (Children.Any())
            {
                var nbchilds = Children.OfType<Button>().Count();
                //10px entre chaque
                var taille = Children.OfType<Button>().Sum(b => b.ActualWidth) + (nbchilds - 1) * 10;
                var posleft = (ActualWidth - taille) / 2;
                var first = Children.OfType<Button>().FirstOrDefault();
                if (first != null)
                {
                    first.Height = 26;
                    first.Margin = new Thickness(100, 0, 0, 0);
                }
                foreach(var child in Children.OfType<Button>())
                {
                    if(!child.Equals(first))
                    {
                        child.Height = 26;
                        child.Margin = new Thickness(10, 0, 0, 0);
                    }
                }
            }
        }
    }
}
