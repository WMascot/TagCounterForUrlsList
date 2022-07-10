using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlTegReaderApp.Commands.Base;

namespace UrlTegReaderApp.Commands
{
    public class Command : CommandBase
    {
        private Action _command;
        private Func<object, bool> _canExecute;
        public Command(Action command, Func<object, bool> canExecute)
        {
            _command = command;
            _canExecute = canExecute;
        }
        public override bool CanExecute(object? parameter)
        {
            return _canExecute(parameter);
        }

        public override void Execute(object? parameter)
        {
            _command();
            RaiseCanExecuteChanged();
        }
    }
}
