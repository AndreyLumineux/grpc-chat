using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ChatWPF.Commands
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> commandTask;
        private readonly Predicate<object> canExecuteTask;

        public RelayCommand(Action<object> commandTask)
        {
            this.commandTask = commandTask ?? throw new ArgumentNullException(nameof(commandTask));
            this.canExecuteTask = (object x) => true;
        }

        public RelayCommand(Action<object> commandTask, Predicate<object> canExecuteTask)
        {
            this.commandTask = commandTask ?? throw new ArgumentNullException(nameof(commandTask));
            this.canExecuteTask = canExecuteTask ?? throw new ArgumentNullException(nameof(canExecuteTask));
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return canExecuteTask != null && canExecuteTask(parameter);
        }

        public void Execute(object parameter)
        {
            commandTask(parameter);
        }
    }
}
