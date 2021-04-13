using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MaCompta.Tools
{
    class MenuService
    {
        private List<MenuItem> allMenuItems;

        public MenuService()
        {
            allMenuItems = new List<MenuItem>();
        }

        public List<MenuItem> GetParentMenuItems()
        {
            List<MenuItem> parentItems = allMenuItems.FindAll(x => x.Parent == null);

            return parentItems;
        }

        public void AddMenuItem(MenuItem item, string parentName = "")
        {
            if (parentName == string.Empty)
            {
                allMenuItems.Add(item);
            }
            else
            {
                MenuItem parent = allMenuItems.Find(x => x.Name == parentName);
                if (parent != null)
                {
                    //item.Parent = parent;
                    parent.Items.Add(item);
                }
                allMenuItems.Add(item);
            }
        }

        public void RemoveMenuItem(MenuItem menuItem)
        {
            foreach (MenuItem item in allMenuItems)
            {
                //item.RemoveSubMenu(menuItem);
                item.Items.Remove(menuItem);
            }

            if (allMenuItems.Contains(menuItem))
            {
                allMenuItems.Remove(menuItem);
            }
        }
    }
}
