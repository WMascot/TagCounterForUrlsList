using System.Windows.Input;
using UrlTegReaderApp.Models;
using UrlTegReaderApp.ViewModels.Base;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UrlTegReaderApp.Interfaces.Commands;
using UrlTegReaderApp.Commands;
using Microsoft.Win32;
using UrlTegReaderApp.Services;

namespace UrlTegReaderApp.ViewModels
{
    public class TagReaderViewModel : ViewModel
    {
        public string FileName { get; private set; }
        public bool IsFileChosen { get; private set; }

        public ICommand ChooseFileCommand { get; private set; }
        public AsyncCommand<ObservableCollection<UrlListItem>> StartReadingCommand { get; private set; } 

        public TagReaderViewModel()
        {
            IsFileChosen = false;
            FileName = "";
            StartReadingCommand = new AsyncCommand<ObservableCollection<UrlListItem>>((_, token) =>
                TagCounterService.TagsCounterForFile(FileName, token)
            );
            ChooseFileCommand = new Command(ChooseFile, _ => StartReadingCommand.CanExecute(_) == true);
        }
        private void ChooseFile()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                FileName = fileDialog.FileName;
                IsFileChosen = true;
                OnPropertyChange(nameof(FileName));
                OnPropertyChange(nameof(IsFileChosen));
            }
        }
    }
}
