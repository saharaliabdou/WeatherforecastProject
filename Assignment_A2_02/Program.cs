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

            Task<NewsApiData> t1 = service.GetNewsAsync();

            Task.WaitAll(t1);

            Console.WriteLine("-----------------");
            if (t1?.Status == TaskStatus.RanToCompletion)
            {
                NewsApiData newsApi = t1.Result;
                Console.WriteLine("Top headlines");
                foreach (var item in newsApi.Articles)
                {
                    Console.WriteLine($"{item.Title} {newsApi.Status}");
                }


            }
            else
            {
                Console.WriteLine($"Geolocation news service error.");
            }

        }
    }       
    }
}
