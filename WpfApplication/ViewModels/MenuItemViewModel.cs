using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MaCompta.ViewModels
{
    public class MenuItemViewModel
    {
        private readonly ICommand _command;

        public MenuItemViewModel(ICommand command)
        {
            _command = command;
                //new CommandViewModel(action);
            MenuItems = new ObservableCollection<MenuItemViewModel>();
        }

        public string Header { get; set; }

        public long Id { get; set; }

        public ObservableCollection<MenuItemViewModel> MenuItems { get; set; }

        public ICommand Command
        {
            get
            {
                return _command;
            }
        }

        private void Execute()
        {
            // (NOTE: In a view model, you normally should not use MessageBox.Show()).
            //MessageBox.Show("Clicked at " + Header);
            System.Diagnostics.Debug.WriteLine("Clicked at " + Header);
        }
    }
}
