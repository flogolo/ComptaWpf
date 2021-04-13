using MaCompta.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MaCompta.ViewModels
{
    public class FilterViewModel: ViewModelBase<FilterViewModel>
    {
        public event EventHandler FilterHasChanged;

        private void OnFilterhasChanged()
        {
            if (FilterHasChanged != null) FilterHasChanged(this, EventArgs.Empty);
        }

        public bool OnlyOneChoice { get; set; }

        public List<string> FilteredItems
        { get { return FilterItems.Where(f => f.IsChecked).Select(f => f.Item).ToList(); } }

        /// <summary>
        /// Liste des ordres utilisés pour le filtrage
        /// </summary>
        public ObservableCollection<CheckedListItem> FilterItems { get; private set; }

        public FilterViewModel()
        {
            FilterItems = new ObservableCollection<CheckedListItem>();
            _disableEvent = false;
        }

        public void InitFilterList(IEnumerable<string> filteredList)
        {
            FilterItems.Clear();
            foreach (string filterStr in filteredList.OrderBy(w => w))
            {
                FilterItems.Add(new CheckedListItem(this) {
                    Item = filterStr,
                    //IsChecked = true
                    IsChecked = false
                });
            }
        }

        public void AddFilter(string filterStr, bool isChecked)
        {
            FilterItems.Add(new CheckedListItem(this) { Item = filterStr, IsChecked = isChecked });
        }

        /// <summary>
        /// Tous les éléments ne sont pas sélectionnés (= filtre actif)
        /// </summary>
        public bool IsNotAllSelected { get {
                //return FilterItems.Any(i=>!i.IsChecked);
                return FilterItems.Any(i => i.IsChecked);
            }
        }

        /// <summary>
        /// Tous les éléments sont sélectionnés
        /// </summary>
        public bool IsAllSelected { get {
                return FilterItems.All(i => !i.IsChecked);
                //return FilterItems.All(i => i.IsChecked);
            } }
        /// <summary>
        /// Tous les éléments sont désélectionnés
        /// </summary>
        //public bool IsAllDeSelected { get { return FilterItems.All(i => !i.IsChecked); } }

        internal void ApplyFilter(CheckedListItem checkedItem)
        {
            if( OnlyOneChoice && checkedItem.IsChecked)
            {
                //désactivation des autres choix
                foreach (var item in FilterItems)
                {
                    if (!item.Equals(checkedItem))
                        item.ResetValue();
                }
            }
            if (!_disableEvent)
            {
                OnFilterhasChanged();
            }
            RaisePropertyChanged(vm => vm.IsNotAllSelected);
        }

        //permet de ne pas lancer l'opération de mise à jour si on coche/décoche tous/aucun
        private bool _disableEvent;

        [BaseCommand("FilterAllCommand")]
        public void ActionFilterAll()
        {
            _disableEvent = true;
            foreach (var item in FilterItems)
            {
//                item.IsChecked = true;
                item.IsChecked = false;
            }
            _disableEvent = false;
            OnFilterhasChanged();
        }

        [BaseCommand("FilterNoneCommand")]
        public void ActionFilterNone()
        {
            //_disableEvent = true;
            //foreach (var item in FilterItems)
            //{
            //    item.IsChecked = false;
            //}
            //_disableEvent = false;

            //OnFilterhasChanged();
        }

    }
}
