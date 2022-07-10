using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using UrlTegReaderApp.Commands.Base;
using UrlTegReaderApp.Extensions;

namespace UrlTegReaderApp.Commands
{
    public class AsyncCommand<TResult> : AsyncCommandBase, INotifyPropertyChanged
    {
        private readonly Func<object?, CancellationToken, Task<TResult>> _command;
        private readonly Func<object?, bool>? _canExecute;

        private NotifyTaskCompletion<TResult>? _execution;
        public NotifyTaskCompletion<TResult>? Execution 
        { 
            get => _execution;
            private set
            {
                if (ReferenceEquals(_execution, value)) return;
                _execution = value;
                OnPropertyChanged();
            }
        }

        private CancelAsyncCommand _сancelCommand;
        
        public AsyncCommand(Func<object?, CancellationToken, Task<TResult>> command, Func<object?, bool>? canExecute = null)
        {
            _command = command;
            _canExecute = canExecute;
            _сancelCommand = new CancelAsyncCommand();
        }
        public override bool CanExecute(object? parameter)
        {
            return Execution == null || Execution.IsCompleted;
        }
        public override async Task ExecuteAsync(object? parameter)
        {
            _сancelCommand.NotifyCommandStarting();
            Execution = new NotifyTaskCompletion<TResult>(_command(parameter, _сancelCommand.Token));
            RaiseCanExecuteChanged();
            await Execution.TaskCompletion;
            _сancelCommand.NotifyCommandFinished();
            RaiseCanExecuteChanged();
        }
        public ICommand CancelCommand { get => _сancelCommand; }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private sealed class CancelAsyncCommand : ICommand
        {
            private CancellationTokenSource _cts = new CancellationTokenSource();
            private bool _commandExecuting;
            public CancellationToken Token => _cts.Token;
            public void NotifyCommandStarting()
            {
                _commandExecuting = true;
                if (!_cts.IsCancellationRequested)
                    return;
                _cts = new CancellationTokenSource();
                RaiseCanExecuteChanged();
            }
            public void NotifyCommandFinished()
            {
                _commandExecuting = false;
                RaiseCanExecuteChanged();
            }
            public event EventHandler? CanExecuteChanged
            {
                add { CommandManager.RequerySuggested += value; }
                remove { CommandManager.RequerySuggested -= value; }
            }
            bool ICommand.CanExecute(object? parameter)
            {
                return _commandExecuting && !_cts.IsCancellationRequested;
            }
            void ICommand.Execute(object? parameter)
            {
                _cts.Cancel();
                RaiseCanExecuteChanged();
            }
        }
    }
}
