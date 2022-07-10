using System.ComponentModel;
using System.Runtime.CompilerServices;
using UrlTegReaderApp.ViewModels.Base;

namespace UrlTegReaderApp.Models
{
    public class UrlListItem : ViewModel
    {
        private string _url;
        private int _tagsCount;
        public string Url { get { return _url; } 
            set { 
                _url = value;
                OnPropertyChange(nameof(Url));
            } 
        }
        public int TagsCount
        {
            get { return _tagsCount; }
            set
            {
                _tagsCount = value;
                OnPropertyChange(nameof(TagsCount));
            }
        }
    }
}
