using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace MaCompta.Common
{
    public static class ParentFinder
    {
        /// <summary>
        /// Methode  permettant de récupérer un UIElement parent du type souhaité
        /// retourn Null si le parent n'existe pas.
        /// </summary>
        /// <typeparam name="T">Le type du parent à rechercher</typeparam>
        /// <param name="control">L'élément racine de la recherche</param>
        /// <returns>L'élément parent ou null si aucun</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter")]
        public static T FindParent<T>(this DependencyObject control) where T : UIElement
        {
            var p = VisualTreeHelper.GetParent(control) as UIElement;
            if (p != null)
            {
                if (p is T)
                    return (T)p;
                return FindParent<T>(p);
            }
            return null;
        }
        /// <summary> 
        /// Walk up the VisualTree, returning first parent object of the type supplied as type parameter 
        /// </summary> 
        public static T FindAncestor<T>(this DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                var o = obj as T;
                if (o != null)
                    return o;

                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        } 
    }

    public static class ChildFinder
    {
        public static DependencyObject FindFirstControlInChildren(DependencyObject obj, string controlType)
        {
            if (obj == null)
                return null;

            // Get a list of all occurrences of a particular type of control (eg "CheckBox") 
            IEnumerable<DependencyObject> ctrls = FindInVisualTreeDown(obj, controlType);
            if (!ctrls.Any())
                return null;

            return ctrls.First();
        }

        public static IEnumerable<DependencyObject> FindInVisualTreeDown(DependencyObject obj, string type)
        {
            if (obj != null)
            {
                if (obj.GetType().ToString().EndsWith(type))
                {
                    yield return obj;
                }

                for (var i = 0; i < VisualTreeHelper.GetChildrenCount(obj); i++)
                {
                    foreach (var child in FindInVisualTreeDown(VisualTreeHelper.GetChild(obj, i), type))
                    {
                        if (child != null)
                        {
                            yield return child;
                        }
                    }
                }
            }
        }

        public static IEnumerable<T> FindVisualChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindVisualChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

    }


}
