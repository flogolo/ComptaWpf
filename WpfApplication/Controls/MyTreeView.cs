using System.Windows;
using System.Windows.Controls;

namespace MaCompta.Controls
{
    class MyTreeView : TreeView
    {
        public MyTreeView()
        {
            SelectedItemChanged += ItemChanged;
        }

        void ItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if (SelectedItem != null)
            {
                SetValue(SelectedModelProperty, SelectedItem);
            }
        }

        public object SelectedModel
        {
            get { return GetValue(SelectedModelProperty); }
            set { SetValue(SelectedModelProperty, value); }
        }

        public static readonly DependencyProperty SelectedModelProperty =
            DependencyProperty.Register("SelectedModel", typeof(object), typeof(MyTreeView), new UIPropertyMetadata(null));

    }
}
