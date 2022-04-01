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
            service.Newsavailable += ReportNewsDataAvailable;

            //Task<NewsApiData> t1 = NewsApiSampleData.GetNewsApiSampleAsync("sports");

            // Task<NewsApiData> t1 = service.GetNewsAsync();


           
            Console.WriteLine("-----------------");

            for (NewsCategory i = NewsCategory.business; i < NewsCategory.technology + 1; i++)
            {


                Task<News> t1 = service.GetNewsAsync(i);

                Task.WaitAll(t1);




                if (t1?.Status == TaskStatus.RanToCompletion)
                {
                    
                    News newsApi = t1.Result;
                    
                    Console.WriteLine($"Top headline for {i}");
                    Console.WriteLine();
                    foreach (var item in newsApi.Articles)
                    {
                        Console.WriteLine($" -  {item.DateTime.ToString("yyyy-MM-dd HH:mm")}: {item.Title} ");
                    }
                    Console.WriteLine();

                }
                else
                {
                    Console.WriteLine($"Geolocation news service error.");
                }

            }
            static void ReportNewsDataAvailable(object sender, string message)
            {
                Console.WriteLine($"Event message from News service: {message}");

                //skriver ut meddelenda om det är cached
            }
        }
    }       
    
}
