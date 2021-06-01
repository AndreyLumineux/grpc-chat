using System;
using System.Windows.Input;

namespace ChatWPF.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _commandTask;
        private readonly Predicate<object> _canExecuteTask;

        public RelayCommand(Action<object> commandTask)
        {
            _commandTask = commandTask ?? throw new ArgumentNullException(nameof(commandTask));
            _canExecuteTask = x => true;
        }

        public RelayCommand(Action<object> commandTask, Predicate<object> canExecuteTask)
        {
            _commandTask = commandTask ?? throw new ArgumentNullException(nameof(commandTask));
            _canExecuteTask = canExecuteTask ?? throw new ArgumentNullException(nameof(canExecuteTask));
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object parameter)
        {
            return _canExecuteTask != null && _canExecuteTask(parameter);
        }

        public void Execute(object parameter)
        {
            _commandTask(parameter);
        }
    }
}
