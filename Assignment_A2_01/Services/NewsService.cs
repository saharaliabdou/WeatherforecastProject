#define UseNewsApiSample  // Remove or undefine to use your own code to read live data
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Threading.Tasks;

using Assignment_A2_01.Models;
using Assignment_A2_01.ModelsSampleData;
namespace Assignment_A2_01.Services
{
    public class NewsService
    {
        HttpClient httpClient = new HttpClient();
        readonly string apiKey = "7616850dd80ab0c5df57eb53e6668a2d";// "d318329c40734776a014f9d9513e14ae";'

        public async Task<NewsApiData> GetNewsAsync()
        {

#if UseNewsApiSample      
            //NewsApiData nd = await NewsApiSampleData.GetNewsApiSampleAsync("sports");

#else
            
            //Your code here to read live data
           
           
#endif
            //https://newsapi.org/docs/endpoints/top-headlines
            var uri = $"https://newsapi.org/v2/top-headlines?country=se&category=sports&apiKey={apiKey}";

            HttpResponseMessage response = await httpClient.GetAsync(uri);
            response.EnsureSuccessStatusCode();
            //Article ar  = await response.Content.ReadFromJsonAsync<Article>();
            NewsApiData nd = await response.Content.ReadFromJsonAsync<NewsApiData>();
            //Your Code to convert WeatherApiData to Forecast using Linq.
            NewsApiData newsapidata = new NewsApiData();
            
            newsapidata.Articles = new List<Article>();


            //newsapidata.Articles = ar.Author.Tolist;
            
            

            nd.Articles.ForEach(wdListItem => {newsapidata.Articles.Add(GetNewsArticle(wdListItem)); });

            return newsapidata;
        }

        private Article GetNewsArticle(Article wdListItem)
        {

            Article article = new Article();
            //item.DateTime = UnixTimeStampToDateTime(wdListItem.dt);
            //article.PublishedAt = UnixTimeStampToDateTime(wdListItem.dt);
            article.Title = wdListItem.Title;
            ////item.Temperature = wdListItem.main.temp;
            ////item.Description = wdListItem.weather.Count > 0 ? wdListItem.weather.First().description : "No info";
            //item.WindSpeed = wdListItem.wind.speed;
         
            return article;
        }

    }
}
