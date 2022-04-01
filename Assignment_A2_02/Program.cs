using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Assignment_A2_02.Models;
using Assignment_A2_02.Services;

namespace Assignment_A2_02
{
    class Program
    {
        static void Main(string[] args)
        {
            NewsService service = new NewsService();

            //Task<NewsApiData> t1 = NewsApiSampleData.GetNewsApiSampleAsync("sports");

           // Task<NewsApiData> t1 = service.GetNewsAsync();
            Task<News> t1 = service.GetNewsAsync(NewsCategory.business);

            Task.WaitAll(t1);

            Console.WriteLine("-----------------");
            News newsApi = t1.Result;
            if (t1?.Status == TaskStatus.RanToCompletion)
            {
               
                Console.WriteLine("Top headlines");
                foreach (var item in newsApi.Articles)
                {
                    Console.WriteLine($" -  {item.DateTime.ToString("yyyy-MM-dd HH:mm")}: {item.Title} ");
                }


            }
            else
            {
                Console.WriteLine($"Geolocation news service error.");
            }

        }
    }       
    
}
