using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace FileSearchApp.ViewModel
{
    class RelayCommand : ICommand
    {
        public event EventHandler CanExecuteChanged;
        private Action _action;

        public RelayCommand(Action action)
        {
            _action = action;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            _action.Invoke();
        }
    }
}
