using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace My.Labs.Translator.ViewModels
{
    public class RelayCommand : ICommand
    {

        private Action<object> action;

        public RelayCommand(Action<object> a)
        {
            this.action = a;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;
        

        public void Execute(object parameter)
        {
            if (action != null)
                action(parameter);
        }
    }
}
