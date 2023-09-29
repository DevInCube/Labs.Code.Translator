using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace My.Labs.Translator.Commands
{
    public class SimpleCommand : ICommand
    {

        private Action action;

        public SimpleCommand(Action a)
        {
            action = a;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public event EventHandler CanExecuteChanged;


        public void Execute(object parameter)
        {
            if (action != null)
                action();
        }
    }
}
