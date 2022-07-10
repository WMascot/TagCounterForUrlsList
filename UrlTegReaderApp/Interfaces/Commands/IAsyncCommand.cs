using System.Threading.Tasks;
using System.Windows.Input;

namespace UrlTegReaderApp.Interfaces.Commands
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object? parameter);
    }
}
