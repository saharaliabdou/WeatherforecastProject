﻿using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Json; //Requires nuget package System.Net.Http.Json
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Json;

using Assignment_A1_03.Models;
using Assignment_A1_03.Services;

namespace Assignment_A1_03
{
    class Program
    {
        static void Main(string[] args)
        {
            //Register the event
            OpenWeatherService service = new OpenWeatherService();
            service.WeatherForecastAvailable += ReportWeatherDataAvailable;

            Task<Forecast> t1 = null, t2 = null, t3 = null, t4 = null;
            Exception exception = null;
            try
            {
                double latitude = 59.5086798659495;
                double longitude = 18.2654625932976;

                //Create the two tasks and wait for comletion
                t1 = service.GetForecastAsync(latitude, longitude);
                t2 = service.GetForecastAsync("Miami");
                
                Task.WaitAll(t1, t2);

                t3 = service.GetForecastAsync(latitude, longitude);
                t4 = service.GetForecastAsync("Miami");
                
                //Wait and confirm we get an event showing cahced data avaialable
                Task.WaitAll(t3, t4);
            }
            catch (Exception ex)
            {
                //if exception write the message later
                exception = ex;
            }

            Console.WriteLine("-----------------");
            if (t1?.Status == TaskStatus.RanToCompletion)
            {
                Forecast forecast = t1.Result;
                Console.WriteLine($"Weather forecast for {forecast.City}");
                var GroupedList = forecast.Items.GroupBy(item => item.DateTime.Date);
                foreach (var group in GroupedList)
                {
                    Console.WriteLine(group.Key.Date.ToShortDateString());
                    foreach (var item in group)
                    {
                        Console.WriteLine($"   - {item.DateTime.ToShortTimeString()}: {item.Description}, teperature: {item.Temperature} degC, wind: {item.WindSpeed} m/s");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Geolocation weather service error.");
                Console.WriteLine($"Error: {exception.Message}");
            }

            Console.WriteLine("-----------------");
            if (t2.Status == TaskStatus.RanToCompletion)
            {
                Forecast forecast = t2.Result;
                Console.WriteLine($"Weather forecast for {forecast.City}");
                var GroupedList = forecast.Items.GroupBy(item => item.DateTime.Date);
                foreach (var group in GroupedList)
                {
                    Console.WriteLine(group.Key.Date.ToShortDateString());
                    foreach (var item in group)
                    {
                        Console.WriteLine($"   - {item.DateTime.ToShortTimeString()}: {item.Description}, temperature: {item.Temperature} degC, wind: {item.WindSpeed} m/s");
                    }
                }
            }
            else
            {
                Console.WriteLine($"City weather service error");
                Console.WriteLine($"Error: {exception.Message}");
            }
        }
        static void ReportWeatherDataAvailable(object sender, string message)
        {
            Console.WriteLine($"Event message from weather service: {message}");
        }
    }
}
