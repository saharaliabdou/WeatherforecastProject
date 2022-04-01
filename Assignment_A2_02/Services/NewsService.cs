#define UseNewsApiSample  // Remove or undefine to use your own code to read live data

using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Assignment_A2_02.Models;
using Assignment_A2_02.ModelsSampleData;
namespace Assignment_A2_02.Services
{
    public class NewsService
    {
        public EventHandler<string> Newsavailable;
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "3cf18a22fddb446a81eceb8de3065438";
        public async Task<News> GetNewsAsync(NewsCategory category)
        {
#if UseNewsApiSample
            //NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);

#else
            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";

           // Your code here to get live data
#endif
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category={category}&apiKey={apiKey}";
            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();  //
            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();
            //Your Code to convert WeatherApiData to Forecast using Linq.
            // NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync(category);

            News news = new News()
            {
                Category = category
            };

            


            

            //OnNewsCategoryAvailable($"New in catogory {category}");
            news.Articles = new List<NewsItem>();
            //return news;
            //nd.Articles.ForEach(a => news.Articles.Add());
            nd.Articles.ForEach(wdListItem => { news.Articles.Add(GetNewsItem(wdListItem)); });
           



                OnNewsCategoryAvailable($"News in category is available for : {news.Category} ");

            
          

            

            
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
           
           
            newsitem.Title= wdListItem.Title;
           

            return newsitem;

            
        }

       

    }
}
