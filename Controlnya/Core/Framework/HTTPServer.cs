using controlnyaMeow.Framework.Handlers;
using System.Net;
using System.Text.Json;

namespace controlnyaMeow.Core.Framework
{
    public class HTTPServer
    {
        private HttpListener server = new HttpListener();
        private bool serverRuns = false;
        private Handler chain;

        public HTTPServer()
        {
            string path = "http://127.0.0.1:8888/";
            server.Prefixes.Add(path);
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

        public void ManageServer()
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

        public async Task CycleRequest()
        {
            while (serverRuns)
            {
                HttpListenerContext context;
                try
                {
                    context = await server.GetContextAsync();

                    Console.WriteLine("Пришел запрос");

                    await chain.HandleRequest(context);
                }
                catch (HttpListenerException)
                {
                    break;
                }
                catch (ObjectDisposedException)
                {
                    break;
                }

                //Console.WriteLine($"Обработан запрос: {context.Request.Url} {context.Request.HttpMethod}");
            }
        }
    }
}