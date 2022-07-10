using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UrlTegReaderApp.Models;

namespace UrlTegReaderApp.Services
{
    public static class TagCounterService
    {
        public static async Task<ObservableCollection<UrlListItem>> TagsCounterForFile(string filename, CancellationToken token = new CancellationToken())
        {
            ObservableCollection<UrlListItem> urlsCollection = new ObservableCollection<UrlListItem>();
            UrlListItem MaxTagsUrl = new UrlListItem()
            {
                Url = "",
                TagsCount = 0
            };
            HtmlWeb web = new HtmlWeb();
            string[] urls = await File.ReadAllLinesAsync(filename, token).ConfigureAwait(false);
            foreach (string url in urls)
            {
                HtmlDocument doc = await web.LoadFromWebAsync(url, token).ConfigureAwait(false);
                int tagsCount = doc.DocumentNode.Descendants("a").Count();
                UrlListItem newUrlListItem = new()
                {
                    Url = url,
                    TagsCount = tagsCount
                };
                if (tagsCount > MaxTagsUrl.TagsCount) urlsCollection.Insert(0, newUrlListItem);
                else urlsCollection.Add(newUrlListItem);
            }
            return urlsCollection;
        }
    }
}
