using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace CommonLibrary.Tools
{
    /// <file>SortableObservableCollection.cs</file>
    /// <created>06/01/2011</created>
    /// <author>FSC</author>
    /// <rversion>1.0</rversion>
    /// <summary>
    /// Méthodes de tri pour une observable collection
    /// </summary>
    public class SortableObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// This will use Comparer.Default    
        /// </summary>
        public void Sort()
        {
            Sort(0, Count, null);
        }

        /// <summary>
        /// Pass custom comparer to Sort method
        /// </summary>
        /// <param name="comparer">comparateur</param>
        public void Sort(IComparer<T> comparer)
        {
            Sort(0, Count, comparer);
        }

        /// <summary>
        /// Use this method to sort part of a collection  
        /// </summary>
        /// <param name="index">index</param>
        /// <param name="count">nombre d 'éléments</param>
        /// <param name="comparer">comparateur</param>
        public void Sort(int index, int count, IComparer<T> comparer)
        {
            ((List<T>) Items).Sort(index, count, comparer);
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}