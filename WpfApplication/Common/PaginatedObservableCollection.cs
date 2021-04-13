using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace MaCompta.Common
{
    public class PaginatedObservableCollection<T>: ObservableCollection<T>
    {
        #region Properties
        public int PageSize
        {
            get { return _itemCountPerPage; }
            set
            {
                if (value >= 0)
                {
                    _itemCountPerPage = value;
                    RecalculateThePageItems();
                    OnPropertyChanged(new PropertyChangedEventArgs("PageSize"));
                }
            }
        }

        public int CurrentPage
        {
            get { return _currentPageIndex; }
            set
            {
                if (value >= 0)
                {
                    _currentPageIndex = value;
                    RecalculateThePageItems();
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrentPage"));
                }
            }
        }

        #endregion

        #region Constructor
        public PaginatedObservableCollection(IEnumerable<T> collection)
        {
            _currentPageIndex = 0;
            _itemCountPerPage = 10;
            _originalCollection = new List<T>(collection);
            RecalculateThePageItems();
        }

        public PaginatedObservableCollection(int itemsPerPage)
        {
            _currentPageIndex = 0;
            _itemCountPerPage = itemsPerPage;
            _originalCollection = new List<T>();
        }
        public PaginatedObservableCollection()
        {
            _currentPageIndex = 0;
            _itemCountPerPage = 10;
            _originalCollection = new List<T>();
        }
        #endregion

        #region private
        private void RecalculateThePageItems()
        {
            Clear();

            int startIndex = _currentPageIndex * _itemCountPerPage;

            for (int i = startIndex; i < startIndex + _itemCountPerPage; i++)
            {
                if (_originalCollection.Count > i)
                    base.InsertItem(i - startIndex, _originalCollection[i]);
            }
        }
        #endregion

        #region Overrides

        protected override void InsertItem(int index, T item)
        {
            int startIndex = _currentPageIndex * _itemCountPerPage;
            int endIndex = startIndex + _itemCountPerPage;

            //Check if the Index is with in the current Page then add to the collection as bellow. And add to the originalCollection also
            if ((index >= startIndex) && (index < endIndex))
            {
                base.InsertItem(index - startIndex, item);

                if (Count > _itemCountPerPage)
                    base.RemoveItem(endIndex);
            }

            if (index >= Count)
                _originalCollection.Add(item);
            else
                _originalCollection.Insert(index, item);
        }

        protected override void RemoveItem(int index)
        {
            int startIndex = _currentPageIndex * _itemCountPerPage;
            int endIndex = startIndex + _itemCountPerPage;
            //Check if the Index is with in the current Page range then remove from the collection as bellow. And remove from the originalCollection also
            if ((index >= startIndex) && (index < endIndex))
            {
                RemoveAt(index - startIndex);

                if (Count <= _itemCountPerPage)
                    base.InsertItem(endIndex - 1, _originalCollection[index + 1]);
            }

            _originalCollection.RemoveAt(index);
        }

        #endregion

        private readonly List<T> _originalCollection;
        private int _currentPageIndex;
        private int _itemCountPerPage;
    }
}
