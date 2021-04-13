using System;
using System.Windows.Input;

namespace MaCompta.Commands
{
    public class BaseCommand: ICommand
    {

        private readonly Action _command;
        private readonly Func<bool> _canExecute;

        public BaseCommand(Action command)
            : this(command, null)
        { }

        public BaseCommand(Action command, Func<bool> canExecute)
        {
            if (command == null)
                throw new ArgumentNullException("command");
            _canExecute = canExecute;
            _command = command;
        }

        public void Execute(object parameter)
        {
            _command();
        }

        public bool CanExecute(object parameter)
        {
            if (_canExecute == null)
                return true;
            return _canExecute();
        }

        public event EventHandler CanExecuteChanged;
    }

    public class BaseCommandAttribute : Attribute
    {
        public BaseCommandAttribute(string commandName)
        {
            CommandName = commandName;
        }

        public string CommandName { get; private set; }
    }
}
