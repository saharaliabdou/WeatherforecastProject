#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Concurrent;
using System.Threading.Tasks;
using System.Collections.Generic;
using Assignment_A2_03.Models;
using Assignment_A2_03.ModelsSampleData;
namespace Assignment_A2_03.Services
{
    public class NewsService
    {
        ConcurrentDictionary<(string, NewsCategory), News> _Cache = new ConcurrentDictionary<(string, NewsCategory), News>();
       
        public EventHandler<string> Newsavailable;
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "3cf18a22fddb446a81eceb8de3065438";
        public async Task<News> GetNewsAsync(NewsCategory category)
        {
            News news = null;


            string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
            NewsCategory cat = category;
            var key = (date, cat);

            if (!_Cache.TryGetValue(key, out news))
            {
                var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";
                news = await ReadNewsApiAsync(uri);

                _Cache[key] = news;
                OnNewsCategoryAvailable($"News in category is available for : { category}");
            }
            else
                OnNewsCategoryAvailable($"Cached news in category is available for :{category}");

            return news;
        }

        private async Task<News> ReadNewsApiAsync(string uri)
        {

            //uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}"; //soupossed to be cat

            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();

            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();


            News news = new News();

            news.Articles = new List<NewsItem>();

            nd.Articles.ForEach(a => { news.Articles.Add(GetNewsItem(a)); });

            return news;

        }

        protected virtual void OnNewsCategoryAvailable(string c)

        {
            Newsavailable?.Invoke(this, c);
        }

        private NewsItem GetNewsItem(Article wdListItem)
        {
            NewsItem newsitem = new NewsItem();

            newsitem.DateTime = wdListItem.PublishedAt;


            newsitem.Title = wdListItem.Title;


            return newsitem;


        }
    }
}
