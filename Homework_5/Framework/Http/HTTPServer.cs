using HttpServer.Framework.Handlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;

namespace HttpServer.Framework.Http
{
    internal class HTTPServer
    {
        private HttpListener server = new HttpListener();
        private bool serverRuns = false;
        private Settings settings;
        private Handler chain;

        public HTTPServer()
        {
            ReadJson();
            BuildChain();
        }

        private void BuildChain()
        {
            Handler staticHandler = new StaticFileHandler();
            Handler controllerHandler = new ControllerHandler();
            Handler notFoundHandler = new NotFoundHandler();

            chain = staticHandler;
            staticHandler.Successor = controllerHandler;
            controllerHandler.Successor = notFoundHandler;
        }

        private void ReadJson()
        {
            string jsonStrings = File.ReadAllText("settings.json");
            settings = JsonSerializer.Deserialize<Settings>(jsonStrings);

            if (settings == null)
            {
                Console.WriteLine("не смогли считать settings.json");
                return;
            }

            string prefix = $"http://{settings.Host}:{settings.Port}/";

            server.Prefixes.Add(prefix);
        }

        public void Start()
        {
            serverRuns = true;

            server.Start();
        }

        public void Stop()
        {
            serverRuns = false;

            server.Stop();
        }

        public void manageServer()
        {
            Console.WriteLine("Для запуска сервера напишите 'start'");

            bool isStarted = false;

            while (!isStarted)
            {
                string str = Console.ReadLine();
                switch (str)
                {
                    case "start":
                        Start();
                        isStarted = true;
                        break;
                }
            }

            CycleRequest();

            Console.WriteLine("Сервер запущен");
            while (true)
            {
                Console.WriteLine("Для остановки сервера напишите 'stop'");

                string str = Console.ReadLine();
                switch (str)
                {
                    case "stop":
                        Stop();
                        return;
                }
            }
        }

        private async Task CycleRequest()
        {
            while (serverRuns)
            {
                HttpListenerContext context;
                try
                {
                    context = await server.GetContextAsync();
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }


                Console.WriteLine("Пришел запрос");

                await chain.HandleRequest(context);

                //Console.WriteLine($"Обработан запрос: {context.Request.Url} {context.Request.HttpMethod}");
            }
        }
    }
}