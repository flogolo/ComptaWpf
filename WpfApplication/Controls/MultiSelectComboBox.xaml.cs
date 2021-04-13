using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace MaCompta.Controls
{
    /// <summary>
    /// Interaction logic for MultiSelectComboBox.xaml
    /// </summary>
    public partial class MultiSelectComboBox
    {
        public const String AllItems = "Tous";
        public const String NoneItems = "Aucun";

        private readonly ObservableCollection<Node> _nodeList;
        public MultiSelectComboBox()
        {
            InitializeComponent();
           _nodeList = new ObservableCollection<Node>();
        }

        #region Dependency Properties

       public static readonly DependencyProperty ItemsSourceProperty =
             DependencyProperty.Register("ItemsSource", typeof(ObservableCollection<IViewModel>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
        OnItemsSourceChanged));

        public static readonly DependencyProperty SelectedItemsProperty =
         DependencyProperty.Register("SelectedItems", typeof(ObservableCollection<IViewModel>), typeof(MultiSelectComboBox), new FrameworkPropertyMetadata(null,
     OnSelectedItemsChanged));

        public static readonly DependencyProperty TextProperty =
           DependencyProperty.Register("Text", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public static readonly DependencyProperty DefaultTextProperty =
            DependencyProperty.Register("DefaultText", typeof(string), typeof(MultiSelectComboBox), new UIPropertyMetadata(string.Empty));

        public Collection<IViewModel> ItemsSource
        {
            get { return (Collection<IViewModel>)GetValue(ItemsSourceProperty); }
            set
            {
                SetValue(ItemsSourceProperty, value);
            }
        }

        public Collection<IViewModel> SelectedItems
        {
            get { return (Collection<IViewModel>)GetValue(SelectedItemsProperty); }
            set
            {
                SetValue(SelectedItemsProperty, value);
            }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public string DefaultText
        {
            get { return (string)GetValue(DefaultTextProperty); }
            set { SetValue(DefaultTextProperty, value); }
        }
        #endregion

        #region Events
        private static void OnItemsSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSelectComboBox)d;
            control.DisplayInControl();
            (e.NewValue as ObservableCollection<IViewModel>).CollectionChanged += control.MultiSelectComboBox_CollectionChanged;
            //control.ItemsSource = (Collection<IViewModel>)e.NewValue;
        }

        private void MultiSelectComboBox_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var item in e.NewItems)
                    {
                        var node = new Node((item as IViewModel).Libelle);
                        _nodeList.Add(node);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var item in e.OldItems)
                    {
                        var node = _nodeList.FirstOrDefault(n => n.Libelle == ((IViewModel) item).Libelle);
                        if(node!=null)
                            _nodeList.Remove(node);
                        //Comptes.Remove((CompteViewModel)item);
                    }
                    break;
                case NotifyCollectionChangedAction.Replace:
                    break;
                case NotifyCollectionChangedAction.Reset:
                    _nodeList.Clear();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private static void OnSelectedItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = (MultiSelectComboBox)d;
            control.SelectNodes();
            control.SetText();
        }

        private void CheckBoxClick(object sender, RoutedEventArgs e)
        {
            var clickedBox = (CheckBox)sender;

            //tous
            if (clickedBox.Content.ToString() == AllItems)
            {
                foreach (Node node in _nodeList)
                {
                    node.IsSelected = true;
                }
            }
            else if (clickedBox.Content.ToString() == NoneItems)
            {
                foreach (Node node in _nodeList)
                {
                    node.IsSelected = false;
                }
            }
            else
            {
                int selectedCount = _nodeList.Count(s => s.IsSelected && s.Libelle != AllItems);
                if (selectedCount == ItemsSource.Count - 1)
                    ItemsSource.FirstOrDefault(i => i.Libelle == AllItems).IsSelected = true;
                else
                    ItemsSource.FirstOrDefault(i => i.Libelle == AllItems).IsSelected = false;
            }
            SetSelectedItems();
            SetText();

        }
        #endregion


        #region Methods
        private void SelectNodes()
        {
            foreach (IViewModel item in SelectedItems)
            {
                Node node = _nodeList.FirstOrDefault(i => i.Libelle == item.Libelle);
                if (node != null)
                    node.IsSelected = true;
            }
        }

        private void SetSelectedItems()
        {
            //if (SelectedItems == null)
            //    SelectedItems = new Dictionary<string, object>();
            SelectedItems.Clear();
            foreach (Node node in _nodeList)
            {
                if (node.IsSelected && node.Libelle != AllItems && node.Libelle != NoneItems)
                {
                    if (ItemsSource.Count > 0)
                    {
                        var item = ItemsSource.First(c => c.Libelle == node.Libelle);
                        SelectedItems.Add(item);
                    }
                }
            }
        }
        private void DisplayInControl()
        {
            _nodeList.Clear();
            //if (this.ItemsSource.Count > 0)
              //  _nodeList.Add(new Node(ALL_ITEMS));
            foreach (IViewModel item in ItemsSource)
            {
                var node = new Node(item.Libelle);
                _nodeList.Add(node);
            }
            MultiSelectCombo.ItemsSource = _nodeList;
        }

        private void SetText()
        {
            if (SelectedItems != null)
            {
                var displayText = new StringBuilder();
                foreach (Node s in _nodeList)
                {
                    if (s.IsSelected && s.Libelle == AllItems)
                    {
                        displayText = new StringBuilder();
                        displayText.Append(AllItems);
                        break;
                    }
                    if (s.IsSelected && s.Libelle != AllItems)
                    {
                        displayText.Append(s.Libelle);
                        displayText.Append(',');
                    }
                }
                Text = displayText.ToString().TrimEnd(new[] { ',' }); 
            }           
            // set DefaultText if nothing else selected
            if (string.IsNullOrEmpty(Text))
            {
                Text = DefaultText;
            }
        }

       
        #endregion
    }

    public class Node : INotifyPropertyChanged, IViewModel
    {

        private string _name;
        private bool _isSelected;
        #region ctor
        public Node(string name)
        {
            Libelle = name;
        }
        #endregion

        #region Properties

        public string Libelle
        {
            get { return _name; }
            set
            {
                _name = value;
                NotifyPropertyChanged("Name");
            }
        }

        public bool IsSelected
        {
            get
            {
                return _isSelected;
            }
            set
            {
                _isSelected = value;
                NotifyPropertyChanged("IsSelected");
            }
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

    }
}
