using CommunityToolkit.Mvvm.ComponentModel;

namespace MaCompta.ViewModels
{
    //<CheckedListItem<T>>
    public class CheckedListItem : ObservableObject
    {
        private bool _isChecked;
        private string _item;
        private FilterViewModel filterViewModel;

        public CheckedListItem()
        { }

        public CheckedListItem(string item, bool isChecked = false)
        {
            _item = item;
            _isChecked = isChecked;
        }

        public CheckedListItem(FilterViewModel filterViewModel)
        {
            this.filterViewModel = filterViewModel;
        }

        public string Item
        {
            get { return _item; }
            set
            {
                _item = value;
                OnPropertyChanged();
            }
        }


        public bool IsChecked
        {
            get { return _isChecked; } 
            set
            {
                _isChecked = value;
                OnPropertyChanged();
                filterViewModel.ApplyFilter(this);
            }
        }

        public void ResetValue()
        {
            _isChecked = false;
            OnPropertyChanged(nameof(IsChecked)) ;
        }
    }
}