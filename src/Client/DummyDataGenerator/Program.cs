using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Bogus;
using DummyDataGenerator.Data;
using Newtonsoft.Json.Linq;
using ToDo.Api.Contract.ToDo;

namespace DummyDataGenerator
{
    //dotnet publish -c Release -r win-x64 --self-contained true -o ..\build\console
    //dotnet publish -c Release -r win-x64 /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true --self-contained true -o ..\build\console
    class Program
    {
        private static ToDoApiProxyFactory _apiProxyFactory;
        
        static async Task Main(string[] args)
        {
            await Authorize();
            await Run();
        }

        private static async Task Run()
        {
            var threads = Prompt<int>("# of threads to run: ");
            var listCount = Prompt<int>("# of lists to be created: ");
            var taskCountMin = Prompt<int>("# of tasks to be created in each list min: ");
            var taskCountMax = Prompt<int>("# of tasks to be created in each list man: ");

            var workers = new Task[threads];
            
            for (var i = 0; i < threads; i++)
            {
                workers[i] = Task.Run(() => CreateLists(listCount, taskCountMin, taskCountMax));
            }

            await Task.WhenAll(workers);
        }

        private static T Prompt<T>(string message, bool secret = false)
        {
            Console.Write(message);

            var result = string.Empty;
            
            if (secret)
            {
                ConsoleKey key;
                do
                {
                    var keyInfo = Console.ReadKey(intercept: true);
                    key = keyInfo.Key;

                    if (key == ConsoleKey.Backspace && result.Length > 0)
                    {
                        Console.Write("\b \b");
                        result = result[0..^1];
                    }
                    else if (!char.IsControl(keyInfo.KeyChar))
                    {
                        Console.Write("*");
                        result += keyInfo.KeyChar;
                    }
                } while (key != ConsoleKey.Enter);

                Console.WriteLine();
            }
            else
            {
                result = Console.ReadLine();
            }
            
            return (T) TypeDescriptor.GetConverter(typeof(T)).ConvertFrom(result);
        }

        private static async Task CreateLists(int listCount, int taskCountMin, int taskCountMax)
        {
            var toDoApi = _apiProxyFactory.CreateDynamicProxy<IToDo>();
            var faker = new Faker();
            
            for (var lc = 0; lc < listCount; lc++)
            {
                var taskCount = faker.Random.Int(taskCountMin, taskCountMax);
                var listName = faker.Commerce.Department();
                var list = await toDoApi.NewList(listName);

                for (var tk = 0; tk < taskCount; tk++)
                {
                    var taskDescription = faker.Lorem.Paragraph(faker.Random.Int(1, 3));
                    var task = await toDoApi.NewTask(list.Id, taskDescription);
                }
                
                Console.WriteLine($"Created list {lc}: {listName} with {taskCount} tasks.");
            }
        }

        private static async Task Authorize()
        {
            string content;
            
            if (File.Exists("token"))
            {
                content = await File.ReadAllTextAsync("token");
                goto found;
            }
            
            var username = Prompt<string>("Login: ");
            var password = Prompt<string>("Password: ", true);
            var payload = new Dictionary<string, string>
            {
                ["client_id"] = "todo-app",
                ["client_secret"] = "ea697a36-5838-2d39-e4e9-e9d5b1e68dc9",
                ["grant_type"] = "password",
                ["username"] = username,
                ["password"] = password
            };
            var request = new HttpRequestMessage(HttpMethod.Post, "/connect/token")
            {
                Content = new FormUrlEncodedContent(payload)
            };
            var client = new HttpClient
            {
                BaseAddress = new Uri("https://sso.starnet.md"),
            };
            var response = await client.SendAsync(request);
            content = await response.Content.ReadAsStringAsync();
            
            await File.WriteAllTextAsync("token", content);
            
            found:
            var result = JObject.Parse(content);

            var accessToken = result.GetValue("access_token")?.ToString();
            var accessTokenType = result.GetValue("token_type")?.ToString() ?? "Bearer";
            var apiClient = new HttpClient
            {
                BaseAddress = new Uri("https://localhost:14900"),
            };
            apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(accessTokenType, accessToken);

            _apiProxyFactory = new ToDoApiProxyFactory(apiClient);
        }
    }
}